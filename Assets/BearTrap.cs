using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour {

    private bool _trapTriggered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<HealthManager>();

        if(player != null && !_trapTriggered)
        {
            player.ChangePlayerStatus(HealthManager.PlayerStatus.Stunned, 2f);
            GetComponentInChildren<Animator>().SetTrigger("BearTrap");
            _trapTriggered = true;
        }
    }
}
