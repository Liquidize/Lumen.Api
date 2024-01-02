using Lumen.Api.Graphics;
using Newtonsoft.Json;

namespace Lumen.Api.Effects
{
    public class ColorWipeEffectSettings : EffectSettings
    {
        [JsonProperty("color")]
        public LedColor Color { get; set; } = ColorLibrary.Orange;
        [JsonProperty("palette")]
        public ColorPalette Palette { get; set; } = PaletteLibrary.Rainbow;
        [JsonProperty("usePalette")]
        public bool UsePalette { get; set; } = true;
        [JsonProperty("fillLastColor")]
        public bool FillLastColor { get; set; } = true;
    }
}