using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;

namespace Lumen.Api.Effects
{
    public class BreathingEffect : LedEffect<BreathingEffectSettings>
    {


        public BreathingEffect(ILedCanvas canvas, BreathingEffectSettings settings) : base(canvas, settings)
        {
        }


        private double CalculateBrightness(double time)
        {
            // Calculate the phase of the sine wave based on time and frequency
            double phase = 2 * Math.PI * (1 / Settings.Frequency) * time;

            // Use a sine function to smoothly oscillate the brightness between 0 and 1
            return (Math.Sin(phase) + 1) / 2;
        }


        protected override void Render(double deltaTime)
        {
            // Calculate the current time since the effect started
            double elapsedTime = (DateTime.UtcNow - StartTime).TotalSeconds;

            // Calculate the brightness based on the current time
            double brightness = CalculateBrightness(elapsedTime);

            if (!Settings.UsePalette)
            {
                var adjustedColor = Settings.Color * brightness;
                Canvas.FillSolid(adjustedColor);
            }
            else
            {
                for (uint i = 0; i < Canvas.PixelCount; i++)
                {
                    var adjustedColor = Settings.Palette[(i / (double)Canvas.PixelCount)] * brightness;
                    Canvas.DrawPixel(i, adjustedColor);
                }
            }

        }
        
        public override BreathingEffectSettings GetEffectDefaults()
        {
            return new BreathingEffectSettings();
        }
    }
}
