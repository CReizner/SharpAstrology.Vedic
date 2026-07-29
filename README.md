# SharpAstrology.Vedic


## SharpAstrology Packages
| Package                                                                                                                | Description                                            | Licence  |
|:-----------------------------------------------------------------------------------------------------------------------|:-------------------------------------------------------|:--------:|
| [SharpAstrology.Base](https://github.com/CReizner/SharpAstrology.Base)                                                 | Base library                                           |   MIT    |
| [SharpAstrology.SwissEph](https://github.com/CReizner/SharpAstrology.SwissEph)                                         | Ephemerides package based on SwissEphNet               | AGPL-3.0 |
| [SharpAstrology.Symbols.BlazorComponents](https://github.com/CReizner/SharpAstrology.Symbols.BlazorComponents)         | Astrological symbols as Blazor components              |   MIT    |
| [SharpAstrology.HumanDesign](https://github.com/CReizner/SharpAstrology.HumanDesign)                                   | Extensions for the Human Design system                 |   MIT    |
| [SharpAstrology.HumanDesign.BlazorComponents](https://github.com/CReizner/SharpAstrology.HumanDesign.BlazorComponents) | Human Design charts as Blazor components               |   MIT    |
| [SharpAstrology.Vedic](https://github.com/CReizner/SharpAstrology.Vedic)                                               | Extensions for Vedic astrology systems                 |   MIT    |
| [SharpAstrology.Vedic.BlazorComponents](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents)             | Vedic astrology charts as Blazor components            |   MIT    |
| [SharpAstrology.West](https://github.com/CReizner/SharpAstrology.West)                                                 | Extensions for western astrology systems               |   MIT    |
| [SharpAstrology.West.BlazorComponents](https://github.com/CReizner/SharpAstrology.West.BlazorComponents)               | Western astrology charts as Blazor components          |   MIT    |
| [SharpAstrology.WebApp](https://github.com/CReizner/SharpAstrology.WebApp)                                             | Blazor Server app built on the SharpAstrology packages | AGPL-3.0 |


## Install
```dotnet add package SharpAstrology.Vedic```

This package extends the `AstrologyChart` class of SharpAstrology.Base. The positions themselves
come from an ephemerides package. The examples below use
[SharpAstrology.SwissEph](https://github.com/CReizner/SharpAstrology.SwissEph).

```dotnet add package SharpAstrology.SwissEph```

## Minimal example

A console program that calculates a rashi chart and reads the nakshatra, the pada, the whole sign
houses and the current dasha from it.

```csharp
using System.Globalization;
using SharpAstrology.DataModels;
using SharpAstrology.Enums;
using SharpAstrology.Ephemerides;
using SharpAstrology.ExtensionMethods;
using SharpAstrology.Utility;

// Moshier needs no ephemeris files, so this example runs as it stands. Lahiri is the
// ayanamsa most Vedic astrologers use, and it is the number the extension methods subtract.
var service = new SwissEphemeridesService(ephType: EphType.Moshier);
using var eph = service.CreateContext(Ayanamsas.Lahiri);

// The chart is calculated tropically, which is the default. The sidereal zodiac comes
// from the ayanamsa the chart carries, and the extension methods subtract it.
var chart = new AstrologyChart(
    new DateTime(1988, 9, 4, 1, 15, 0, DateTimeKind.Utc), eph,
    latitude: 51.0, longitude: 11.0,
    houseSystem: HouseSystems.WholeSign);

Console.WriteLine($"Ayanamsa: {chart.Ayanamsa.ToString("F4", CultureInfo.InvariantCulture)}");

// Moon and lagna in the sidereal zodiac, with nakshatra and pada.
Console.WriteLine($"Moon:  {chart.ConstellationOf(Planets.Moon)}, "
                  + $"{chart.NakshatraOf(Planets.Moon)} pada {(int)chart.PadaOf(Planets.Moon)}");

var lagna = chart.ConstellationOf(Cross.Asc);
Console.WriteLine($"Lagna: {lagna}, {chart.NakshatraOf(Cross.Asc)} pada {(int)chart.PadaOf(Cross.Asc)}");

// The whole sign houses of a rashi chart are counted from the sign of the lagna.
foreach (var planet in new[] { Planets.Sun, Planets.Moon, Planets.Mars })
{
    var sign = chart.ConstellationOf(planet);
    Console.WriteLine($"{planet,-6} {sign,-11} house {(int)VedicAstrologyUtility.WholeSignHouseOf(sign, lagna)}");
}

// The vimshottari chain covers 120 years from birth. CurrentDasha throws once that is over.
var maha = chart.Dashas().CurrentDasha();
var antar = maha.CurrentDasha();
Console.WriteLine($"Maha dasha:  {maha.Dasha} from {maha.Start:yyyy-MM-dd} to {maha.End:yyyy-MM-dd}");
Console.WriteLine($"Antar dasha: {antar.Dasha} from {antar.Start:yyyy-MM-dd} to {antar.End:yyyy-MM-dd}");
```

The output, run in July 2026. The two dasha lines move with the current date, the rest does not.

```
Ayanamsa: 23.7004
Moon:  Taurus, Mrigashirsha pada 2
Lagna: Cancer, Pushya pada 3
Sun    Leo         house 2
Moon   Taurus      house 11
Mars   Pisces      house 9
Maha dasha:  Jupiter from 2010-11-08 to 2026-11-08
Antar dasha: Rahu from 2024-06-14 to 2026-11-08
```

A more accurate calculation needs the ephemeris files.
[SharpAstrology.SwissEph](https://github.com/CReizner/SharpAstrology.SwissEph) describes which
files those are, where they belong and how the service is set up to read them.

## Visualizing a chart

SharpAstrology offers a package that allows you to visualize your AstrologyChart via a Blazor component. 
See the project [SharpAstrology.Vedic.BlazorComponents](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents).

![Astro Chart](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents/blob/main/.github_assets/vedic_chart.png)