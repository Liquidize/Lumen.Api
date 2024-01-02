using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Lumen.Api.Effects
{
    public abstract class EffectSettings
    {
        public double Lifetime { get; set; } = 0;

    }
}
