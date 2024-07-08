using SharpAstrology.Enums;

namespace SharpAstrology.ExtensionMethods;

public static class VedicPlanetsExtensionMethods
{
    public static Dashas ToDasha(this Planets planet) => planet switch
    {
        Planets.SouthNode => Dashas.Ketu,
        Planets.Venus => Dashas.Venus,
        Planets.Sun => Dashas.Sun,
        Planets.Moon => Dashas.Moon,
        Planets.Mars => Dashas.Mars,
        Planets.NorthNode => Dashas.Rahu,
        Planets.Jupiter => Dashas.Jupiter,
        Planets.Saturn => Dashas.Saturn,
        Planets.Mercury => Dashas.Mercury
    };

    public static (int, int) ToNaturalDasha(this Planets planet) => planet switch
    {
        Planets.Moon => (0,1),
        Planets.Mars => (1,3),
        Planets.Mercury => (3,12),
        Planets.Venus => (12,32),
        Planets.Jupiter => (32,50),
        Planets.Sun => (50, 70),
        Planets.Saturn => (70, 120),
        _ => throw new NotSupportedException($"Planet {planet} not supported.")
    };
}