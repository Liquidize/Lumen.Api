using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lumen.Api.Graphics
{
    /// <summary>
    /// Represents a collection of colors used for LED effects.
    /// </summary>
    public class ColorPalette
    {
        /// <summary>
        /// Gets or sets the array of LED colors in the palette.
        /// </summary>
        [JsonProperty("colors")]
        public LedColor[] Colors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the colors should be blended when accessing them with a fractional index.
        /// </summary>
        [JsonProperty("blend")]
        public bool Blend { get; set; } = true;

        /// <summary>
        /// Gets the original size of the color palette.
        /// </summary>
        [JsonIgnore]
        public int OriginalSize
        {
            get
            {
                return Colors.Length;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPalette"/> class with the specified array of colors and blend setting.
        /// </summary>
        /// <param name="colors">The array of LED colors.</param>
        /// <param name="blend">Optional. Indicates whether the colors should be blended. Default is true.</param>
        public ColorPalette(LedColor[] colors, bool blend = true)
        {
            Colors = colors;
            Blend = blend;
        }

        /// <summary>
        /// Gets the LED color at the specified index in the palette.
        /// </summary>
        /// <param name="i">The index of the color to retrieve.</param>
        /// <returns>The LED color at the specified index.</returns>
        [JsonIgnore]
        public virtual LedColor this[int i]
        {
            get
            {
                return Colors[i % Colors.Length];
            }
        }

        /// <summary>
        /// Gets the interpolated LED color at the specified fractional index in the palette.
        /// </summary>
        /// <param name="d">The fractional index of the color to retrieve.</param>
        /// <returns>The interpolated LED color at the specified fractional index.</returns>
        [JsonIgnore]
        public virtual LedColor this[double d]
        {
            get
            {
                while (d < 0)
                {
                    d += 1.0;
                }

                d -= ((long)d);

                double fracPerColor = 1.0 / Colors.Length;
                double indexD = d / fracPerColor;
                int index = (int)(d / fracPerColor) % Colors.Length;
                double fraction = indexD - index;

                LedColor color1 = Colors[index];
                if (!Blend) return color1;

                // Get next color and blend them

                LedColor color2 = Colors[(index + 1) % Colors.Length];
                return color1.Blend(color2, 1 - fraction);
            }
        }
    }

}
