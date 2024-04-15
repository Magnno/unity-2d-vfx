using Maguinho.VFX;
using UnityEngine;

public sealed class ClickRippleEffectDemo : MonoBehaviour
{
    [SerializeField] private RippleEffectHandler handler;

    private void Update()
    {
        // Checks if the left mouse button has been clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Gets the mouse position relative to the screen (pixels).
            Vector2 mouseScreenPosition = Input.mousePosition;

            // Converts the screen mouse position to a position in the world.
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            // Adds a ripple effect at the clicked position.
            handler.AddRippleEffect(mouseWorldPosition);
        }
    }
}
