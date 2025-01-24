using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    public float shakeDuration = 0.2f; // Duration of the shake
    public float shakeMagnitude = 0.2f; // Magnitude of the shake

    private Vector3 originalPosition;
    private float shakeTimeRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.position = originalPosition + new Vector3(shakeOffset.x, shakeOffset.y, 0f);

            shakeTimeRemaining -= Time.deltaTime;

            if (shakeTimeRemaining <= 0)
            {
                transform.position = originalPosition;
            }
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeTimeRemaining = duration;
        shakeMagnitude = magnitude;
    }
}
