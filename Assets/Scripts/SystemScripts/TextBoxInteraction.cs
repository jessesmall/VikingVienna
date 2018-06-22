using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxInteraction : MonoBehaviour {

    public GameObject textBoxPanel;

    public Text dialogueText;

    private static TextBoxInteraction _instance;
    public static TextBoxInteraction Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ShowText(string message, float displayTime, FontStyle fontStyle = FontStyle.Normal)
    {
        StartCoroutine(DisplayText(message, displayTime, fontStyle));
    }

    IEnumerator DisplayText(string message, float displayTime, FontStyle fontStyle)
    {
        dialogueText.text = message;
        dialogueText.fontStyle = fontStyle;
        EnableTextBox();
        yield return new WaitForSeconds(displayTime);
        dialogueText.fontStyle = FontStyle.Normal;
        DisableTextBox();
    }

    private void DisableTextBox()
    {
        textBoxPanel.SetActive(false);
    }

    private void EnableTextBox()
    {
        textBoxPanel.SetActive(true);
    }
}
