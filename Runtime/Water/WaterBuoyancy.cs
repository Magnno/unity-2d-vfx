using UnityEngine;

namespace Maguinho.VFX
{
    public sealed class WaterBuoyancy : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("The water to simulate buoyance.")]
        public WaterManager waterManager;
        [Tooltip("The rigidbody used. Rigidbody's 'Gravity Scale' will be set to zero, because this script is already applying gravity.")]
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Header("Buoyancy Values")]
        [SerializeField] private float gravityScale = 1f;
        [Tooltip("The transition between not submerged and fully submerged.")]
        [SerializeField, Min(.001f)] private float submergedDepth = .5f;
        [Tooltip("Buoyancy force to apply when submerged.")]
        [SerializeField, Min(0f)] private float buoyancyForce = 10f;
        [Tooltip("Drag when submerged. Prevents the object from bouncing.")]
        [SerializeField, Min(0f)] private float dragForce = 2f;
        [Tooltip("Angular drag when submerged.")]
        [SerializeField, Min(0f)] private float angularDragForce = 0f;

        [Space(10f)]
        [SerializeField] private Transform[] pointsToApplyForce;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rigidbody2D)
            {
                _rigidbody2D.gravityScale = 0f;
            }
        }
#endif
        private void FixedUpdate()
        {
            foreach (var point in pointsToApplyForce)
            {
                Vector2 pos = point.position;
                _rigidbody2D.AddForceAtPosition(Physics2D.gravity.y * gravityScale / pointsToApplyForce.Length * Vector2.up, pos);
                float height = waterManager.GetHeightAt(pos.x);
                if (pos.y < height)
                {
                    float mult = Mathf.Clamp01((height - pos.y) / submergedDepth);
                    _rigidbody2D.AddForceAtPosition(buoyancyForce * mult * Time.fixedDeltaTime * 10f * -Physics2D.gravity.y * gravityScale * Vector2.up, pos); // force
                    _rigidbody2D.AddForceAtPosition(-dragForce * mult * Time.fixedDeltaTime * 100f * _rigidbody2D.GetPointVelocity(pos), pos); // drag
                    _rigidbody2D.AddTorque(-angularDragForce * mult * _rigidbody2D.angularVelocity * Time.fixedDeltaTime * 100f); // angular drag
                }
            }
        }
    }
}
