using UnityEngine;

namespace Maguinho.VFX
{
    [CreateAssetMenu(fileName = "RippleSettings", menuName = "Maguinho/VFX/New Ripple Properties")]
    public class RipplePropertiesSO : ScriptableObject
    {
        public float amplitude = 0.5f;
        public float velocity = 0.7f;
        public float duration = 1.25f;
        public float damping = 3.0f;
        public float frequency = 2.0f;
        public float smoothEdge = 0.25f;
    }
}
