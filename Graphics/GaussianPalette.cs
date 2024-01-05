using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lumen.Api.Graphics
{
    /// <summary>
    /// Represents a color palette with Gaussian interpolation for LED effects.
    /// </summary>
    public class GaussianPalette : ColorPalette
    {
        /// <summary>
        /// Gets or sets the smoothing factor for Gaussian interpolation.
        /// </summary>
        [JsonProperty("smoothing")]
        public double Smoothing { get; set; } = 0.0;

        /// <summary>
        /// Gets or sets the array of factors used for Gaussian interpolation.
        /// </summary>
        [JsonIgnore]
        public double[] Factors { get; set; } = new[] { 0.06136, 0.24477, 0.38774, 0.24477, 0.06136 };

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianPalette"/> class with the specified array of colors, smoothing factor, and blend setting.
        /// </summary>
        /// <param name="colors">The array of LED colors.</param>
        /// <param name="smoothing">Optional. The smoothing factor for Gaussian interpolation. Default is 0.0.</param>
        /// <param name="blend">Optional. Indicates whether the colors should be blended. Default is true.</param>
        public GaussianPalette(LedColor[] colors, double smoothing = 0.0, bool blend = true) : base(colors, blend)
        {
            Smoothing = smoothing;
        }

        /// <summary>
        /// Gets the interpolated LED color at the specified fractional index using Gaussian interpolation.
        /// </summary>
        /// <param name="d">The fractional index of the color to retrieve.</param>
        /// <returns>The interpolated LED color at the specified fractional index using Gaussian interpolation.</returns>
        public override LedColor this[double d]
        {
            get
            {
                double s = Smoothing / OriginalSize;

                double red = base[d - s * 2].R * Factors[0] +
                             base[d - s].R * Factors[1] +
                             base[d].R * Factors[2] +
                             base[d + s].R * Factors[3] +
                             base[d + s * 2].R * Factors[4];

                double green = base[d - s * 2].G * Factors[0] +
                               base[d - s].G * Factors[1] +
                               base[d].G * Factors[2] +
                               base[d + s].G * Factors[3] +
                               base[d + s * 2].G * Factors[4];

                double blue = base[d - s * 2].B * Factors[0] +
                              base[d - s].B * Factors[1] +
                              base[d].B * Factors[2] +
                              base[d + s].B * Factors[3] +
                              base[d + s * 2].B * Factors[4];

                return new LedColor((byte)red, (byte)green, (byte)blue);
            }
        }
    }

}
