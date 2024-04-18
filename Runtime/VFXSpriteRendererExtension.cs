using UnityEngine;

namespace Maguinho.VFX
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class VFXSpriteRendererExtension : MonoBehaviour
    {
        private const string shaderName = "Maguinho/VFXSprite";

        private SpriteRenderer _sr;
        private SpriteRenderer sr => _sr ? _sr : _sr = GetComponent<SpriteRenderer>();

        private Material _VFXMat;
        public Material VFXMat
        {
            get
            {
                if (_VFXMat == null)
                    _VFXMat = new Material(Shader.Find(shaderName));

                return _VFXMat;
            }
        }

        private void OnValidate()
        {
            // Called when a parameter is changed in the inspector or when the scene is changed. (editor only)
            AssignMaterial();
            UpdateAllParametersInMaterial();
        }

        #region Warp
        [SerializeField] private bool _enableWarpEffect = false;
        public bool EnableWarpEffect
        {
            get { return _enableWarpEffect; }
            set
            {
                _enableWarpEffect = value;
                if (_enableWarpEffect)
                    VFXMat.EnableKeyword("_ENABLE_WARP");
                else
                    VFXMat.DisableKeyword("_ENABLE_WARP");
            }
        }

        [SerializeField] private Vector2 _warpTilling = new Vector2(1f, 0f);
        public Vector2 WarpTilling
        {
            get { return _warpTilling; }
            set { _warpTilling = value; VFXMat.SetVector("_Warp_Tilling", _warpTilling); }
        }

        [SerializeField] private Vector2 _warpIntensity = new Vector2(.4f, 0f);
        public Vector2 WarpIntensity
        {
            get { return _warpIntensity; }
            set { _warpIntensity = value; VFXMat.SetVector("_Warp_Intensity", _warpIntensity); }
        }

        [SerializeField] private Vector2 _warpSpeed = new Vector2(6f, 0f);
        public Vector2 WarpSpeed
        {
            get { return _warpSpeed; }
            set { _warpSpeed = value; VFXMat.SetVector("_Warp_Speed", _warpSpeed); }
        }

        [SerializeField] private bool _enableVerticalWarpMask;
        public bool EnableVerticalWarpMask
        {
            get { return _enableVerticalWarpMask; }
            set
            {
                _enableVerticalWarpMask = value;
                if (_enableVerticalWarpMask)
                    VFXMat.EnableKeyword("_ENABLE_VERTICAL_WARP_MASK");
                else
                    VFXMat.DisableKeyword("_ENABLE_VERTICAL_WARP_MASK");
            }
        }

        [SerializeField, Range(0f, 1f)] private float _verticalWarpMask = 0f;
        public float VerticalWarpMask
        {
            get { return _verticalWarpMask; }
            set { _verticalWarpMask = value; VFXMat.SetFloat("_Vertical_Warp_Mask", _verticalWarpMask); }
        }

        [SerializeField] private bool _invertVerticalWarpMask = false;
        public bool InvertVerticalWarpMask
        {
            get { return _invertVerticalWarpMask; }
            set { _invertVerticalWarpMask = value; VFXMat.SetInt("_Invert_Vertical_Warp_Mask", _invertVerticalWarpMask ? 1 : 0); }
        }
        #endregion

        #region CC
        [SerializeField] private bool _enableColorCorrection = false;
        public bool EnableColorCorrection
        {
            get { return _enableColorCorrection; }
            set
            {
                _enableColorCorrection = value;
                if (_enableColorCorrection)
                    VFXMat.EnableKeyword("_ENABLE_COLOR_CORRECTION");
                else
                    VFXMat.DisableKeyword("_ENABLE_COLOR_CORRECTION");
            }
        }

        [SerializeField] private bool _invertColors = false;
        public bool InvertColors
        {
            get { return _invertColors; }
            set
            {
                _invertColors = value;
                if (_invertColors)
                    VFXMat.EnableKeyword("_INVERT_COLORS");
                else
                    VFXMat.DisableKeyword("_INVERT_COLORS");
            }
        }

        [SerializeField, Range(-1f, 1f)] private float _hue = 0f;
        public float Hue
        {
            get { return _hue; }
            set { _hue = value; VFXMat.SetFloat("_Hue", _hue); }
        }

        [SerializeField, Range(-1f, 1f)] private float _saturation = 0f;
        public float Saturation
        {
            get { return _saturation; }
            set { _saturation = value; VFXMat.SetFloat("_Saturation", _saturation); }
        }

        [SerializeField, Range(-1f, 1f)] private float _contrast = 0f;
        public float Contrast
        {
            get { return _contrast; }
            set { _contrast = value; VFXMat.SetFloat("_Contrast", _contrast); }
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
                    VFXMat.EnableKeyword("_ENABLE_SHAKE");
                else
                    VFXMat.DisableKeyword("_ENABLE_SHAKE");
            }
        }

        [SerializeField, Min(0f)] private float _shakeIntensity = 1f;
        public float ShakeIntensity
        {
            get { return _shakeIntensity; }
            set { _shakeIntensity = value; VFXMat.SetFloat("_Shake_Intensity", _shakeIntensity); }
        }

        [SerializeField, Min(0f)] private float _shakeSpeed = 2f;
        public float ShakeSpeed
        {
            get { return _shakeSpeed; }
            set { _shakeSpeed = value; VFXMat.SetFloat("_Shake_Speed", _shakeSpeed); }
        }
        #endregion

        #region Orbital Movement
        [SerializeField] private bool _enableOrbitalMovement = false;
        public bool EnableOrbitalMovement
        {
            get { return _enableOrbitalMovement; }
            set
            {
                _enableOrbitalMovement = value;
                if (_enableOrbitalMovement)
                    VFXMat.EnableKeyword("_ENABLE_ORBITAL_MOVEMENT");
                else
                    VFXMat.DisableKeyword("_ENABLE_ORBITAL_MOVEMENT");
            }
        }

        [SerializeField] private float _orbitalMovementIntensity = 1f;
        public float OrbitalMovementIntensity
        {
            get { return _orbitalMovementIntensity; }
            set { _orbitalMovementIntensity = value; VFXMat.SetFloat("_Orbital_Movement_Intensity", _orbitalMovementIntensity); }
        }

        [SerializeField] private Vector2 _orbitalMovementSpeedScale = Vector2.one;
        public Vector2 OrbitalMovementSpeedScale
        {
            get { return _orbitalMovementSpeedScale; }
            set { _orbitalMovementSpeedScale = value; VFXMat.SetVector("_Orbital_Movement_Speed_Scale", _orbitalMovementSpeedScale); }
        }
        #endregion

        public void AssignMaterial()
        {
            sr.material = VFXMat;
        }

        public void UpdateAllParametersInMaterial()
        {
            // Warp Effect
            if (_enableWarpEffect)
                VFXMat.EnableKeyword("_ENABLE_WARP");
            else
                VFXMat.DisableKeyword("_ENABLE_WARP");

            VFXMat.SetVector("_Warp_Tilling", _warpTilling);
            VFXMat.SetVector("_Warp_Intensity", _warpIntensity);
            VFXMat.SetVector("_Warp_Speed", _warpSpeed);

            if (_enableVerticalWarpMask)
                VFXMat.EnableKeyword("_ENABLE_VERTICAL_WARP_MASK");
            else
                VFXMat.DisableKeyword("_ENABLE_VERTICAL_WARP_MASK");

            VFXMat.SetFloat("_Vertical_Warp_Mask", _verticalWarpMask);
            VFXMat.SetInt("_Invert_Vertical_Warp_Mask", _invertVerticalWarpMask ? 1 : 0);

            // Color
            if (_enableColorCorrection)
                VFXMat.EnableKeyword("_ENABLE_COLOR_CORRECTION");
            else
                VFXMat.DisableKeyword("_ENABLE_COLOR_CORRECTION");

            if (_invertColors)
                VFXMat.EnableKeyword("_INVERT_COLORS");
            else
                VFXMat.DisableKeyword("_INVERT_COLORS");

            VFXMat.SetFloat("_Hue", _hue);
            VFXMat.SetFloat("_Saturation", _saturation);
            VFXMat.SetFloat("_Contrast", _contrast);

            // Shake
            if (_enableShake)
                VFXMat.EnableKeyword("_ENABLE_SHAKE");
            else
                VFXMat.DisableKeyword("_ENABLE_SHAKE");

            VFXMat.SetFloat("_Shake_Intensity", _shakeIntensity);
            VFXMat.SetFloat("_Shake_Speed", _shakeSpeed);

            // Orbital Movement
            if (_enableOrbitalMovement)
                VFXMat.EnableKeyword("_ENABLE_ORBITAL_MOVEMENT");
            else
                VFXMat.DisableKeyword("_ENABLE_ORBITAL_MOVEMENT");

            VFXMat.SetFloat("_Orbital_Movement_Intensity", _orbitalMovementIntensity);
            VFXMat.SetVector("_Orbital_Movement_Speed_Scale", _orbitalMovementSpeedScale);
        }

        private void Start()
        {
            AssignMaterial();
            UpdateAllParametersInMaterial();
        }
    }
}
