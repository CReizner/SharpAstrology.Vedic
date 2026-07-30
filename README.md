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
// ayanamsa most Vedic astrologers use, and it decides where sidereal Aries begins.
var service = new SwissEphemeridesService(ephType: EphType.Moshier);
using var eph = service.CreateContext(Ayanamsas.Lahiri);

// EphCalculationMode.Sidereal puts the whole chart into the sidereal zodiac, the planets,
// the house cusps and the axes alike. That is the zodiac of Vedic astrology, so a rashi
// chart is built this way. With HouseSystems.WholeSign the cusps then land exactly on the
// boundaries of the constellations, which is what whole sign houses are defined over.
var chart = new AstrologyChart(
    new DateTime(1988, 9, 4, 1, 15, 0, DateTimeKind.Utc), eph,
    latitude: 51.0, longitude: 11.0,
    houseSystem: HouseSystems.WholeSign,
    mode: EphCalculationMode.Sidereal);

Console.WriteLine($"Ayanamsa: {chart.Ayanamsa.ToString("F4", CultureInfo.InvariantCulture)}");

// SignOf reads the divisions of the zodiac the chart stands in. Here those are the
// constellations, because the chart was calculated sidereally. chart.CalculationMode says so.
Console.WriteLine($"Moon:  {chart.SignOf(Planets.Moon)}, "
                  + $"{chart.NakshatraOf(Planets.Moon)} pada {(int)chart.PadaOf(Planets.Moon)}");

var lagna = chart.SignOf(Cross.Asc);
Console.WriteLine($"Lagna: {lagna}, {chart.NakshatraOf(Cross.Asc)} pada {(int)chart.PadaOf(Cross.Asc)}");

// The whole sign houses of a rashi chart are counted from the constellation of the lagna.
foreach (var planet in new[] { Planets.Sun, Planets.Moon, Planets.Mars })
{
    var sign = chart.SignOf(planet);
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

## Which zodiac does the chart stand in?

A chart stands in exactly one zodiac. It is chosen when the chart is built and it holds for the
planets, the house cusps and the axes alike. `chart.CalculationMode` says which one it is,
`EphCalculationMode.Tropic` for the signs and `EphCalculationMode.Sidereal` for the constellations.
Whoever needs both zodiacs builds two charts.

The methods of this package divide the longitudes the chart stores. They never shift them, so the
zodiac of the chart is the zodiac of the answer. Build the chart with `EphCalculationMode.Sidereal`
and `NakshatraOf`, `PadaOf` and `Dashas` give the nakshatras, padas and the vimshottari chain of
Vedic astrology.

A tropical chart is accepted as well and then divides the tropical zodiac into twenty-seven parts.
That is a different answer, about two nakshatras away from the Vedic one, and nothing in this
package corrects or refuses it. The dashas are the place where it costs the most, because the moon
also fixes where the chain of a hundred and twenty years starts. Choosing the zodiac is the one
decision this package leaves to you.

Two more things follow the same rule. With `HouseSystems.WholeSign` a sidereal chart puts the cusps
on the boundaries of the constellations, while a tropical chart puts them on the boundaries of the
signs. And `SignOf` names constellations only on a sidereal chart, which is what a Vedic reading of
the chart is about.

A more accurate calculation needs the ephemeris files.
[SharpAstrology.SwissEph](https://github.com/CReizner/SharpAstrology.SwissEph) describes which
files those are, where they belong and how the service is set up to read them.

## Visualizing a chart

SharpAstrology offers a package that allows you to visualize your AstrologyChart via a Blazor component. 
See the project [SharpAstrology.Vedic.BlazorComponents](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents).

![Astro Chart](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents/blob/main/.github_assets/vedic_chart.png)