using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHandler : MonoBehaviour
{
    [Header("Scene Settings")]
    public string openingSceneName = "OpeningScene"; // The name of your opening scene

    void Update()
    {
        // Check if the R key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Load the specified opening scene
            SceneManager.LoadScene(openingSceneName);
        }
    }
}
