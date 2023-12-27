using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;
using Newtonsoft.Json;

namespace Lumen.Api.Effects
{
    public abstract class LedEffect
    {
        [JsonIgnore]
        public virtual string Name
        {
            get { return GetType().Name; }
        }

        

        protected LedEffect()
        {
            var settings = GetEffectDefaults();
            SetEffectParameters(settings);
        }

        protected LedEffect(Dictionary<string, object> effectParams)
        {
            SetEffectParameters(effectParams);
        }

        public virtual int Lifetime { get; protected set; } = 5;

        [JsonIgnore]
        public DateTime StartTime { get; protected set; } = DateTime.UtcNow;

        private bool _endRequested = false;


        /// <summary>
        /// Optional logic update function for abstraction, called directly before Render
        /// </summary>
        /// <param name="deltaTime">Total milliseconds between the last frame render and now</param>
        public virtual void Update(double deltaTime)
        {

        }

        /// <summary>
        /// Gets the end time for this effect
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndTime()
        {
            return StartTime.AddSeconds(Lifetime);
        }


        /// <summary>
        /// Sets the start time of this effect
        /// </summary>
        /// <param name="time"></param>
        public void SetStartTime(DateTime time)
        {
            StartTime = time;
        }

        /// <summary>
        /// Checks whether this effects lifetime is over.
        /// </summary>
        /// <returns></returns>
        public bool IsLifetimeOver()
        {
            return (Lifetime != 0 && DateTime.UtcNow > GetEndTime()) || _endRequested;
        }

        /// <summary>
        /// Renders a single frame of the effect.
        /// </summary>
        /// <param name="canvas">The canvas to draw on</param>
        /// <param name="deltaTime">Total milliseconds between last frame draw and now, potentially useful for smoothing effects out</param>
        protected abstract void Render(ILedCanvas canvas, double deltaTime);

        /// <summary>
        /// Request that this effect end, forcing its lifetime to be over regardless of how many seconds were left.
        /// </summary>
        public void RequestEnd() => _endRequested = true;

        /// <summary>
        /// Draws a single frame
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="deltaTime"></param>
        public void DrawFrame(ILedCanvas canvas, double deltaTime)
        {
            Render(canvas, deltaTime);
        }

        /// <summary>
        /// Gets a dictionary containing the default parameters for this effect.
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, object> GetEffectDefaults()
        {
            return new Dictionary<string, object>
            {
                { "lifetime", Lifetime }
            };
        }

        protected virtual Dictionary<string, object> MergeDefaults(Dictionary<string, object> effectParams)
        {

            if (effectParams == null || effectParams.Count == 0)
                return GetEffectDefaults();

            var defaults = GetEffectDefaults();
            foreach (var kvp in defaults)
            {
                if (!effectParams.ContainsKey(kvp.Key))
                {
                    effectParams.Add(kvp.Key, kvp.Value);
                }
            }

            return effectParams;
        }

        /// <summary>
        /// Sets the effect parameters based off the given dictionary of values. If no dictionary is passed or it is empty get the defaults.
        /// If a dictionary with values is passed, we merge in the missing values from the defaults.
        /// </summary>
        /// <param name="effectParams"></param>
        public virtual void SetEffectParameters(Dictionary<string, object> effectParams)
        {
            effectParams = MergeDefaults(effectParams);
            Lifetime = Convert.ToInt32(effectParams["lifetime"]);
        }
    }
}
