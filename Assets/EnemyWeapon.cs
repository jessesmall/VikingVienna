using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<HealthManager>();

        if(player != null)
        {
            player.TakeDamage();
        }
    }
}
