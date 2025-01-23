using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTex;
    public Vector2 hotspot = Vector2.zero; // Allows setting the hotspot from the Inspector

    private void Awake()
    {
        if (cursorTex != null)
        {
            Cursor.SetCursor(cursorTex, hotspot, CursorMode.ForceSoftware);
        }
        else
        {
            Debug.LogWarning("Cursor texture not assigned.");
        }
    }
}
