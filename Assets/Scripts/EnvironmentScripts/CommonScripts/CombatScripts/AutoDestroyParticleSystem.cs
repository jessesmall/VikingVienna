using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour {

    private ParticleSystem particles;

    public void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (particles.isPlaying)
            return;
        else
            Destroy(gameObject);
    }
}
