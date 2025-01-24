using System.Collections;
using UnityEngine;

public class ActivateParticle : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // Reference to your particle system prefab
    public AudioClip thudSound; // Reference to the thud sound
    private AudioSource audioSource; // Reference to the AudioSource component
    public float thudSoundDuration = 1f; // Duration for the thud sound to play (in seconds)

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Vector3 clickPosition = GetMouseWorldPosition(); // Get world position of the click

            // Instantiate and play the particle system at the click position
            ParticleSystem instance = Instantiate(particleSystemPrefab, clickPosition, Quaternion.identity);
            instance.Play();

            // Play the thud sound
            if (audioSource != null && thudSound != null)
            {
                audioSource.PlayOneShot(thudSound); // Plays the sound once
                StartCoroutine(LimitSoundDuration()); // Limit the sound duration
            }

            // Destroy the particle system after it has finished playing
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);

            // Trigger screen shake
            if (ScreenShake.Instance != null)
            {
                ScreenShake.Instance.TriggerShake(0.2f, 0.2f); // Adjust duration and magnitude as needed
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Convert screen position to world position
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane; // Adjust this based on your camera setup
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    // Coroutine to limit the sound duration
    private IEnumerator LimitSoundDuration()
    {
        // Wait for the duration of the thud sound
        yield return new WaitForSeconds(thudSoundDuration);

        // Stop the sound if it's still playing (in case the sound is longer than the set duration)
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
