using UnityEngine;
using System.Collections;

public class PathedProjectile : MonoBehaviour {

    private Vector3 _destination;
    private float _speed;

    public GameObject DestroyEffect;

    public void Initialize(Vector3 destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * _speed);

        var distanceSquared = (_destination - transform.position).sqrMagnitude;
        if (distanceSquared > 0.01f * 0.01f)
            return;
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
