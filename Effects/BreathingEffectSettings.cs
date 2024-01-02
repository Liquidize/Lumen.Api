using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;
using Newtonsoft.Json;

namespace Lumen.Api.Effects
{
    public class BreathingEffectSettings : EffectSettings
    {

        [JsonProperty("color")]
        public LedColor Color { get; set; } = ColorLibrary.Orange;

        [JsonProperty("frequency")]
        public double Frequency { get; set; } = 5.0;

        [JsonProperty("palette")]
        public ColorPalette Palette { get; set; } = PaletteLibrary.Rainbow;

        [JsonProperty("usePalette")]
        public bool UsePalette { get; set; } = false;

        public BreathingEffectSettings()
        {

            Lifetime = 5;

        }


    }
}
