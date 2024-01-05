namespace SharpAstrology.Enums;

public enum Nakshatras
{
    Ashvini = 0,
    Bharani = 1,
    Krittika = 2,
    Rohini = 3,
    Mrigashirsha = 4,
    Ardra = 5,
    Punarvasu = 6,
    Pushya = 7,
    Ashlesha = 8,
    Magha = 9,
    PurvaPhalguni = 10,
    UttaraPhalguni = 11,
    Hasta = 12,
    Chitra = 13,
    Svati = 14,
    Vishakha = 15,
    Anuradha = 16,
    Jyeshtha = 17,
    Mula = 18,
    PurvaAshadha = 19,
    UttaraAashadha = 20,
    Shravana = 21,
    Shravishtha = 22,
    Shatabhisha = 23,
    PurvaBhadrapada = 24,
    UttaraBhadrapada = 25,
    Revati = 26
}
public static class NakshatrasExtensionMethods
{
    public static string GetDisplayNameForWheel(this Nakshatras nakshatra) => nakshatra switch
    {
        Nakshatras.Ashvini => "Ashvini",
        Nakshatras.Bharani => "Bharani",
        Nakshatras.Krittika => "Krittika",
        Nakshatras.Rohini => "Rohini",
        Nakshatras.Mrigashirsha => "Mrigashirsha",
        Nakshatras.Ardra => "Ardra",
        Nakshatras.Punarvasu => "Punarvasu",
        Nakshatras.Pushya => "Pushya",
        Nakshatras.Ashlesha => "Ashlesha",
        Nakshatras.Magha => "Magha",
        Nakshatras.PurvaPhalguni => "Purva Phal.",
        Nakshatras.UttaraPhalguni => "Uttara Phal.",
        Nakshatras.Hasta => "Hasta",
        Nakshatras.Chitra => "Chitra",
        Nakshatras.Svati => "Svati",
        Nakshatras.Vishakha => "Vishakha",
        Nakshatras.Anuradha => "Anuradha",
        Nakshatras.Jyeshtha => "Jyeshtha",
        Nakshatras.Mula => "Mula",
        Nakshatras.PurvaAshadha => "Purva Ash.",
        Nakshatras.UttaraAashadha => "Uttara Ash.",
        Nakshatras.Shravana => "Shravana",
        Nakshatras.Shravishtha => "Shravishtha",
        Nakshatras.Shatabhisha => "Shatabhisha",
        Nakshatras.PurvaBhadrapada => "Purva Bhad.",
        Nakshatras.UttaraBhadrapada => "Uttara Bhad.",
        Nakshatras.Revati => "Revati"
    };
}