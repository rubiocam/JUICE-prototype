using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour
{
    public Texture2D idleCursor; // Default cursor texture
    public Texture2D clickCursor; // Click animation cursor texture
    public float clickDuration = 0.2f; // Duration of the click animation

    private bool isClicking = false;

    [Header("Cursor Hotspot")]
    public Vector2 hotspot = Vector2.zero; // Allows setting the hotspot from the Inspector

    private void Start()
    {
        // Set the initial cursor
        Cursor.SetCursor(idleCursor, hotspot, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isClicking) // Left mouse button
        {
            StartCoroutine(AnimateClickCursor());
        }
    }

    private IEnumerator AnimateClickCursor()
    {
        isClicking = true;

        // Set the click cursor
        Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);

        // Wait for the animation duration
        yield return new WaitForSeconds(clickDuration);

        // Reset to the idle cursor
        Cursor.SetCursor(idleCursor, hotspot, CursorMode.Auto);

        isClicking = false;
    }
}
