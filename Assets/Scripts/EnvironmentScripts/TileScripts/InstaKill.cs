using UnityEngine;
using System.Collections;

public class InstaKill : MonoBehaviour {

	public void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player == null)
            return;

        LevelManager.Instance.KillPlayer();
    }
}
