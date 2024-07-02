using SharpAstrology.Enums;

namespace SharpAstrology.Vedic.DataModels;

public sealed class DashaCalculationResult
{
    public List<MahaDashaEpisode> MahaDashaEpisodes { get; } = new(9);

    public MahaDashaEpisode CurrentDasha()
    {
        return MahaDashaEpisodes.First(d => d.CurrentlyActive);
    }
}

public sealed class MahaDashaEpisode
{
    public Dashas Dasha { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public bool CurrentlyActive { get; init; }

    public List<AntarDashaEpisode> AntarDashaEpisodes { get; } = new(9);
    
    public AntarDashaEpisode CurrentDasha()
    {
        return AntarDashaEpisodes.First(d => d.CurrentlyActive);
    }
}

public sealed class AntarDashaEpisode
{
    public Dashas Dasha { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public bool CurrentlyActive { get; init; }

    public List<PratyantarDashaEpisode> PratyantarDashaEpisodes { get; } = new(9);
    
    public PratyantarDashaEpisode CurrentDasha()
    {
        return PratyantarDashaEpisodes.First(d => d.CurrentlyActive);
    }
}

public sealed class PratyantarDashaEpisode
{
    public Dashas Dasha { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public bool CurrentlyActive { get; init; }
}

