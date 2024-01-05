using SharpAstrology.DataModels;
using SharpAstrology.Enums;
using SharpAstrology.Utility;

namespace SharpAstrology.ExtensionMethods;

public static class VedicAstrologyChartExtensionMethods
{
    public static bool PlanetIsInNakshatra(this VedicAstrologyChart chart, 
        Planets planet, Nakshatras nakshatra, Padas? pada = null)
    {
        var longitude = chart.PlanetsPositions[planet].Longitude;
        if (nakshatra != VedicAstrologyUtility.NakshatraOf(longitude)) return false;
        if (pada is null) return true;
        var deg = longitude % 13.333333;
        return pada switch
        {
            Padas.First when deg < 3.33333325 => true,
            Padas.Second when deg >= 3.33333325 & deg < 6.6666665 => true,
            Padas.Third when deg >= 6.6666665 & deg < 9.99999975 => true,
            Padas.Fourth when deg >= 9.99999975 & deg < 13.333333 => true,
            _ => false
        };
    }
    
    public static bool RulerIsInNakshatra(this VedicAstrologyChart chart,
        RulerOf ruler, Nakshatras nakshatra, Dictionary<ZodiacSigns, Planets> rulership, Padas? pada = null)
    {
        if (!chart.HousesAvailable) return false;
        var inSign = AstrologyUtility.ZodiacSignOf(chart.HousePositions.HouseCusps[ruler.ToHouse()]);
        var planet = rulership[inSign];
        return chart.PlanetIsInNakshatra(planet, nakshatra, pada);
    }
    
    public static bool HouseIsInNakshatra(this VedicAstrologyChart chart, 
        Houses house, Nakshatras nakshatra, Padas? pada = null)
    {
        if (!chart.HousesAvailable) return false;
        var longitude = chart.HousePositions.HouseCusps[house];
        if (nakshatra != VedicAstrologyUtility.NakshatraOf(longitude)) return false;
        if (pada is null) return true;
        var deg = longitude % 13.333333;
        return pada switch
        {
            Padas.First when deg < 3.33333325 => true,
            Padas.Second when deg >= 3.33333325 & deg < 6.6666665 => true,
            Padas.Third when deg >= 6.6666665 & deg < 9.99999975 => true,
            Padas.Fourth when deg >= 9.99999975 & deg < 13.333333 => true,
            _ => false
        };
    }
    
    public static bool CrossIsInNakshatra(this VedicAstrologyChart chart, 
        Cross direction, Nakshatras nakshatra, Padas? pada = null)
    {
        if (!chart.HousesAvailable) return false;
        var longitude = chart.HousePositions.Cross[direction];
        if (nakshatra != VedicAstrologyUtility.NakshatraOf(longitude)) return false;
        if (pada is null) return true;
        var deg = longitude % 13.333333;
        return pada switch
        {
            Padas.First when deg < 3.33333325 => true,
            Padas.Second when deg >= 3.33333325 & deg < 6.6666665 => true,
            Padas.Third when deg >= 6.6666665 & deg < 9.99999975 => true,
            Padas.Fourth when deg >= 9.99999975 & deg < 13.333333 => true,
            _ => false
        };
    }

}