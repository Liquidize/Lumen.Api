using Lumen.Api.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumen.Api.Effects
{
    /// <summary>
    /// A color fill effect, optionally will only fill a specific pixel every Nth pixel
    /// </summary>
    public class SimpleColorFillEffect : LedEffect<SimpleColorFillEffectSettings>
    {

        /// <summary>
        ///  Default constructor used when loading
        /// </summary>
        public SimpleColorFillEffect(ILedCanvas canvas, SimpleColorFillEffectSettings settings) : base(canvas, settings)
        {

        }

        /// <summary>
        /// Render a single frame
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="deltaTime"></param>
        protected override void Render(double deltaTime)
        {
            Canvas.FillSolid(ColorLibrary.Black);
            for (uint i = 0; i < Canvas.PixelCount; i += Settings.EveryNth)
                Canvas.DrawPixel(i, Settings.Color);
        }

    }
}
