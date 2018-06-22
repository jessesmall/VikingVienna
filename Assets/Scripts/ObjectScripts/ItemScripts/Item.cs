using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Item : MonoBehaviour, IPlayerRespawnListener {

    public string ItemName;
    public Sprite ItemIcon;

    public AudioClip PickUpSound;
    public AudioClip UseSound;
    public GameObject PickUpEffect;

    private bool isCollected;

    [TextArea(1, 10)]
    public string OptionalItemUseText;
    public float DisplayTime;
    public FontStyle Font;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected)
        {
            return;
        }

        var inventory = other.GetComponentInParent<InventoryManager>();
        if(inventory == null)
        {
            return;
        }

        OnCollected(inventory);
        //Instantiate(PickUpEffect, transform.position, transform.rotation);
        isCollected = true;
        gameObject.SetActive(false);

    }

    public void OnCollected(InventoryManager inventory)
    {
        inventory.AddItem(this);
    }

    public void OnPlayerRespawnInThisCheckPoint(CheckPoint checkPoint, PlayerController player)
    {
        isCollected = false;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Returns True if we were able to use the item, False if we could not
    /// </summary>
    /// <param name="caller"></param>
    /// <returns></returns>
    public virtual bool Use(GameObject caller)
    {
        TextBoxInteraction.Instance.ShowText(OptionalItemUseText, DisplayTime, Font);
        return true;
    }
}
