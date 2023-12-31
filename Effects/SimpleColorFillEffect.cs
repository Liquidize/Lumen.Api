using Lumen.Api.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumen.Api.Effects
{
    
    public class SimpleColorFillEffect : LedEffect
    {
    
        protected LedColor Color = ColorLibrary.DarkRed;
        protected uint EveryNth = 1;

        /// <summary>
        ///  Default constructor used when loading
        /// </summary>
        public SimpleColorFillEffect(ILedCanvas canvas, Dictionary<string, object> settings) : base(canvas, settings)
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
            for (uint i = 0; i < Canvas.PixelCount; i += EveryNth)
                Canvas.DrawPixel(i, Color);
        }

        /// <summary>
        /// Gets a dictionary containing the default parameters
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, object> GetEffectDefaults()
        {
            return new Dictionary<string, object>()
            {
                { "lifetime", 0 },
                { "color", ColorLibrary.DarkRed },
                { "everyNth", 1 }
            };
        }

        /// <summary>
        /// Sets the parameters of this instance of the effect from the provided dictionary.
        /// </summary>
        /// <param name="effectParams">Key-value pair of parameters, if none is passed Defaults are obtained</param>
        public override void SetEffectParameters(Dictionary<string, object> effectParams)
        {
            base.SetEffectParameters(effectParams);
            Lifetime = Convert.ToInt32(effectParams["lifetime"]);
            Color = (LedColor)effectParams["color"];
            EveryNth = Convert.ToUInt32(effectParams["everyNth"]);

        }
    }
}
