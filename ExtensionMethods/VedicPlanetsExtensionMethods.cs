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
}