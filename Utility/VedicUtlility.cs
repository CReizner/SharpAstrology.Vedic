using SharpAstrology.Enums;

namespace SharpAstrology.Utility;

public static class VedicAstrologyUtility
{
    public static Nakshatras NakshatraOf(double longitude) => (Nakshatras)(longitude / 13.333333);
}