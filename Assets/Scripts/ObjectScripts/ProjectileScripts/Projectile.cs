using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

    public float speed;
    public LayerMask collisionMask;
    public float currentSpeedX;
    public float currentSpeedY;

    public float destinationDistanceX;
    public float destinationDistanceY;

    public bool stopped = false;

    public GameObject Owner { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector2 InitialVelocity { get; private set; }

    public string projectileAnimation;

    public void Initialize(GameObject owner, Vector2 direction, Vector2 initialVelocity)
    {
        this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.right = direction;
        Owner = owner;
        Direction = direction;
        InitialVelocity = initialVelocity;
        OnInitialized();
    }

    protected virtual void OnInitialized()
    {
        currentSpeedX = InitialVelocity.x + speed;
        currentSpeedY = InitialVelocity.y + speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if((collisionMask.value & (1 << other.gameObject.layer)) == 0)
        {
            OnNotCollideWith(other);
            return;
        }

        var isOwner = other.gameObject == Owner;
        if (isOwner)
        {
            OnCollideOwner();
            return;
        }
        OnCollideOther(other);
    }

    protected virtual void OnNotCollideWith(Collider2D other)
    {

    }

    protected virtual void OnCollideOwner()
    {

    }

    protected virtual void OnCollideOther(Collider2D other)
    {

    }

}
