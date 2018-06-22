using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Stats")]
public class EnemyStats : ScriptableObject {

    //Combat
    public float attackRange;
    public float attackSpeed;

    //Vision
    public int lookDistance;
    public int fov;

    //Movement
    public float speed;
    public float jumpHeight;

    //Health
    public int maxHealth;

    public GameObject hurtEffect;
    public GameObject destroyEffect;

}
