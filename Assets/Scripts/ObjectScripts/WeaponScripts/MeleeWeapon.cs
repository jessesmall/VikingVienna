using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {

    public int Damage = 0;

    private Vector2 lastposition,
        velocity;

    public void LateUpdate()
    {
        velocity = (lastposition - (Vector2)transform.position) / Time.deltaTime;
        lastposition = transform.position;
    }
}
