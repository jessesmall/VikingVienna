using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    private bool _trapTriggered = false;
    public float effectTime = 2f;
    public HealthManager.PlayerStatus status;
    public AudioClip activateSound;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<HealthManager>();

        if(player != null && !_trapTriggered)
        {
            player.ChangePlayerStatus(status, effectTime);
            GetComponentInChildren<Animator>().SetTrigger("Trap");
            SoundManager.instance.PlaySingle(transform, activateSound);
            _trapTriggered = true;
        }
    }
}
