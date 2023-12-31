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

        public BreathingEffect(ILedCanvas canvas, Dictionary<string, object> settings) : base(canvas, settings)
        {
        }


        private double CalculateBrightness(double time)
        {
            // Calculate the phase of the sine wave based on time and frequency
            double phase = 2 * Math.PI * (1 / Frequency) * time;

            // Use a sine function to smoothly oscillate the brightness between 0 and 1
            return (Math.Sin(phase) + 1) / 2;
        }


        protected override void Render(double deltaTime)
        {
            // Calculate the current time since the effect started
            double elapsedTime = (DateTime.UtcNow - StartTime).TotalSeconds;

            // Calculate the brightness based on the current time
            double brightness = CalculateBrightness(elapsedTime);

            if (!UsePalette)
            {
                var adjustedColor = Color * brightness;
                Canvas.FillSolid(adjustedColor);
            }
            else
            {
                for (uint i = 0; i < Canvas.PixelCount; i++)
                {
                    var adjustedColor = Palette[(i / (double)Canvas.PixelCount)] * brightness;
                    Canvas.DrawPixel(i, adjustedColor);
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

        public override void SetEffectSettings(Dictionary<string, object> effectParams, bool mergeDefaults = true)
        {
            base.SetEffectSettings(effectParams, mergeDefaults);
            Color = (LedColor)effectParams["color"];
            Frequency = Convert.ToDouble(effectParams["frequency"]);
            Palette = (ColorPalette)effectParams["palette"];
            UsePalette = (bool)effectParams["usePalette"];
        }
    }
}
