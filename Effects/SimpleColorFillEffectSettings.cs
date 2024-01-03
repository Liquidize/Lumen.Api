using System.Dynamic;
using Lumen.Api.Graphics;
using Newtonsoft.Json;

namespace Lumen.Api.Effects;

public class SimpleColorFillEffectSettings : EffectSettings
{
    [JsonProperty("color")]
    public LedColor Color { get; set; } = ColorLibrary.DarkRed;

    [JsonProperty("everyNth")] public uint EveryNth { get; set; } = 1;
}