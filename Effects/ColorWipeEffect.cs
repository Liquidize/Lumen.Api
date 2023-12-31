using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;

namespace Lumen.Api.Effects
{
    
    public class ColorWipeEffect : LedEffect
    {
        public LedColor Color = ColorLibrary.Orange;
        public uint WipeSize = 0;
        public ColorPalette Palette { get; set; } = PaletteLibrary.Rainbow; // Add a ColorPalette property

        public bool UsePalette { get; set; } = true; // Add a boolean property for using the palette for color wipe

        private int _currentPaletteIndex = 0;

        public bool FillLastColor { get; set; } = true;

        private int _lastPaletteIndex = -1;

        public ColorWipeEffect(ILedCanvas canvas, Dictionary<string, object> settings) : base(canvas, settings)
        {
        }

        protected override Dictionary<string, object> GetEffectDefaults()
        {
            return base.GetEffectDefaults().Concat(new Dictionary<string, object>()
            {
                { "color", ColorLibrary.Orange },
                { "palette", PaletteLibrary.Rainbow },
                { "usePalette", true },
                { "fillLastColor", true }
            }).ToDictionary();
        }

        public override void SetEffectParameters(Dictionary<string, object> effectParams)
        {
            base.SetEffectParameters(effectParams);
            Color = (LedColor)effectParams["color"];
            Palette = (ColorPalette)effectParams["palette"];
            UsePalette = (bool)effectParams["usePalette"];
            FillLastColor = (bool)effectParams["fillLastColor"];
        }

        protected override void Update(double deltaTime)
        {
            if (WipeSize >= Canvas.PixelCount)
            {
                if (UsePalette)
                {
                    _lastPaletteIndex = _currentPaletteIndex; // Save the last color in the palette
                    // Move to the next color in the palette
                    _currentPaletteIndex++;
                    if (_currentPaletteIndex >= Palette.OriginalSize)
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

            if (UsePalette &&
                FillLastColor && _lastPaletteIndex != -1) // If using palette and filling last color, fill with the last color in the palette
            {
                Canvas.FillSolid(Palette[_lastPaletteIndex]);
            }
            else // If not using palette or not filling last color, fill with black
            {
                Canvas.FillSolid(ColorLibrary.Black);
            }

            if (!UsePalette) // If not using palette, fill with a single color
            {
                for (var i = 0; i < WipeSize; i++)
                {
                    Canvas.DrawPixel((uint)i, Color);
                }
            }
            else // If using palette, fill with colors from the palette
            {
                int startIndex = _currentPaletteIndex;
                for (var i = 0; i < WipeSize; i++)
                {
                    Canvas.DrawPixel((uint)i, Palette[startIndex]);
                }
            }
        }
    }
}
