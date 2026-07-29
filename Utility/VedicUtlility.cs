using SharpAstrology.Enums;

namespace SharpAstrology.Utility;

public static class VedicAstrologyUtility
{
    /// <summary>The width of one nakshatra in degrees, one twenty-seventh of the circle.</summary>
    public const double NakshatraAngle = 360.0 / 27;

    /// <summary>The width of one pada in degrees, a quarter of a nakshatra.</summary>
    public const double PadaAngle = NakshatraAngle / 4;

    /// <summary>
    /// The nakshatra of a longitude. Expects a sidereal longitude if a nakshatra in the usual
    /// sense is meant. The longitude is normalized, so values outside <c>[0, 360)</c> are allowed.
    /// </summary>
    /// <param name="longitude">The longitude to get the nakshatra of.</param>
    public static Nakshatras NakshatraOf(double longitude)
        => (Nakshatras)(int)(AstrologyUtility.NormalizeDegrees(longitude) / NakshatraAngle);

    /// <summary>
    /// The pada, the quarter within the nakshatra, of a longitude. Expects a sidereal longitude
    /// if a pada in the usual sense is meant. The longitude is normalized, so values outside
    /// <c>[0, 360)</c> are allowed.
    /// </summary>
    /// <param name="longitude">The longitude to get the pada of.</param>
    public static Padas PadaOf(double longitude)
        => (Padas)((int)(AstrologyUtility.NormalizeDegrees(longitude) % NakshatraAngle / PadaAngle) + 1);

    /// <summary>
    /// The whole sign house of a sign, counted from the sign of the ascendant.
    /// This is how a rashi chart numbers its boxes. The sign of the ascendant is the
    /// first house, the next sign the second one, and so on.
    /// </summary>
    /// <remarks>
    /// This method takes signs and no longitudes. A whole sign house is defined over sign
    /// boundaries, so counting sign indices is exact. Deriving the same number from cusp
    /// longitudes is not, because a cusp calculated in the tropical zodiac is no boundary
    /// in the sidereal one.
    ///
    /// It is not called HouseOf, although AstrologyUtility.HouseOf lives in the same
    /// namespace and no name conflict would arise. Two methods of the same name with a
    /// different meaning would be a trap.
    /// </remarks>
    /// <param name="sign">The sign to get the house of.</param>
    /// <param name="ascendantSign">The sign the ascendant stands in, in the same zodiac.</param>
    public static Houses WholeSignHouseOf(Zodiac sign, Zodiac ascendantSign)
        => (Houses)(((int)sign - (int)ascendantSign + 12) % 12 + 1);
}