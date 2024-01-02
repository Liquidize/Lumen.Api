using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumen.Api.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lumen.Api.Effects
{
    /// <summary>
    /// Abstract base class of all effects without generic settings type. Used internally within Lumen as an object reference to all effects,
    /// defines all critical methods and properties that effects must implement.
    /// </summary>
    public abstract class LedEffect
    {
        /// <summary>
        /// Display name of this effect.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Checks whether the lifetime of the effect is over.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsLifetimeOver();

        /// <summary>
        /// Draws a single frame of the effect.
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void DrawFrame(double deltaTime);

        /// <summary>
        /// Gets the unique id of this effect.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the start time of this effect
        /// </summary>
        [JsonIgnore] public DateTime StartTime { get; protected set; }

        /// <summary>
        /// Sets the start time of the effect.
        /// </summary>
        /// <param name="time"></param>
        public void SetStartTime(DateTime time)
        {
            StartTime = time;
        }

        /// <summary>
        /// Gets the end time of the effect.
        /// </summary>
        /// <returns></returns>
        public abstract DateTime GetEndTime();

        /// <summary>
        /// Request that this effect end, forcing its lifetime to be over regardless of how many seconds were left.
        /// </summary>
        public abstract void RequestEnd();

        /// <summary>
        /// Gets the current effect settings
        /// </summary>
        /// <returns></returns>
        public abstract EffectSettings GetEffectSettings();
        /// <summary>
        /// Gets the default effect settings.
        /// </summary>
        /// <returns></returns>
        public abstract EffectSettings GetEffectDefaults();
        /// <summary>
        /// Sets the current effect settings via a serialized JObject of a settings object.
        /// </summary>
        /// <param name="settingsObj"></param>
        public abstract void SetEffectSettings(JObject settingsObj);

        /// <summary>
        /// Sets the unique ID of this effect. Throws an exception if the ID is already set.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// The base class for all effects. Utilizies a custom settings class for settings per effect type.
    /// </summary>
    /// <typeparam name="TSettings">Settings class used for this effect</typeparam>
    public abstract class LedEffect<TSettings> : LedEffect where TSettings : EffectSettings
    {

        /// <summary>
        /// The Lumen application calls this specific constructor when creating effects, passing in the canvas and settings
        /// Settings could potentially be null, so we need to check for that and set it to the defaults if it is.
        /// Canvas should never be null, so we throw an exception if it is.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="settings"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LedEffect(ILedCanvas canvas, TSettings settings)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            if (settings == null)
            {
                settings = GetEffectDefaults();
            }

            Settings = settings;
            Canvas = canvas;
        }

        /// <summary>
        /// Display name of this effect
        /// </summary>
        public override string Name
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Canvas used for drawing
        /// </summary>
        protected ILedCanvas Canvas { get; }

        /// <summary>
        /// Current applied settings as a dictionary
        /// </summary>
        protected TSettings Settings { get; set; }


        /// <summary>
        /// A boolean to dictate whether the effect has been requested to end immediately
        /// </summary>
        private bool _endRequested = false;


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
        public override DateTime GetEndTime()
        {
            var seconds = (int)Settings.Lifetime;
            var ms = (int)((Settings.Lifetime - seconds) * 1000);
            return StartTime.AddSeconds(seconds).AddMilliseconds(ms);
        }


        /// <summary>
        /// Checks whether this effects lifetime is over.
        /// </summary>
        /// <returns></returns>
        public override bool IsLifetimeOver()
        {
            return (Settings.Lifetime != 0 && DateTime.UtcNow > GetEndTime()) || _endRequested;
        }

        /// <summary>
        /// Request the effect to end immediately, regardless of how many seconds were left.
        /// </summary>
        public override void RequestEnd()
        {
            _endRequested = true;
        }

        /// <summary>
        /// Renders a single frame of the effect.
        /// </summary>
        /// <param name="deltaTime">Total milliseconds between last frame draw and now, potentially useful for smoothing effects out</param>
        protected abstract void Render(double deltaTime);

        /// <summary>
        /// Draws a single frame
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void DrawFrame(double deltaTime)
        {
            Update(deltaTime);
            Render(deltaTime);
        }

        /// <summary>
        /// Gets an instance of the default settings for this effect.
        /// </summary>
        /// <returns></returns>
        public override TSettings GetEffectDefaults()
        {
            return Activator.CreateInstance<TSettings>();
        }

        /// <summary>
        /// Gets the current effect settings.
        /// </summary>
        /// <returns></returns>
        public override EffectSettings GetEffectSettings()
        {
            return Settings;
        }

        /// <summary>
        /// Sets the active effect settings via a serialized JObject of a settings object.
        /// Deserializes the instance if possible, otherwise throws an exception.
        /// </summary>
        /// <param name="settingsObj"></param>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="Exception"></exception>
        public override void SetEffectSettings(JObject settingsObj)
        {
            try
            {
                var settings = settingsObj.ToObject<TSettings>();
                if (settings != null)
                {
                    Settings = settings;
                    Console.WriteLine("Set settings");
                }
            }
            catch (JsonException jsonEx)
            {
                throw new JsonException($"Error deserializing settings JSON: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while setting effect settings: {ex.Message}");
            }
        }
    }
}
