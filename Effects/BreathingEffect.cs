using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;

namespace Lumen.Api.Effects
{
    public class BreathingEffect : LedEffect
    {
        public LedColor Color { get; set; } = ColorLibrary.Orange;
        public double Frequency { get; set; } = 5;
        public ColorPalette Palette { get; set; } = PaletteLibrary.Rainbow;
        public bool UsePalette { get; set; } = false;


        private double CalculateBrightness(double time)
        {
            // Calculate the phase of the sine wave based on time and frequency
            double phase = 2 * Math.PI * (1 / Frequency) * time;

            // Use a sine function to smoothly oscillate the brightness between 0 and 1
            return (Math.Sin(phase) + 1) / 2;
        }


        protected override void Render(ILedCanvas canvas, double deltaTime)
        {
            // Calculate the current time since the effect started
            double elapsedTime = (DateTime.UtcNow - StartTime).TotalSeconds;

            // Calculate the brightness based on the current time
            double brightness = CalculateBrightness(elapsedTime);

            if (!UsePalette)
            {
                var adjustedColor = Color * brightness;
                canvas.FillSolid(adjustedColor);
            }
            else
            {
                for (uint i = 0; i < canvas.PixelCount; i++)
                {
                    var adjustedColor = Palette[(i / (double)canvas.PixelCount)] * brightness;
                    canvas.DrawPixel(i, adjustedColor);
                }
            }

        }

        protected override Dictionary<string, object> GetEffectDefaults()
        {
            return base.GetEffectDefaults().Concat(new Dictionary<string, object>()
            {
                {"color", ColorLibrary.Orange},
                {"frequency", 5},
                {"palette", PaletteLibrary.Rainbow},
                {"usePalette", false}
            }).ToDictionary();
        }

        public override void SetEffectParameters(Dictionary<string, object> effectParams)
        {
            base.SetEffectParameters(effectParams);
            Color = (LedColor)effectParams["color"];
            Frequency = Convert.ToDouble(effectParams["frequency"]);
            Palette = (ColorPalette)effectParams["palette"];
            UsePalette = (bool)effectParams["usePalette"];
        }
    }
}
