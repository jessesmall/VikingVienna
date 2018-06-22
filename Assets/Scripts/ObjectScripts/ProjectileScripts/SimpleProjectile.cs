using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleProjectile : Projectile
{
    public GameObject destroyedEffect;
    public ElementType Element;

    public float TimeToLive;

    private int lastTargetID = -1;

    public bool enemyProjectile = false;
    

    public void Update()
    {
        if((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }
        
        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + speed) * Time.deltaTime), Space.World);
    }

    protected override void OnCollideOther(Collider2D other)
    {
        if (!enemyProjectile)
        {
            if(other.gameObject.layer == LayerMask.GetMask("Enemy"))
            {
                CollideWithEnemy(other);
            }
            else
            {
                CollideWithWall(other);
            }
        }
        else
        {
            CollideWithPlayer(other);
        }
        DestroyProjectile();
    }

    private void CollideWithEnemy(Collider2D other)
    {
        // DEAL ENEMY DAMAGE
    }

    private void CollideWithPlayer(Collider2D other)
    {
        var player = other.gameObject.GetComponentInParent<HealthManager>();
        if(player != null)
        {
            player.TakeDamage();
        }
    }

    private void CollideWithWall(Collider2D other)
    {
        Debug.Log("HIT A WALL");
        var wall = other.gameObject.GetComponent<ElementalTile>();
        if(wall != null)
        {
            wall.HitWall(Element);
        }
    }

    private void DestroyProjectile()
    {
        if (destroyedEffect != null)
            Instantiate(destroyedEffect, transform.position, transform.rotation);
        var children = new List<GameObject>();
        foreach(Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
        Destroy(gameObject);
    }
}