using UnityEngine;

namespace PlaceHolderVFX
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class VFXSpriteRendererExtension : MonoBehaviour
    {
        [HideInInspector]
        public Material previousMat = null;

        private Material vfxMat = null;


        private bool _enableWarpEffect = false;
        public bool EnableWarpEffect
        {
            get { return _enableWarpEffect; }
            set
            {
                _enableWarpEffect = value;
                SetKeyword("_ENABLE_WARP", value);
            }
        }

        private Vector2 _warpTilling = new Vector2(4f, 0f);
        public Vector2 WarpTilling
        {
            get { return _warpTilling; }
            set
            {
                _warpTilling = value;
                SetVector2("_Warp_Tilling", value);
            }
        }

        private Vector2 _warpIntensity = new Vector2(.4f, 0f);
        public Vector2 WarpIntensity
        {
            get { return _warpIntensity; }
            set
            {
                _warpIntensity = value;
                SetVector2("_Warp_Intensity", value);
            }
        }

        private Vector2 _warpSpeed = new Vector2(6f, 0f);
        public Vector2 WarpSpeed
        {
            get { return _warpSpeed; }
            set
            {
                _warpSpeed = value;
                SetVector2("_Warp_Speed", value);
            }
        }

        private void OnValidate()
        {
            if (previousMat != null)
                return;

            var sr = GetComponent<SpriteRenderer>();

            previousMat = sr.sharedMaterial;

            Shader shader = Shader.Find("Maguinho/VFXSprite");
            if (shader != null)
            {
                vfxMat = new Material(shader);
                sr.material = vfxMat;
            }
            else
            {
                Debug.LogError("Shader not found!");
                previousMat = null;
            }
        }

        private void SetKeyword(string reference, bool value)
        {
            if (vfxMat == null)
            {
                Debug.Log("Shader no set!");
                return;
            }

            if (value)
                vfxMat.EnableKeyword(reference);
            else
                vfxMat.DisableKeyword(reference);
        }

        private void SetVector2(string reference, Vector2 value)
        {
            if (vfxMat == null)
            {
                Debug.Log("Shader no set!");
                return;
            }

            vfxMat.SetVector(reference, value);
        }
    }
}
