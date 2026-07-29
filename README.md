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

# Examples

## Which house does a sign stand in?
A rashi chart numbers its boxes from the box of the ascendant. The sign of the ascendant is the
first house, the next sign the second one, and so on. `VedicAstrologyUtility.WholeSignHouseOf`
does that counting.

```csharp
using SharpAstrology.Enums;
using SharpAstrology.Utility;

var lagna = Zodiac.Cancer;

// Houses.House1
var houseOfCancer = VedicAstrologyUtility.WholeSignHouseOf(Zodiac.Cancer, lagna);

// Houses.House10, the count wraps around Aries
var houseOfAries = VedicAstrologyUtility.WholeSignHouseOf(Zodiac.Aries, lagna);
```

The method takes signs and no longitudes. That is the point. A whole sign house is defined over
sign boundaries, so counting sign indices is exact. Reading the same number from the house cusps
of a chart is not, because a cusp calculated in the tropical zodiac is no boundary in the
sidereal one.

Both signs have to come from the same zodiac. If the chart was calculated tropically and is
displayed sidereally, subtract the ayanamsa from both longitudes before taking their signs.

## Nakshatra and pada

`VedicAstrologyUtility` calculates both over a longitude, and the extension methods on
`AstrologyChart` do it in the sidereal zodiac of the chart.

```csharp
using SharpAstrology.Enums;
using SharpAstrology.ExtensionMethods;
using SharpAstrology.Utility;

// Over the chart. The ayanamsa of the chart is subtracted, the chart stays untouched.
var nakshatraOfTheMoon = chart.NakshatraOf(Planets.Moon);
var padaOfTheAscendant = chart.PadaOf(Cross.Asc);

// Over a longitude, for a zodiac of your own choice. Values outside 0 to 360 are normalized.
var nakshatra = VedicAstrologyUtility.NakshatraOf(138.0278);
var pada = VedicAstrologyUtility.PadaOf(138.0278);
```

