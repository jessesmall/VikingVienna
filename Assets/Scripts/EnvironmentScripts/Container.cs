using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable {

    public Item Loot;
    public KeyItem.Type KeyType;
    public bool KeyRequired = true;

    public AudioClip OpenSound;

    public bool isTrap = false;
    public GameObject trapEffect;

    public void OpenContainer()
    {
        if (!isTrap)
        {
            var inventory = _player.GetComponent<InventoryManager>();
            inventory.AddItem(Loot);
        }
        else
        {
            Instantiate(trapEffect, this.transform.position, this.transform.rotation);
        }
        DestoryGameObject();
    }

    private void DestoryGameObject()
    {
        Destroy(gameObject);
    }

    public override void Interact()
    {
        if (!KeyRequired)
            OpenContainer();
        else
            base.Interact();
    }
}
