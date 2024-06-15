using UnityEngine;

namespace Maguinho.VFX
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteVFX : MonoBehaviour
    {
        private const string SHADER_NAME = "Maguinho/SpriteVFX";

        private SpriteRenderer Sr => _sr ? _sr : _sr = GetComponent<SpriteRenderer>();
        private SpriteRenderer _sr;

        public Material Mat => _mat ? _mat : _mat = new Material(Shader.Find(SHADER_NAME));
        private Material _mat;

        #region Warp
        [SerializeField] private bool _enableWarp = false;
        public bool EnableWarp
        {
            get { return _enableWarp; }
            set
            {
                _enableWarp = value;
                if (_enableWarp)
                    Mat.EnableKeyword("_ENABLE_WARP");
                else
                    Mat.DisableKeyword("_ENABLE_WARP");
            }
        }

        [SerializeField] private Vector2 _warpIntensity = new(0f, 2f);
        public Vector2 WarpIntensity
        {
            get { return _warpIntensity; }
            set { _warpIntensity = value; Mat.SetVector("_Warp_Intensity", _warpIntensity); }
        }

        [SerializeField, Min(0f)] private Vector2 _warpScale = new(1f, 1f);
        public Vector2 WarpScale
        {
            get { return _warpScale; }
            set { _warpScale = value; Mat.SetVector("_Warp_Scale", _warpScale); }
        }

        [SerializeField] private Vector2 _warpSpeed = new(2f, 2f);
        public Vector2 WarpSpeed
        {
            get { return _warpSpeed; }
            set { _warpSpeed = value; Mat.SetVector("_Warp_Speed", _warpSpeed); }
        }

        [SerializeField] private bool _enableVerticalWarpMask = false;
        public bool EnableVerticalWarpMask
        {
            get { return _enableVerticalWarpMask; }
            set
            {
                _enableVerticalWarpMask = value;
                Mat.SetFloat("_Enable_Warp_Mask", _enableVerticalWarpMask ? 1f : 0f);
            }
        }

        [SerializeField, Range(0f, 1f)] private float _verticalWarpMask = 0f;
        public float VerticalWarpMask
        {
            get { return _verticalWarpMask; }
            set { _verticalWarpMask = value; Mat.SetFloat("_Vertical_Warp_Mask", _verticalWarpMask); }
        }

        [SerializeField] private bool _invertVerticalWarpMask = false;
        public bool InvertVerticalWarpMask
        {
            get { return _invertVerticalWarpMask; }
            set { _invertVerticalWarpMask = value; Mat.SetInt("_Invert_Vertical_Warp_Mask", _invertVerticalWarpMask ? 1 : 0); }
        }
        #endregion

        #region Color Adjustments
        [SerializeField] private bool _enableColorAdjustments = false;
        public bool EnableColorAdjustments
        {
            get { return _enableColorAdjustments; }
            set
            {
                _enableColorAdjustments = value;
                if (_enableColorAdjustments)
                    Mat.EnableKeyword("_ENABLE_COLOR_ADJUSTMENTS");
                else
                    Mat.DisableKeyword("_ENABLE_COLOR_ADJUSTMENTS");
            }
        }

        [SerializeField] private bool _invertColors = false;
        public bool InvertColors
        {
            get { return _invertColors; }
            set
            {
                _invertColors = value;
                Mat.SetFloat("_Invert_Colors", _invertColors ? 1f : 0f);
            }
        }

        [SerializeField, Range(-1f, 1f)] private float _hue = 0f;
        public float Hue
        {
            get { return _hue; }
            set { _hue = value; Mat.SetFloat("_Hue", _hue); }
        }

        [SerializeField, Range(-1f, 1f)] private float _saturation = 0f;
        public float Saturation
        {
            get { return _saturation; }
            set { _saturation = value; Mat.SetFloat("_Saturation", _saturation); }
        }

        [SerializeField, Range(-1f, 1f)] private float _contrast = 0f;
        public float Contrast
        {
            get { return _contrast; }
            set { _contrast = value; Mat.SetFloat("_Contrast", _contrast); }
        }
        #endregion

        #region Shake
        [SerializeField] private bool _enableShake = false;
        public bool EnableShake
        {
            get { return _enableShake; }
            set
            {
                _enableShake = value;
                if (_enableShake)
                    Mat.EnableKeyword("_ENABLE_SHAKE");
                else
                    Mat.DisableKeyword("_ENABLE_SHAKE");
            }
        }

        [SerializeField, Min(0f)] private float _shakeIntensity = 1f;
        public float ShakeIntensity
        {
            get { return _shakeIntensity; }
            set { _shakeIntensity = value; Mat.SetFloat("_Shake_Intensity", _shakeIntensity); }
        }

        [SerializeField, Min(0f)] private float _shakeSpeed = 4f;
        public float ShakeSpeed
        {
            get { return _shakeSpeed; }
            set { _shakeSpeed = value; Mat.SetFloat("_Shake_Speed", _shakeSpeed); }
        }
        #endregion

        #region Sine Movement
        [SerializeField] private bool _enableSineMovement = false;
        public bool EnableSineMovement
        {
            get { return _enableSineMovement; }
            set
            {
                _enableSineMovement = value;
                if (_enableSineMovement)
                    Mat.EnableKeyword("_ENABLE_SINE_MOVEMENT");
                else
                    Mat.DisableKeyword("_ENABLE_SINE_MOVEMENT");
            }
        }

        [SerializeField] private Vector2 _sineMovementIntensity = new(2f, 2f);
        public Vector2 SineMovementIntensity
        {
            get { return _sineMovementIntensity; }
            set { _sineMovementIntensity = value; Mat.SetVector("_Sine_Movement_Intensity", _sineMovementIntensity); }
        }

        [SerializeField] private Vector2 _sineMovementSpeed = Vector2.one;
        public Vector2 SineMovementSpeed
        {
            get { return _sineMovementSpeed; }
            set { _sineMovementSpeed = value; Mat.SetVector("_Sine_Movement_Speed", _sineMovementSpeed); }
        }

        [SerializeField] private bool _sineMovementSineAndCosine = true;
        public bool SineMovementUseSineAndCosine
        {
            get { return _sineMovementSineAndCosine; }
            set { _sineMovementSineAndCosine = value; Mat.SetFloat("_Sine_Movement_Sine_and_Cosine", _sineMovementSineAndCosine ? 1f : 0f); }
        }
        #endregion

        private void UpdateMaterial()
        {
            Sr.material = Mat;

            // Warp Effect
            if (_enableWarp)
                Mat.EnableKeyword("_ENABLE_WARP");
            else
                Mat.DisableKeyword("_ENABLE_WARP");

            Mat.SetVector("_Warp_Intensity", _warpIntensity);
            Mat.SetVector("_Warp_Scale", _warpScale);
            Mat.SetVector("_Warp_Speed", _warpSpeed);

            Mat.SetFloat("_Enable_Warp_Mask", _enableVerticalWarpMask ? 1f : 0f);
            Mat.SetFloat("_Vertical_Warp_Mask", _verticalWarpMask);
            Mat.SetFloat("_Invert_Vertical_Warp_Mask", _invertVerticalWarpMask ? 1f : 0f);

            // Color
            if (_enableColorAdjustments)
                Mat.EnableKeyword("_ENABLE_COLOR_ADJUSTMENTS");
            else
                Mat.DisableKeyword("_ENABLE_COLOR_ADJUSTMENTS");

            Mat.SetFloat("_Invert_Colors", _invertColors ? 1f : 0f);
            Mat.SetFloat("_Hue", _hue);
            Mat.SetFloat("_Saturation", _saturation);
            Mat.SetFloat("_Contrast", _contrast);

            // Shake
            if (_enableShake)
                Mat.EnableKeyword("_ENABLE_SHAKE");
            else
                Mat.DisableKeyword("_ENABLE_SHAKE");

            Mat.SetFloat("_Shake_Intensity", _shakeIntensity);
            Mat.SetFloat("_Shake_Speed", _shakeSpeed);

            // Sine Movement
            if (_enableSineMovement)
                Mat.EnableKeyword("_ENABLE_SINE_MOVEMENT");
            else
                Mat.DisableKeyword("_ENABLE_SINE_MOVEMENT");

            Mat.SetVector("_Sine_Movement_Intensity", _sineMovementIntensity);
            Mat.SetVector("_Sine_Movement_Speed", _sineMovementSpeed);
            Mat.SetFloat("_Sine_Movement_Sine_and_Cosine", _sineMovementSineAndCosine ? 1f : 0f);
        }

        private void Start()
        {
            UpdateMaterial();
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateMaterial();
        }
#endif
    }
}
