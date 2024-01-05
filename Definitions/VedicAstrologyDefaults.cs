using SharpAstrology.Enums;

namespace SharpAstrology.Definitions;

public static class VedicAstrologyDefaults
{
    /// <summary>
    /// The major and minor celestial bodies used by the default vedic astrology system.
    /// </summary>
    public static readonly Planets[] VedicDefaultPlanets = {
        Planets.Sun, Planets.Earth, Planets.Mars, Planets.Mercury,
        Planets.Venus, Planets.Moon, Planets.Jupiter, Planets.Saturn, 
        Planets.NorthNode, Planets.SouthNode
    };
}