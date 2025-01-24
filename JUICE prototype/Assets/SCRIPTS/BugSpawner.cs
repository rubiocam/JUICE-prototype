using UnityEngine;
using UnityEngine.SceneManagement;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab; // Reference to the bug prefab
    public int maxBugsOnScreen = 5; // Maximum number of bugs allowed on screen
    public int totalBugLimit = 10; // Total number of bugs that can be spawned
    public float spawnInterval = 2f; // Time interval between spawns
    public int killGoal = 10; // The number of kills to reach before triggering the end screen

    private int currentBugCount = 0; // Tracks the number of bugs currently active
    private int totalBugsSpawned = 0; // Tracks the total number of bugs spawned
    private int currentKills = 0; // Tracks the total number of kills
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
        // Check if spawning conditions are met
        if (currentBugCount >= maxBugsOnScreen || totalBugsSpawned >= totalBugLimit) return;

        // Random position within the screen bounds
        float randomX = Random.Range(-screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float randomY = Random.Range(-screenBounds.y + edgeMargin, screenBounds.y - edgeMargin);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Spawn the bug
        GameObject newBug = Instantiate(bugPrefab, spawnPosition, Quaternion.identity);
        currentBugCount++;
        totalBugsSpawned++;

        // Attach a callback to decrease the bug count and handle kills when the bug is destroyed
        Bug bugScript = newBug.GetComponent<Bug>();
        if (bugScript != null)
        {
            bugScript.OnBugDestroyed += HandleBugDestroyed;
        }
        else
        {
            Debug.LogError("Bug prefab is missing the Bug script!");
        }

        Debug.Log("Bug spawned");
    }

    void HandleBugDestroyed(GameObject bug)
    {
        // Decrease the bug count
        currentBugCount--;

        // Increment the kill counter
        currentKills++;

        // Log the kill count
        Debug.Log("Kills: " + currentKills);

        // Check if the player has reached the kill goal
        if (currentKills >= killGoal)
        {
            Debug.Log("Kill goal reached! Transitioning to the ending screen.");
            LoadEndingScreen();
        }
    }

    void LoadEndingScreen()
    {
        // You can load your ending screen here
        SceneManager.LoadScene("EndingScene"); // Uncomment and replace with your actual ending scene name
        Debug.Log("Ending Screen Loaded");
    }
}
