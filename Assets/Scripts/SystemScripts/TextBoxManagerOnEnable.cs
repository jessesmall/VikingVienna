using Assets.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManagerOnEnable : MonoBehaviour {

    public GameObject textBox;
    public GameObject textBoxPanel;

    public Text dialogueText;

    public TextAsset textFile;
    public string[] textLines;


    public int currentLineIndex;

    public int endAtLineIndex;

    public string dialogueKey;

    // Use this for initialization
    public void StartScript()
    {
        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }

        currentLineIndex = 0;
        endAtLineIndex = textLines.Length - 1;

        StartCoroutine("CheckTextBox");
    }

    IEnumerator CheckTextBox()
    {
        while (currentLineIndex < endAtLineIndex)
        {
            if (currentLineIndex > endAtLineIndex)
            {
                DisableTextBox();
                break;
            }
            if (textLines[currentLineIndex].Split(':')[0] == dialogueKey)
            {
                EnableTextBox();
                dialogueText.text = textLines[currentLineIndex].Split('-')[0];
            }
            else
            {
                DisableTextBox();
            }

            if (currentLineIndex + 1 > endAtLineIndex)
            {
                DisableTextBox();
                currentLineIndex++;
                break;
            }
            double secondsToWait = Convert.ToDouble(textLines[currentLineIndex + 1].Split('-')[1]) - Convert.ToDouble(textLines[currentLineIndex].Split('-')[1]);
            currentLineIndex++;
            yield return new WaitForSeconds((float)secondsToWait);
        }
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
