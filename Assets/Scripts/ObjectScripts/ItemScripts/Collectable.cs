using UnityEngine;
using System.Collections;

public abstract class Collectable : MonoBehaviour, IPlayerRespawnListener {

    public GameObject Effect;
    public Animator Animator;
    public string AnimationTrigger;

    private bool isCollected;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected)
            return;

        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        OnCollected(player);
        Instantiate(Effect, transform.position, transform.rotation);

        isCollected = true;
        Animator.SetTrigger(AnimationTrigger);
    }

    abstract public void OnCollected(PlayerController player);

    public void FinishAnimationEvent()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayerRespawnInThisCheckPoint(CheckPoint checkPoint, PlayerController player)
    {
        isCollected = false;
        gameObject.SetActive(true);
    }
}
