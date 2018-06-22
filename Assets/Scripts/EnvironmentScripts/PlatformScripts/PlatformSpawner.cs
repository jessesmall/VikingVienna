using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour {

    public Transform Destination;
    public SpawnedPlatform Platform;

    public float MinSpeed;
    public float MaxSpeed;

    private float _platformSpeed;

    public float MaxSpawnRate;
    public float MinSpawnRate;
    private float _spawnRate = 0;


    private float nextShotInSeconds;

    public void Start()
    {
        nextShotInSeconds = _spawnRate;
    }

    public void Update()
    {
        if ((nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        _platformSpeed = Random.Range(MinSpeed, MaxSpeed);
        _spawnRate = Random.Range(MinSpawnRate, MaxSpawnRate);
        nextShotInSeconds = _spawnRate;
        var projectile = (SpawnedPlatform)Instantiate(Platform, transform.position, transform.rotation);
        projectile.Initialize(Destination, _platformSpeed);
    }

    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
