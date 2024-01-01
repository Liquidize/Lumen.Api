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

        /// <summary>
        /// The Lumen application calls this specific constructor when creating effects, passing in the canvas and settings
        /// Settings could potentially be null, so we need to check for that and set it to the defaults if it is.
        /// Canvas should never be null, so we throw an exception if it is.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="settings"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LedEffect(ILedCanvas canvas, Dictionary<string, object> settings)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            if (settings == null)
            {
                settings = GetEffectDefaults();
            }


            Canvas = canvas;
            SetEffectSettings(settings, true);
        }

        /// <summary>
        /// Display name of this effect
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Unique ID of this effect instance
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Lifetime of the effect in seconds. 0 means infinite.
        /// </summary>
        public virtual double Lifetime { get; protected set; } = 5;

        [JsonIgnore]
        public DateTime StartTime { get; protected set; } = DateTime.UtcNow;

        private bool _endRequested = false;

        /// <summary>
        /// Canvas used for drawing
        /// </summary>
        protected ILedCanvas Canvas { get; set; }

        /// <summary>
        /// Current applied settings as a dictionary
        /// </summary>
        public Dictionary<string, object> Settings { get; protected set; } = new Dictionary<string, object>();

        /// <summary>
        /// Optional logic update function for abstraction, called directly before Render
        /// </summary>
        /// <param name="deltaTime">Total milliseconds between the last frame render and now</param>
        protected virtual void Update(double deltaTime)
        {

        }

        /// <summary>
        /// Gets the end time for this effect
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndTime()
        {
            var seconds = (int)Lifetime;
            var ms = (int)((Lifetime - seconds) * 1000);
            return StartTime.AddSeconds(seconds).AddMilliseconds(ms);
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
        protected abstract void Render(double deltaTime);

        /// <summary>
        /// Request that this effect end, forcing its lifetime to be over regardless of how many seconds were left.
        /// </summary>
        public void RequestEnd() => _endRequested = true;

        /// <summary>
        /// Draws a single frame
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="deltaTime"></param>
        public void DrawFrame(double deltaTime)
        {
            Update(deltaTime);
            Render(deltaTime);
        }

        /// <summary>
        /// Gets a dictionary containing the default parameters for this effect.
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, object> GetEffectDefaults()
        {
            return new Dictionary<string, object>
            {
                { "lifetime", 5 }
            };
        }

        /// <summary>
        /// Gets the current effect settings as a dictionary, used for Lumen API calls.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetEffectSettings()
        {
            return Settings;
        }

        /// <summary>
        /// Merge in the default values for this effect into the given dictionary if their keys aren't present already
        /// </summary>
        /// <param name="effectParams"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, object> MergeSettings(Dictionary<string, object> original, Dictionary<string, object> target)
        {

            if (target == null || target.Count == 0)
                return original;

            foreach (var kvp in original)
            {
                if (!target.ContainsKey(kvp.Key))
                {
                    target.Add(kvp.Key, kvp.Value);
                }
            }

            return target;
        }

        /// <summary>
        /// Sets the effect parameters based off the given dictionary of values. If no dictionary is passed, or it is empty get the defaults.
        /// If a dictionary with values is passed, we merge in the missing values from the defaults.
        /// </summary>
        /// <param name="effectParams"></param>
        public virtual void SetEffectSettings(Dictionary<string, object> effectParams, bool mergeDefaults = false)
        {
            effectParams = mergeDefaults
                ? MergeSettings(GetEffectDefaults(), effectParams)
                : MergeSettings(GetEffectSettings(), effectParams);
            Lifetime = Convert.ToDouble(effectParams["lifetime"]);
            Settings = effectParams;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (!string.IsNullOrEmpty(Id))
            {
                throw new InvalidOperationException("Cannot set the ID of an effect that already has one");
            }

            Id = id;
        }
    }
}
