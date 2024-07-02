using System.Diagnostics;
using SharpAstrology.DataModels;
using SharpAstrology.Definitions;
using SharpAstrology.Enums;
using SharpAstrology.Exceptions;
using SharpAstrology.Utility;
using SharpAstrology.Vedic.DataModels;

namespace SharpAstrology.ExtensionMethods;

public static class VedicAstrologyChartExtensionMethods
{
    private const double NakshatraAngle = 13.333333;
    private const double PadaAngle = NakshatraAngle / 4;
    private const double DaysPerYear = 365.242190;
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Planets planet)
    {
        return VedicAstrologyUtility.NakshatraOf(chart.PositionOf(planet, true).Longitude);
    }
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Houses house)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return VedicAstrologyUtility.NakshatraOf(chart.HousePositions.HouseCusps[house] - chart.Ayanamsa);
    }
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Cross direction)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return VedicAstrologyUtility.NakshatraOf(chart.HousePositions.Cross[direction] - chart.Ayanamsa);
    }

    public static Padas PadaOf(this AstrologyChart chart, Planets planet)
    {
        return (chart.PositionOf(planet, true).Longitude % NakshatraAngle) switch
        {
            < PadaAngle => Padas.First,
            < 2*PadaAngle and >= PadaAngle => Padas.Second,
            < 3*PadaAngle and >= 2*PadaAngle => Padas.Third,
            < 4*PadaAngle and >= 3*PadaAngle => Padas.Fourth
        };
    }
    public static Padas PadaOf(this AstrologyChart chart, Houses house)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return ((chart.HousePositions.HouseCusps[house]-chart.Ayanamsa) % 13.333333) switch
        {
            < PadaAngle => Padas.First,
            < 2*PadaAngle and >= PadaAngle => Padas.Second,
            < 3*PadaAngle and >= 2*PadaAngle => Padas.Third,
            < 4*PadaAngle and >= 3*PadaAngle => Padas.Fourth
        };
    }
    public static Padas PadaOf(this AstrologyChart chart, Cross direction)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return ((chart.HousePositions.Cross[direction]-chart.Ayanamsa) % 13.333333) switch
        {
            < PadaAngle => Padas.First,
            < 2*PadaAngle and >= PadaAngle => Padas.Second,
            < 3*PadaAngle and >= 2*PadaAngle => Padas.Third,
            < 4*PadaAngle and >= 3*PadaAngle => Padas.Fourth
        };
    }

    public static Dashas CurrentMahaDasha(this AstrologyChart chart)
    {
        throw new NotImplementedException();
    }

    public static DashaCalculationResult Dashas(this AstrologyChart chart)
    {
        var initialNakshatra = chart.NakshatraOf(Planets.Moon);
        var periodPassed = (chart.PositionOf(Planets.Moon).Longitude % NakshatraAngle) / NakshatraAngle;
        var initialDasha = VedicAstrologyDefaults.DefaultNakshatraRulers[initialNakshatra].ToDasha();
        var periodStart = chart.PointInTime -
            TimeSpan.FromDays(DaysPerYear * VedicAstrologyDefaults.DashaPeriods[initialDasha] * periodPassed);
        var result = new DashaCalculationResult();
        var now = DateTime.UtcNow;
        foreach (var mahaDasha in new Cycle<Dashas>(Enum.GetValues<Dashas>(), initialDasha))
        {
            var periodEnd = periodStart + TimeSpan.FromDays(DaysPerYear * VedicAstrologyDefaults.DashaPeriods[mahaDasha]);
            var mahaDashaEpisode = new MahaDashaEpisode
            {
                Dasha = mahaDasha,
                Start = periodStart,
                End = periodEnd,
                CurrentlyActive = now < periodEnd && now >= periodStart
            };

            var antarPeriodStart = periodStart;
            var antarDashaTimeFragment = DaysPerYear * VedicAstrologyDefaults.DashaPeriods[mahaDasha] / 120;
            foreach (var antarDasha in new Cycle<Dashas>(Enum.GetValues<Dashas>(), mahaDasha))
            {
                var antarPeriodEnd = antarPeriodStart +
                                     TimeSpan.FromDays(antarDashaTimeFragment * VedicAstrologyDefaults.DashaPeriods[antarDasha]);
                var antarDashaEpisode = new AntarDashaEpisode
                {
                    Dasha = antarDasha,
                    Start = antarPeriodStart,
                    End = antarPeriodEnd,
                    CurrentlyActive = now < antarPeriodEnd && now >= antarPeriodStart
                };

                var pretyAntarPeriodStart = antarPeriodStart;
                var pretyAntarDashaTimeFragment = antarDashaTimeFragment * VedicAstrologyDefaults.DashaPeriods[antarDasha] / 120;
                foreach (var pretyAntarDasha in new Cycle<Dashas>(Enum.GetValues<Dashas>(), antarDasha))
                {
                    var pretyAntarDashaEnd = pretyAntarPeriodStart +
                                             TimeSpan.FromDays(pretyAntarDashaTimeFragment *
                                                               VedicAstrologyDefaults.DashaPeriods[pretyAntarDasha]);
                    antarDashaEpisode.PratyantarDashaEpisodes.Add(new()
                    {
                        Dasha = pretyAntarDasha,
                        Start = pretyAntarPeriodStart,
                        End = pretyAntarDashaEnd,
                        CurrentlyActive = now < pretyAntarDashaEnd && now >= pretyAntarPeriodStart
                    });
                    pretyAntarPeriodStart += TimeSpan.FromDays(
                        pretyAntarDashaTimeFragment * VedicAstrologyDefaults.DashaPeriods[pretyAntarDasha]);
                }
                
                antarPeriodStart += TimeSpan.FromDays(antarDashaTimeFragment * VedicAstrologyDefaults.DashaPeriods[antarDasha]);
                mahaDashaEpisode.AntarDashaEpisodes.Add(antarDashaEpisode);
            }
            
            periodStart += TimeSpan.FromDays(DaysPerYear * VedicAstrologyDefaults.DashaPeriods[mahaDasha]);
            result.MahaDashaEpisodes.Add(mahaDashaEpisode);
        }

        return result;
    }
}