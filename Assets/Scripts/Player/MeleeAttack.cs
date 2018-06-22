using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    public float AttackSpeed = 1f;
    private float lastAttacked;

    private void Update()
    {
        if(lastAttacked > 0)
        {
            lastAttacked -= Time.deltaTime;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponentInChildren<StateController>();
        var enemyParent = collision.gameObject.GetComponentInParent<StateController>();
        if (enemy != null && enemy.tag == "Enemy")
        {
            if (lastAttacked <= 0)
            {
                enemy.TakeDamage();
                lastAttacked = AttackSpeed;
            }
        }
        else if (enemyParent != null)
        {
            if (lastAttacked <= 0)
            {
                enemyParent.TakeDamage();
                lastAttacked = AttackSpeed;
            }
        }
        return;
    }
}
