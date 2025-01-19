using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActivateParticle : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // Reference to your particle system prefab

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Vector3 clickPosition = GetMouseWorldPosition(); // Get world position of the click

            // Instantiate and play the particle system at the click position
            ParticleSystem instance = Instantiate(particleSystemPrefab, clickPosition, Quaternion.identity);
            instance.Play();

            // Destroy the particle system after it has finished playing
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Convert screen position to world position
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane; // Adjust this based on your camera setup
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}

