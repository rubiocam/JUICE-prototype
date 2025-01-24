using System.Collections;
using UnityEngine;

public class ResizeCursorOnClick : MonoBehaviour
{
    public Texture2D normalCursor; // The default cursor texture
    public Texture2D smallCursor; // The smaller cursor texture
    public float shrinkDuration = 0.2f; // How long the smaller cursor will be shown
    public int scaleFactor = 2; // Factor by which to scale up the cursors

    private Texture2D scaledNormalCursor; // Scaled-up normal cursor
    private Texture2D scaledSmallCursor; // Scaled-up small cursor
    private bool isShrinking = false;

    [Header("Cursor Hotspot")]
    public Vector2 hotspot = Vector2.zero; // Allows setting the hotspot from the Inspector

    private void Start()
    {
        // Scale the cursors
        scaledNormalCursor = ScaleTexture(normalCursor, scaleFactor);
        scaledSmallCursor = ScaleTexture(smallCursor, scaleFactor);

        // Set the initial cursor with the hotspot
        if (scaledNormalCursor != null)
        {
            Cursor.SetCursor(scaledNormalCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogError("Normal cursor texture is missing! Please assign it in the Inspector.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShrinking) // Left mouse button
        {
            StartCoroutine(ShrinkCursor());
        }
    }

    private IEnumerator ShrinkCursor()
    {
        isShrinking = true;

        // Change to the smaller cursor with the hotspot
        if (scaledSmallCursor != null)
        {
            Cursor.SetCursor(scaledSmallCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogError("Small cursor texture is missing! Please assign it in the Inspector.");
        }

        // Wait for the shrink duration
        yield return new WaitForSeconds(shrinkDuration);

        // Reset to the normal cursor with the hotspot
        if (scaledNormalCursor != null)
        {
            Cursor.SetCursor(scaledNormalCursor, hotspot, CursorMode.Auto);
        }

        isShrinking = false;
    }

    private Texture2D ScaleTexture(Texture2D originalTexture, int scale)
    {
        if (originalTexture == null)
        {
            Debug.LogError("Cursor texture is missing! Please assign it in the Inspector.");
            return null;
        }

        int newWidth = originalTexture.width * scale;
        int newHeight = originalTexture.height * scale;

        // Create a new texture with the scaled size
        Texture2D scaledTexture = new Texture2D(newWidth, newHeight);
        Color[] originalPixels = originalTexture.GetPixels();
        Color[] scaledPixels = new Color[newWidth * newHeight];

        // Scale the pixels
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                int originalX = x / scale;
                int originalY = y / scale;
                scaledPixels[y * newWidth + x] = originalPixels[originalY * originalTexture.width + originalX];
            }
        }

        // Apply the scaled pixels to the new texture
        scaledTexture.SetPixels(scaledPixels);
        scaledTexture.Apply();

        return scaledTexture;
    }
}
