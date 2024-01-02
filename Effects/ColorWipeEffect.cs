using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;

namespace Lumen.Api.Effects
{

    public class ColorWipeEffect : LedEffect<ColorWipeEffectSettings>
    {
        public uint WipeSize { get; set; } = 0;

        private int _lastPaletteIndex = -1;

        private int _currentPaletteIndex = 0;

        public ColorWipeEffect(ILedCanvas canvas, ColorWipeEffectSettings settings) : base(canvas, settings)
        {
        }


        protected override void Update(double deltaTime)
        {
            if (WipeSize >= Canvas.PixelCount)
            {
                if (Settings.UsePalette)
                {
                    _lastPaletteIndex = _currentPaletteIndex; // Save the last color in the palette
                    // Move to the next color in the palette
                    _currentPaletteIndex++;
                    if (_currentPaletteIndex >= Settings.Palette.OriginalSize)
                    {
                        _currentPaletteIndex = 0; // Start over if reached the end of the palette
                    }
                }
                WipeSize = 0;
            }
            else
            {
                WipeSize++;
            }
        }

        protected override void Render(double deltaTime)
        {

            if (Settings.UsePalette &&
                Settings.FillLastColor && _lastPaletteIndex != -1) // If using palette and filling last color, fill with the last color in the palette
            {
                Canvas.FillSolid(Settings.Palette[_lastPaletteIndex]);
            }
            else // If not using palette or not filling last color, fill with black
            {
                Canvas.FillSolid(ColorLibrary.Black);
            }

            if (!Settings.UsePalette) // If not using palette, fill with a single color
            {
                for (var i = 0; i < WipeSize; i++)
                {
                    Canvas.DrawPixel((uint)i, Settings.Color);
                }
            }
            else // If using palette, fill with colors from the palette
            {
                for (var i = 0; i < WipeSize; i++)
                {
                    Canvas.DrawPixel((uint)i, Settings.Palette[_currentPaletteIndex]);
                }
            }
        }
    }
}
