using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class BugMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Speed of the bug
    public float directionChangeInterval = 1f; // Time in seconds between direction changes
    public float edgeMarginPixels = 30f; // Margin from the screen edge in pixels

    [Header("Particle System")]
    public GameObject deathParticles; // Prefab of the particle system to spawn on death

    [Header("Audio Settings")]
    public AudioClip wallSlamSound; // Wall slam sound to play when the bug hits a wall
    public AudioClip splatSound; // Splat sound to play when the bug is clicked
    private AudioSource audioSource; // AudioSource component to play the sound
    public float splatSoundDelay = 0.2f;

    [Header("Sound Duration")]
    public float wallSlamSoundDuration = 1f; // Duration limit for the wall slam sound

    private Vector2 screenBounds; // Screen bounds in world space
    private Vector2 moveDirection; // Current movement direction
    private float edgeMargin; // Margin in world units
    private bool isClicked = false; // Flag to stop the movement after clicking


    private void Start()
    {
        // Get the screen bounds in world space
        Camera mainCamera = Camera.main;
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);

        // Convert edge margin from pixels to world units
        edgeMargin = mainCamera.ScreenToWorldPoint(new Vector3(edgeMarginPixels, 0, 0)).x - mainCamera.ScreenToWorldPoint(Vector3.zero).x;

        // Start the direction change coroutine
        StartCoroutine(ChangeDirection());

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject! Please add one.");
        }
    }

    private void Update()
    {
        if (isClicked)
        {
            // If clicked, stop the bug from moving (no translation)
            return;
        }

        // Move the bug
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // Keep the bug within bounds and adjust direction if clamped
        AdjustDirectionIfClamped();

        // Rotate the bug to face away from the movement direction
        UpdateRotation();
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Clicked");

        // Stop the bug's movement
        isClicked = true; // Set the flag to true to stop the movement

        // Trigger screen shake
        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.TriggerShake(0.2f, 0.2f); // Adjust duration and magnitude as needed
        }

        // Play the wall slam sound with a duration limit
        if (audioSource != null && wallSlamSound != null)
        {
            StartCoroutine(PlayWallSlamSoundWithLimit());
        }

        // Spawn the particle effect at the bug's position
        if (deathParticles != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 1f); // Set Z to 1
            Instantiate(deathParticles, spawnPosition, Quaternion.identity);
        }

        // Destroy the bug with a delay for the splat sound
        StartCoroutine(PlaySplatSoundThenDestroy());
    }

    private IEnumerator PlayWallSlamSoundWithLimit()
    {
        // Play the wall slam sound
        audioSource.clip = wallSlamSound;
        audioSource.Play();

        // Wait for the specified duration
        yield return new WaitForSeconds(wallSlamSoundDuration);

        // Stop the wall slam sound if it's still playing
        if (audioSource.isPlaying && audioSource.clip == wallSlamSound)
        {
            audioSource.Stop();
        }

        // Destroy the bug
        Destroy(gameObject);

        // Wait for the delay (e.g., 0.5 seconds)
        yield return new WaitForSeconds(splatSoundDelay);
    }

    private IEnumerator PlaySplatSoundThenDestroy()
    {
        // Play the splat sound
        if (audioSource != null && splatSound != null)
        {
            audioSource.PlayOneShot(splatSound);
        }

        // Wait for the splat sound to finish playing before destroying the bug
        yield return new WaitForSeconds(splatSound.length);

    }


    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Choose a random direction (angle)
            float angle = Random.Range(0f, 360f);
            moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            // Wait for the next direction change
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void AdjustDirectionIfClamped()
    {
        // Check and clamp the bug's position within the screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float clampedY = Mathf.Clamp(transform.position.y, -screenBounds.y + edgeMargin, screenBounds.y - edgeMargin);

        // If the bug is near the edge, redirect its movement direction
        if (clampedX != transform.position.x || clampedY != transform.position.y)
        {
            // If near an edge, force the bug to move away from the edge
            if (transform.position.x >= screenBounds.x - edgeMargin || transform.position.x <= -screenBounds.x + edgeMargin)
            {
                moveDirection.x = -moveDirection.x; // Reverse X direction
            }
            if (transform.position.y >= screenBounds.y - edgeMargin || transform.position.y <= -screenBounds.y + edgeMargin)
            {
                moveDirection.y = -moveDirection.y; // Reverse Y direction
            }
        }

        // Apply the clamped position
        transform.position = new Vector2(clampedX, clampedY);
    }

    private void UpdateRotation()
    {
        // Only update rotation if the bug is moving above a certain threshold to avoid jittering
        if (moveDirection.magnitude > 0.01f)
        {
            // Calculate the angle based on the movement direction
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // Rotate the bug to face away from the movement direction
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
        }
    }
}
