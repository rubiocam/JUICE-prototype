using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BugLogic : MonoBehaviour
{
    [Header("Particle System")]
    public GameObject deathParticles; // Prefab of the particle system to spawn on death

    [Header("Audio Settings")]
    public AudioClip wallSlamSound; // Wall slam sound to play when the bug hits a wall
    public AudioClip splatSound; // Splat sound to play when the bug is clicked
    private AudioSource audioSource; // AudioSource component to play the sound
    public float splatSoundDelay = 0.2f;
    
    [Header("Sound Duration")]
    public float wallSlamSoundDuration = 0.4f; // Duration limit for the wall slam sound

    [Header("Jitter Settings")]
    public float jitterRange = 0.1f; // Range of jitter movement
    public float jitterSpeed = 10f; // Speed of jittering

    private Vector3 initialPosition; // Initial position of the fly

    void Start()
    {
        // Store the initial position
        initialPosition = transform.position;

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject! Please add one.");
        }
    }

    void Update()
    {
        // Apply jittering around the initial position
        float offsetX = Mathf.Sin(Time.time * jitterSpeed) * jitterRange;
        float offsetY = Mathf.Cos(Time.time * jitterSpeed) * jitterRange;

        transform.position = initialPosition + new Vector3(offsetX, offsetY, 0);
    }

    private void OnMouseDown()
    {
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

        LoadStartingScreen();
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
        yield return new WaitForSeconds(splatSound.length); yield return 
            new WaitForSeconds(0.2f);
    }

    private void LoadStartingScreen()
    {
        // Load the "Ending" scene (replace with your actual scene name)
        SceneManager.LoadScene("GamePlayScene");
    }
}
