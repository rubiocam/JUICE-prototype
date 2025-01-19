using UnityEngine;
using System;

public class Bug : MonoBehaviour
{
    public event Action<GameObject> OnBugDestroyed;

    private void OnDestroy()
    {
        // Notify listeners that the bug is destroyed
        OnBugDestroyed?.Invoke(gameObject);
    }
}
