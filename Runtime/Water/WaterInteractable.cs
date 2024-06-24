using UnityEngine;

namespace Maguinho.VFX
{
    public sealed class WaterInteractable : MonoBehaviour
    {
        [SerializeField] private WaterManager waterManager;
        [SerializeField] private float ampMultiplier = 1f;

        private Vector2 oldPos;

        private void Update()
        {
            Vector2 pos = transform.position;
            float height = waterManager.GetHeightAt(pos.x);
            if (Mathf.Sign(oldPos.y - height) != Mathf.Sign(pos.y - height))
            {
                waterManager.AddRipple(pos.x, (pos.y - oldPos.y) * ampMultiplier);
            }
            oldPos = pos;
        }
    }
}
