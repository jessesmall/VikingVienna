using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogueCharacter : Interactable {

    private bool _itemRequired;
    public Item RequiredItem;
    [TextArea(1, 10)]
    public string IntroString;
    [TextArea(1,10)]
    public string SuccessString;

    private bool _interacted = false;

    public UnityEvent Event;

    public override void Start()
    {
        if (RequiredItem != null)
            _itemRequired = true;
        base.Start();
    }

    private IEnumerator GiveItem()
    {
        TextBoxInteraction.Instance.ShowText(SuccessString, DisplayTime, fontStyle);
        _player.GetComponent<InventoryManager>().RemoveItem(RequiredItem);
        yield return new WaitForSeconds(DisplayTime);
        RunScript();
        DestoryGameObject();
    }

    private void RunScript()
    {
        Event.Invoke();
    }

    private void DestoryGameObject()
    {
        Destroy(gameObject);
    }

    public override void Interact()
    {
        var hasItem = _player.GetComponent<InventoryManager>().ItemArray.Contains(RequiredItem);
        if (!_itemRequired)
            RunScript();
        else if (hasItem)
            StartCoroutine(GiveItem());
        else if (!_interacted)
        {
            TextBoxInteraction.Instance.ShowText(IntroString, DisplayTime, fontStyle);
            _interacted = true;
        }
        else
            base.Interact();
    }
}
