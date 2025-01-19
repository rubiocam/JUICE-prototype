using UnityEngine;
using System.Collections;

public class BugMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Speed of the bug
    public float directionChangeInterval = 1f; // Time between direction changes
    public float edgeMargin = 0.5f; // Margin from the screen edge in world units

    [Header("Particle System")]
    public GameObject deathParticles; // Prefab of the particle system to spawn on death

    private Vector2 moveDirection; // Current movement direction
    private Vector2 screenBounds; // Screen bounds in world space

    private void Start()
    {
        // Get screen bounds in world space
        Camera mainCamera = Camera.main;
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);

        // Start changing direction randomly
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        // Move the bug
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // Keep the bug within bounds
        AdjustDirectionIfClamped();

        // Rotate the bug to face away from the movement direction
        UpdateRotation();
    }

    private void OnMouseDown()
    {
        // Spawn the particle effect at the bug's position
        if (deathParticles != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 1f); // Set Z to 1
            Instantiate(deathParticles, spawnPosition, Quaternion.identity);
        }

        // Destroy the bug
        Destroy(gameObject);
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Choose a random direction
            float angle = Random.Range(0f, 360f);
            moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            // Wait for the next direction change
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void AdjustDirectionIfClamped()
    {
        // Clamp the bug's position within the screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float clampedY = Mathf.Clamp(transform.position.y, -screenBounds.y + edgeMargin, screenBounds.y - edgeMargin);

        if (clampedX != transform.position.x || clampedY != transform.position.y)
        {
            // Adjust movement direction to bounce away from the screen edge
            moveDirection = -moveDirection;
        }

        // Apply the clamped position
        transform.position = new Vector2(clampedX, clampedY);
    }

    private void UpdateRotation()
    {
        if (moveDirection != Vector2.zero) // Ensure there's movement
        {
            // Calculate the angle based on the movement direction
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // Add 180 degrees to make the bug's wings face away from the movement direction
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
        }
    }
}
