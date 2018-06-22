using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour {



    public void OnTriggerEnter2D(Collider2D collision)
    {
            var player = collision.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.canClimb = true;
            }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.canClimb = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
            var player = collision.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.canClimb = false;
            }
    }
}
