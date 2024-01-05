using SharpAstrology.Definitions;
using SharpAstrology.Enums;

namespace SharpAstrology.DataModels;

public sealed class VedicAstrologyChartOptions
{
    public Planets[] IncludeObjects { get; set; } = VedicAstrologyDefaults.VedicDefaultPlanets;
}