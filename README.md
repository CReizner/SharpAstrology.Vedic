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

## Visualizing a chart

SharpAstrology offers a package that allows you to visualize your AstrologyChart via a Blazor component. 
See the project [SharpAstrology.Vedic.BlazorComponents](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents).

![Astro Chart](https://github.com/CReizner/SharpAstrology.Vedic.BlazorComponents/blob/main/.github_assets/vedic_chart.png)