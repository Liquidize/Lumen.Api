using Lumen.Api.Graphics;
using Newtonsoft.Json;

namespace Lumen.Api.Effects;

public class SimpleColorFillEffectSettings : EffectSettings
{
    [JsonProperty("color")]
    public LedColor Color = ColorLibrary.DarkRed;
    [JsonProperty("everyNth")]
    public uint EveryNth = 1;
}