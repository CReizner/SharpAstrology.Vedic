using SharpAstrology.DataModels;
using SharpAstrology.Definitions;
using SharpAstrology.Enums;
using SharpAstrology.Exceptions;
using SharpAstrology.Utility;
using SharpAstrology.Vedic.DataModels;

namespace SharpAstrology.ExtensionMethods;

/// <summary>
/// The Vedic divisions of a chart, the nakshatra, the pada and the vimshottari dashas.
/// </summary>
/// <remarks>
/// A chart stands in exactly one zodiac, chosen when it is built, and
/// <see cref="AstrologyChart.CalculationMode"/> says which one. Every method here divides the
/// longitude the chart stores and never shifts it, so the zodiac of the chart is the zodiac of the
/// answer.
///
/// A tropical chart is allowed on purpose. It answers, and it answers about the tropical zodiac.
/// Whoever wants Vedic answers builds the chart with <see cref="EphCalculationMode.Sidereal"/>. That
/// is the one decision this package leaves to the caller, and it is not checked anywhere.
/// </remarks>
public static class VedicAstrologyChartExtensionMethods
{
    private const double DaysPerYear = 365.242190;

    #region Nakshatra and pada

    /// <summary>
    /// The nakshatra of a planet, read in the zodiac this chart stands in. A chart built with
    /// <see cref="EphCalculationMode.Sidereal"/> gives the nakshatra of Vedic astrology. A tropical
    /// chart divides the tropical zodiac into twenty-seven parts instead, which is a different
    /// answer and is not corrected here.
    /// Use <see cref="VedicAstrologyUtility.NakshatraOf(double)"/> to apply the
    /// twenty-seven-fold division to a longitude of your own choice.
    /// </summary>
    /// <param name="chart">The chart to read the position from.</param>
    /// <param name="planet">The planet to get the nakshatra of.</param>
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Planets planet)
        => VedicAstrologyUtility.NakshatraOf(chart.PositionOf(planet).Longitude);

    /// <summary>
    /// The nakshatra of a house cusp, read in the zodiac this chart stands in.
    /// </summary>
    /// <param name="chart">The chart to read the cusp from.</param>
    /// <param name="house">The house cusp to get the nakshatra of.</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Houses house)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return VedicAstrologyUtility.NakshatraOf(chart.HousePositions.HouseCusps[house]);
    }

    /// <summary>
    /// The nakshatra of a direction of the cross, read in the zodiac this chart stands in.
    /// </summary>
    /// <param name="chart">The chart to read the direction from.</param>
    /// <param name="direction">The direction of the cross (e.g., Ascendant, Midheaven).</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Nakshatras NakshatraOf(this AstrologyChart chart, Cross direction)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return VedicAstrologyUtility.NakshatraOf(chart.HousePositions.Cross[direction]);
    }

    /// <summary>
    /// The pada of a planet, read in the zodiac this chart stands in. Build the chart with
    /// <see cref="EphCalculationMode.Sidereal"/> for the pada of Vedic astrology.
    /// Use <see cref="VedicAstrologyUtility.PadaOf(double)"/> to apply the division to a
    /// longitude of your own choice.
    /// </summary>
    /// <param name="chart">The chart to read the position from.</param>
    /// <param name="planet">The planet to get the pada of.</param>
    public static Padas PadaOf(this AstrologyChart chart, Planets planet)
        => VedicAstrologyUtility.PadaOf(chart.PositionOf(planet).Longitude);

    /// <summary>
    /// The pada of a house cusp, read in the zodiac this chart stands in.
    /// </summary>
    /// <param name="chart">The chart to read the cusp from.</param>
    /// <param name="house">The house cusp to get the pada of.</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Padas PadaOf(this AstrologyChart chart, Houses house)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return VedicAstrologyUtility.PadaOf(chart.HousePositions.HouseCusps[house]);
    }

    /// <summary>
    /// The pada of a direction of the cross, read in the zodiac this chart stands in.
    /// </summary>
    /// <param name="chart">The chart to read the direction from.</param>
    /// <param name="direction">The direction of the cross (e.g., Ascendant, Midheaven).</param>
    /// <exception cref="HousesNotAvailableException">Thrown if the house positions are not available.</exception>
    public static Padas PadaOf(this AstrologyChart chart, Cross direction)
    {
        if (chart.HousePositions is null) throw new HousesNotAvailableException();
        return VedicAstrologyUtility.PadaOf(chart.HousePositions.Cross[direction]);
    }

    #endregion

    #region Dashas

    /// <summary>
    /// The maha dasha that is active now. The chain covers 120 years from birth.
    /// The moon of this chart fixes where it starts, so the chart has to stand in the sidereal
    /// zodiac for the chain to be the vimshottari chain of Vedic astrology.
    /// </summary>
    /// <param name="chart">The chart to calculate the dashas of.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the current point in time lies outside the 120 years the chain covers.
    /// </exception>
    public static Dashas CurrentMahaDasha(this AstrologyChart chart)
        => chart.Dashas().CurrentDasha().Dasha;

    public static DashaCalculationResult Dashas(this AstrologyChart chart)
    {
        // The moon longitude does more here than name a nakshatra. The fraction of that nakshatra
        // already passed fixes where the chain of a hundred and twenty years starts, so a moon in
        // the wrong zodiac moves every maha, antar and pratyantar period at once and the result
        // still looks plausible. Build the chart sidereally.
        var moon = chart.PositionOf(Planets.Moon).Longitude;
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