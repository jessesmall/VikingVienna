using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public int InventorySize = 8;
    public Selectable[] ItemSlots;
    [HideInInspector]
    public Item[] ItemArray;
    private Image[] ItemIcons;
    private int CurrentItemIndex = 0;

    public Sprite DefaultItemSprite;

    public void Start()
    {
        ItemArray = new Item[InventorySize];
        ItemIcons = new Image[InventorySize];

        for (int i = 0; i < InventorySize; i++)
        {
            ItemIcons[i] = ItemSlots[i].gameObject.transform.Find("ItemSlot/ItemIcon").GetComponent<Image>();
            ItemIcons[i].sprite = DefaultItemSprite;
        }

        ItemSlots[CurrentItemIndex].Select();
        ItemSlots[CurrentItemIndex].OnSelect(null);
    }

    public void AddItem(Item item)
    {
        int freeItemSlot = Array.IndexOf(ItemArray, null);
        ItemArray[freeItemSlot] = item;
        ItemIcons[freeItemSlot].sprite = item.ItemIcon;
        EnableItemText();
    }

    public void RemoveItem(int itemIndex)
    {
        ItemArray[itemIndex] = null;
        ItemIcons[itemIndex].sprite = DefaultItemSprite;
    }

    public void RemoveItem(Item item)
    {
        int itemIndex = Array.FindIndex(ItemArray, x => x == item);
        RemoveItem(itemIndex);
    }

    public void UseItem()
    {
        if(ItemArray[CurrentItemIndex] != null)
        {
            if (ItemArray[CurrentItemIndex].Use(gameObject))
            {
                RemoveItem(CurrentItemIndex);
                DisableItemText();
            }
        }
    }

    public void UpdateCurrentItemIndex(int newIndex)
    {
        DisableItemText();
        ItemSlots[CurrentItemIndex].OnDeselect(null);
        CurrentItemIndex = (newIndex - 1);
        ItemSlots[CurrentItemIndex].Select();
        ItemSlots[CurrentItemIndex].OnSelect(null);
        EnableItemText();
    }
    public void UpdateCurrentItemIndex(bool increase)
    {
        DisableItemText();
        ItemSlots[CurrentItemIndex].OnDeselect(null);
        CurrentItemIndex = increase ? CurrentItemIndex + 1 : CurrentItemIndex - 1;
        if(CurrentItemIndex < 0)
        {
            CurrentItemIndex = InventorySize - 1;
        }
        else if (CurrentItemIndex >= InventorySize)
        {
            CurrentItemIndex = 0;
        }
        ItemSlots[CurrentItemIndex].Select();
        ItemSlots[CurrentItemIndex].OnSelect(null);
        EnableItemText();
    }

    private void EnableItemText()
    {
        var itemText = ItemSlots[CurrentItemIndex].GetComponentInChildren<Text>();
        itemText.text = ItemArray[CurrentItemIndex] != null ? ItemArray[CurrentItemIndex].ItemName : "";
        ItemSlots[CurrentItemIndex].GetComponentInChildren<Text>().enabled = true;
    }

    private void DisableItemText()
    {
        var itemText = ItemSlots[CurrentItemIndex].GetComponentInChildren<Text>();
        itemText.text = "";
        ItemSlots[CurrentItemIndex].GetComponentInChildren<Text>().enabled = false;
    }
}
