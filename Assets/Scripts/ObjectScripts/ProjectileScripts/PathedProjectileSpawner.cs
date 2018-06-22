using UnityEngine;
using System.Collections;

public class PathedProjectileSpawner : MonoBehaviour {

    public Transform Destination;
    public GameObject SpawnLocation;
    public PathedProjectile Projectile;

    public bool isFacingRight;

    public GameObject SpawnEffect;
    public float Speed;
    /// <summary>
    /// seconds / shot
    /// </summary>
    public float FireRate;

    private float nextShotInSeconds;

    public Animator Animator;
    public string AnimationToPlay;

    public AudioClip projectileSound;

    public virtual void Start()
    {
        nextShotInSeconds = FireRate;
    }

    public virtual void Update()
    {
        if ((nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
        projectile.transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
        projectile.Initialize(Destination.position, Speed);

        if (SpawnEffect != null)
            Instantiate(SpawnEffect, SpawnLocation.transform.position, transform.rotation);

        if (Animator != null)
            Animator.SetTrigger(AnimationToPlay);

        if (projectileSound != null)
            AudioManager.Instance.PlayClip3D(projectileSound, transform.position);


    }

    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(SpawnLocation.transform.position, Destination.position);
    }
}
