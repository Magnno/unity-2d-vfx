using UnityEngine;

namespace PlaceHolderVFX
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
            UpdateParametersInMaterial();
        }

        [SerializeField] private bool _enableWarpEffect = false;
        public bool EnableWarpEffect
        {
            get { return _enableWarpEffect; }
            set { _enableWarpEffect = value; UpdateParametersInMaterial(); }
        }

        [SerializeField] private Vector2 _warpTilling = new Vector2(4f, 0f);
        public Vector2 WarpTilling
        {
            get { return _warpTilling; }
            set { _warpTilling = value; UpdateParametersInMaterial(); }
        }

        [SerializeField] private Vector2 _warpIntensity = new Vector2(.4f, 0f);
        public Vector2 WarpIntensity
        {
            get { return _warpIntensity; }
            set { _warpIntensity = value; UpdateParametersInMaterial(); }
        }

        [SerializeField] private Vector2 _warpSpeed = new Vector2(6f, 0f);
        public Vector2 WarpSpeed
        {
            get { return _warpSpeed; }
            set { _warpSpeed = value; UpdateParametersInMaterial(); }
        }


        public void AssignMaterial()
        {
            sr.material = VFXMat;
        }

        public void UpdateParametersInMaterial()
        {
            // Warp Effect
            if (_enableWarpEffect)
                VFXMat.EnableKeyword("_ENABLE_WARP");
            else
                VFXMat.DisableKeyword("_ENABLE_WARP");

            VFXMat.SetVector("_Warp_Tilling", _warpTilling);
            VFXMat.SetVector("_Warp_Intensity", _warpIntensity);
            VFXMat.SetVector("_Warp_Speed", _warpSpeed);
        }

        private void Start()
        {
            AssignMaterial();
            UpdateParametersInMaterial();
        }
    }
}
