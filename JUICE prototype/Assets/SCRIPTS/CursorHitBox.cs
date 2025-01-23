using UnityEngine;

public class CursorHitbox : MonoBehaviour
{
    [Header("Cursor Settings")]
    public Vector2 offset = Vector2.zero; // Optional offset for the hitbox position

    private void Update()
    {
        // Get the mouse position in screen space and convert to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Apply the offset and ensure the hitbox stays in the 2D plane
        transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, 0f);
    }
}
