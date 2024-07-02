using System.Collections.ObjectModel;
using SharpAstrology.Enums;

namespace SharpAstrology.Definitions;

public static class VedicAstrologyDefaults
{
    /// <summary>
    /// The major and minor celestial bodies used by the default vedic astrology system.
    /// </summary>
    public static readonly IReadOnlyList<Planets> VedicDefaultPlanets = [
        Planets.Sun, Planets.Earth, Planets.Mars, Planets.Mercury,
        Planets.Venus, Planets.Moon, Planets.Jupiter, Planets.Saturn, 
        Planets.NorthNode, Planets.SouthNode
    ];

    public static readonly IReadOnlyDictionary<Dashas, double> DashaPeriods = new Dictionary<Dashas, double>
    {
        [Dashas.Ketu] = 7.0,
        [Dashas.Venus] = 20.0,
        [Dashas.Sun] = 6.0,
        [Dashas.Moon] = 10.0,
        [Dashas.Mars] = 7.0,
        [Dashas.Rahu] = 18.0,
        [Dashas.Jupiter] = 16.0,
        [Dashas.Saturn] = 19.0,
        [Dashas.Mercury] = 17.0
    };

    public static readonly IReadOnlyDictionary<Nakshatras, Planets> DefaultNakshatraRulers =
        new Dictionary<Nakshatras, Planets>
        {
            [Nakshatras.Ashvini] = Planets.SouthNode,
            [Nakshatras.Bharani] = Planets.Venus,
            [Nakshatras.Krittika] = Planets.Sun,
            [Nakshatras.Rohini] = Planets.Moon,
            [Nakshatras.Mrigashirsha] = Planets.Mars,
            [Nakshatras.Ardra] = Planets.NorthNode,
            [Nakshatras.Punarvasu] = Planets.Jupiter,
            [Nakshatras.Pushya] = Planets.Saturn,
            [Nakshatras.Ashlesha] = Planets.Mercury,
            [Nakshatras.Magha] = Planets.SouthNode,
            [Nakshatras.PurvaPhalguni] = Planets.Venus,
            [Nakshatras.UttaraPhalguni] = Planets.Sun,
            [Nakshatras.Hasta] = Planets.Moon,
            [Nakshatras.Chitra] = Planets.Mars,
            [Nakshatras.Svati] = Planets.NorthNode,
            [Nakshatras.Vishakha] = Planets.Jupiter,
            [Nakshatras.Anuradha] = Planets.Saturn,
            [Nakshatras.Jyeshtha] = Planets.Mercury,
            [Nakshatras.Mula] = Planets.SouthNode,
            [Nakshatras.PurvaAshadha] = Planets.Venus,
            [Nakshatras.UttaraAashadha] = Planets.Sun,
            [Nakshatras.Shravana] = Planets.Moon,
            [Nakshatras.Shravishtha] = Planets.Mars,
            [Nakshatras.Shatabhisha] = Planets.NorthNode,
            [Nakshatras.PurvaBhadrapada] = Planets.Jupiter,
            [Nakshatras.UttaraBhadrapada] = Planets.Saturn,
            [Nakshatras.Revati] = Planets.Mercury
        };
    
}