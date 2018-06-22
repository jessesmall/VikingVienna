using Assets.Scripts.Player;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Teleporter : MonoBehaviour
{
    public Teleporter teleportTo;
    private bool teleportUsed = false;
    public Transform teleportLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerInput>();
        if (player != null && !teleportUsed)
        {
            teleportTo.teleportUsed = true;
            player.transform.position = teleportTo.teleportLocation.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerInput>();
        if (player != null)
        {
            teleportUsed = false;
        }
    }
}