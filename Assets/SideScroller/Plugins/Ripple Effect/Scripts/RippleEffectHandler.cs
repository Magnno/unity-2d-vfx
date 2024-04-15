using UnityEngine;

namespace Maguinho.VFX
{
    public sealed class RippleEffectHandler : MonoBehaviour
    {
        // Names for each property in the material.
        public const string TIME = "_t";
        public const string ORIGIN = "_origin";
        public const string AMPLITUDE = "_amplitude";
        public const string VELOCITY = "_velocity";
        public const string DURATION = "_duration";
        public const string DAMPING = "_damping";
        public const string FREQUENCY = "_frequency";
        public const string SMOOTH = "_smoothEdge";

        [Tooltip("The ripple effect material used in the render feature.")]
        [SerializeField] private Material _material;

        /// <summary>
        /// Default properties that are applied when no specific properties are provided.
        /// </summary>
        [SerializeField] private RipplePropertiesSO _defaultProperties;

        private RippleEffect[] _effects;

        /// <summary>
        /// Class for storing information about each ripple effect.
        /// </summary>
        public class RippleEffect
        {
            public readonly int id;
            public bool enabled;
            public float time;
            public Vector2 origin; // World space
            public RipplePropertiesSO properties;

            public RippleEffect(int id)
            {
                this.id = id;
            }
        }

        private void OnEnable()
        {
            // Initializes 5 instances of classes to store information about the ripple effects.
            // 5 is the limit of allowed effects in the shader.

            _effects = new RippleEffect[5];
            for (int i = 0; i < _effects.Length; i++)
            {
                _effects[i] = new RippleEffect(i);
            }
        }

        private void OnDisable()
        {
            // Disable the effects.
            for (int i = 0; i < _effects.Length; i++)
                ToggleEffectActivation(i, false);
        }

        private void Update()
        {
            // Updates the time value of the active effects.
            // Disables the effect if the time value exceeds the duration.
            foreach (var effect in _effects)
            {
                if (!effect.enabled)
                    continue;

                effect.time += Time.deltaTime;
                UpdateEffectProperty(effect.id, TIME, effect.time);

                if (effect.time >= effect.properties.duration)
                {
                    effect.enabled = false;
                    ToggleEffectActivation(effect.id, false);
                }
            }
        }

        /// <summary>
        /// Adds a ripple effect to the scene.
        /// </summary>
        /// <param name="origin">The origin position of the effect in world space.</param>
        /// <param name="properties">Optional properties for the ripple effect; defaults to the standard properties if not specified.</param>
        public void AddRippleEffect(Vector2 origin, RipplePropertiesSO properties = null)
        {
            // Searches for an effect that is not yet being used.
            // If all are being used, the effect will not be applied.
            foreach (var effect in _effects)
            {
                if (effect.enabled)
                    continue;

                effect.enabled = true;
                effect.time = 0f;
                effect.origin = origin;
                effect.properties = properties == null ? _defaultProperties : properties;

                UpdateAllEffectProperties(effect.id, 0f, origin, effect.properties);
                ToggleEffectActivation(effect.id, true);

                return;
            }

            Debug.LogWarning("Limit of 5 effects reached.");
        }

        /// <summary>
        /// Toggles an effect's activation in the material.
        /// </summary>
        /// <param name="id">ID of the effect.</param>
        /// <param name="active">Enable (true) or disable (false).</param>
        public void ToggleEffectActivation(int id, bool active)
        {
            if (active)
                _material.EnableKeyword($"_ENABLED{id}");
            else
                _material.DisableKeyword($"_ENABLED{id}");
        }

        /// <summary>
        /// Updates a specific property of an effect in the material.
        /// </summary>
        /// <param name="id">ID of the effect.</param>
        /// <param name="property">Property to update.</param>
        /// <param name="value">The new value.</param>
        public void UpdateEffectProperty(int id, string property, float value)
        {
            _material.SetFloat(property + id, value);
        }

        /// <summary>
        /// Updates a specific property of an effect in the material.
        /// </summary>
        /// <param name="id">ID of the effect.</param>
        /// <param name="property">Property to update.</param>
        /// <param name="value">The new value.</param>
        public void UpdateEffectProperty(int id, string property, Vector2 value)
        {
            _material.SetVector(property + id, value);
        }

        /// <summary>
        /// Updates all properties of an effect in the material.
        /// </summary>
        /// <param name="id">ID of the effect.</param>
        /// <param name="time">The new time value.</param>
        /// <param name="origin">The new origin value.</param>
        /// <param name="properties">The new properties.</param>
        public void UpdateAllEffectProperties(int id, float time, Vector2 origin, RipplePropertiesSO properties)
        {
            UpdateEffectProperty(id, TIME, time);
            UpdateEffectProperty(id, ORIGIN, origin);
            UpdateEffectProperty(id, AMPLITUDE, properties.amplitude);
            UpdateEffectProperty(id, VELOCITY, properties.velocity);
            UpdateEffectProperty(id, DURATION, properties.duration);
            UpdateEffectProperty(id, DAMPING, properties.damping);
            UpdateEffectProperty(id, FREQUENCY, properties.frequency);
            UpdateEffectProperty(id, SMOOTH, properties.smoothEdge);
        }
    }
}
