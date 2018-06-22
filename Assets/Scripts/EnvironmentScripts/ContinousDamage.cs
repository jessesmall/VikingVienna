using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousDamage : MonoBehaviour {

    public int DamagePerTick;
    public float DamageTickTime;
    private float timeSinceLastTick = 0;

    public void Update()
    {
        if(timeSinceLastTick > 0)
        {
            timeSinceLastTick -= Time.deltaTime;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<HealthManager>();
        if(player != null && timeSinceLastTick <= 0)
        {
            player.TakeNoIFrameDamage();
            timeSinceLastTick = DamageTickTime;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponentInParent<HealthManager>();
        if(player != null)
        {
            if (timeSinceLastTick <= 0)
            {
                player.TakeNoIFrameDamage();
                timeSinceLastTick = DamageTickTime;
            }
        }

    }
}
