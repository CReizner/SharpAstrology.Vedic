using SharpAstrology.Enums;
using SharpAstrology.ExtensionMethods;
using SharpAstrology.Interfaces;

namespace SharpAstrology.DataModels;

public sealed class VedicAstrologyChart
{
    public Dictionary<Planets, PlanetPosition> PlanetsPositions { get; set; }
    public HousePosition HousePositions { get; set; }
    public bool HousesAvailable { get; set; }
    
    public static VedicAstrologyChart FromPointInTime(DateTime pointInTime, IEphemerides eph, VedicAstrologyChartOptions? options = null)
    {
        options ??= new VedicAstrologyChartOptions();
        var planetPositions = eph.PlanetsPosition(options.IncludeObjects, pointInTime);
        return new VedicAstrologyChart()
        {
            PlanetsPositions = planetPositions,
            HousesAvailable = false
        };
    }

    public static VedicAstrologyChart FromPointInTimeAndSpace(DateTime pointInTime, double latitude, double longitude,
        IEphemerides eph, VedicAstrologyChartOptions? options = null)
    {
        var chartModel = FromPointInTime(pointInTime, eph, options);
        chartModel.HousePositions = eph
            .HouseCuspPositions(pointInTime, latitude, longitude);
        chartModel.HousesAvailable = true;

        return chartModel;
    }
}