using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab; // Reference to the bug prefab
    public int maxBugs = 10; // Maximum number of bugs allowed
    public float spawnInterval = 2f; // Time interval between spawns

    private int currentBugCount = 0; // Tracks the number of bugs currently active
    private Vector2 screenBounds; // Screen bounds in world space
    private float edgeMargin = 0.5f; // Margin from the screen edge in world units

    void Start()
    {
        // Get screen bounds
        Camera mainCamera = Camera.main;
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);

        // Start spawning bugs
        InvokeRepeating(nameof(SpawnBug), spawnInterval, spawnInterval);
    }

    void SpawnBug()
    {
        if (currentBugCount >= maxBugs) return; // Don't spawn if max bugs are already active

        // Random position within the screen bounds
        float randomX = Random.Range(-screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float randomY = Random.Range(-screenBounds.y + edgeMargin, screenBounds.y - edgeMargin);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Spawn the bug
        GameObject newBug = Instantiate(bugPrefab, spawnPosition, Quaternion.identity);
        currentBugCount++;

        // Attach a callback to decrease the bug count when the bug is destroyed
        newBug.GetComponent<Bug>().OnBugDestroyed += HandleBugDestroyed;
        Debug.Log("Bug spawned");

    }

    void HandleBugDestroyed(GameObject bug)
    {
        currentBugCount--;
    }
}
