using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private void Start()
    {
        // Destroy the particle system after its lifetime ends
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}
