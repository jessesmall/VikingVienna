using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickPlatform : MonoBehaviour, IPlayerRespawnListener {

    public enum TrickType { Fade, Shock, Fake };
    public TrickType Trick;

    public float EffectTime = 1.5f;

    public GameObject TrickEffect;

    private bool _effectTriggered;

    private SpriteRenderer[] renders;

    public void Start()
    {
        renders = GetComponentsInChildren<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerController>();
        if(player != null && !_effectTriggered)
        {
            _effectTriggered = true;
            switch (Trick)
            {
                case TrickType.Fade:
                    StartCoroutine(Fade());
                    break;
                case TrickType.Shock:
                    break;
                case TrickType.Fake:
                    Fake();
                    break;
            }
        }
    }

    public IEnumerator Fade()
    {
        for(float i = 0; i < EffectTime*10; i++)
        {
            foreach(var spriteRender in renders)
            {
                spriteRender.enabled = !spriteRender.enabled;
            }
            yield return new WaitForSeconds(0.1f);
        }
        if(TrickEffect != null)
            Instantiate(TrickEffect, transform.position, transform.rotation, null);
        gameObject.SetActive(false);
    }

    public void Fake()
    {

    }

    public void OnPlayerRespawnInThisCheckPoint(CheckPoint checkPoint, PlayerController player)
    {
        if (_effectTriggered)
        {
            foreach(var spriteRender in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRender.enabled = true;
            }
            gameObject.SetActive(true);
            _effectTriggered = false;
        }
    }
}
