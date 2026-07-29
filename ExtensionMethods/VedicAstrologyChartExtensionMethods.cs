using SharpAstrology.DataModels;
using SharpAstrology.Definitions;
using SharpAstrology.Enums;
using SharpAstrology.Exceptions;
using SharpAstrology.Utility;
using SharpAstrology.Vedic.DataModels;

namespace SharpAstrology.ExtensionMethods;

public static class VedicAstrologyChartExtensionMethods
{
    private const double DaysPerYear = 365.242190;

    #region Sidereal longitudes

    // The sidereal longitude, without using the overload PositionOf(planet, true). Up to
    // SharpAstrology.Base 0.12.0 that one subtracted the ayanamsa on the stored
    // PlanetPosition and damaged the chart. Subtracting here keeps this library
    // independent of the version of Base.
    private static double SiderealLongitude(AstrologyChart chart, Planets planet)
        => AstrologyUtility.SubtractDegree(chart.PositionOf(planet).Longitude, chart.Ayanamsa);

    private static double SiderealLongitude(AstrologyChart chart, Houses house)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return AstrologyUtility.SubtractDegree(chart.HousePositions.HouseCusps[house], chart.Ayanamsa);
    }

    private static double SiderealLongitude(AstrologyChart chart, Cross direction)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return AstrologyUtility.SubtractDegree(chart.HousePositions.Cross[direction], chart.Ayanamsa);
    }

    #endregion

    #region Nakshatra and pada

    /// <summary>
    /// The nakshatra of a planet, calculated in the sidereal zodiac of this chart.
    /// Use <see cref="VedicAstrologyUtility.NakshatraOf(double)"/> to apply the
    /// twenty-seven-fold division to a longitude of your own choice.
    /// </summary>
    /// <param name="chart">The chart to read the position from.</param>
    /// <param name="planet">The planet to get the nakshatra of.</param>
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Planets planet)
        => VedicAstrologyUtility.NakshatraOf(SiderealLongitude(chart, planet));

    /// <summary>
    /// The nakshatra of a house cusp, calculated in the sidereal zodiac of this chart.
    /// </summary>
    /// <param name="chart">The chart to read the cusp from.</param>
    /// <param name="house">The house cusp to get the nakshatra of.</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Houses house)
        => VedicAstrologyUtility.NakshatraOf(SiderealLongitude(chart, house));

    /// <summary>
    /// The nakshatra of a direction of the cross, calculated in the sidereal zodiac of this chart.
    /// </summary>
    /// <param name="chart">The chart to read the direction from.</param>
    /// <param name="direction">The direction of the cross (e.g., Ascendant, Midheaven).</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Cross direction)
        => VedicAstrologyUtility.NakshatraOf(SiderealLongitude(chart, direction));

    /// <summary>
    /// The pada of a planet, calculated in the sidereal zodiac of this chart.
    /// Use <see cref="VedicAstrologyUtility.PadaOf(double)"/> to apply the division to a
    /// longitude of your own choice.
    /// </summary>
    /// <param name="chart">The chart to read the position from.</param>
    /// <param name="planet">The planet to get the pada of.</param>
    public static Padas PadaOf(this AstrologyChart chart, Planets planet)
        => VedicAstrologyUtility.PadaOf(SiderealLongitude(chart, planet));

    /// <summary>
    /// The pada of a house cusp, calculated in the sidereal zodiac of this chart.
    /// </summary>
    /// <param name="chart">The chart to read the cusp from.</param>
    /// <param name="house">The house cusp to get the pada of.</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Padas PadaOf(this AstrologyChart chart, Houses house)
        => VedicAstrologyUtility.PadaOf(SiderealLongitude(chart, house));

    /// <summary>
    /// The pada of a direction of the cross, calculated in the sidereal zodiac of this chart.
    /// </summary>
    /// <param name="chart">The chart to read the direction from.</param>
    /// <param name="direction">The direction of the cross (e.g., Ascendant, Midheaven).</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Padas PadaOf(this AstrologyChart chart, Cross direction)
        => VedicAstrologyUtility.PadaOf(SiderealLongitude(chart, direction));

    #endregion

    #region Dashas

    /// <summary>
    /// The maha dasha that is active now. The chain covers 120 years from birth.
    /// </summary>
    /// <param name="chart">The chart to calculate the dashas of.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the current point in time lies outside the 120 years the chain covers.
    /// </exception>
    public static Dashas CurrentMahaDasha(this AstrologyChart chart)
        => chart.Dashas().CurrentDasha().Dasha;

    public static DashaCalculationResult Dashas(this AstrologyChart chart)
    {
        // The fraction of the nakshatra already passed comes from the same sidereal longitude
        // as the nakshatra itself. This line used to read the longitude that the call to
        // NakshatraOf had just shortened inside the chart, so a second call to Dashas() on the
        // same chart returned a different chain.
        var moon = SiderealLongitude(chart, Planets.Moon);
        var initialNakshatra = VedicAstrologyUtility.NakshatraOf(moon);
        var periodPassed = moon % VedicAstrologyUtility.NakshatraAngle / VedicAstrologyUtility.NakshatraAngle;
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

    #endregion
}