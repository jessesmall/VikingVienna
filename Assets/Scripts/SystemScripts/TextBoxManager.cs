using Assets.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

    public List<TextBox> TextBoxes;

    public TextAsset TextFile;
    private string[] _textLines;


    private int _currentLineIndex;
    private int _endAtLineIndex;

    private PlayerInput player;

    private TextBox _currentTextBox;
    private Text _currentText;

    public bool AutoPlay = true;

    // Use this for initialization
    void Awake()
    {
        player = FindObjectOfType<PlayerInput>();
        if (TextFile != null)
        {
            _textLines = TextFile.text.Split('\n');
        }

        _currentLineIndex = 0;
        _endAtLineIndex = _textLines.Length - 1;
    }

    private void Start()
    {
        if (AutoPlay)
        {
            if (player != null)
                player.DisablePlayerInput();
            StartCoroutine("CheckTextBox");
        }
    }

    public void StartScene()
    {
        if (player != null)
            player.DisablePlayerInput();
        StartCoroutine(CheckTextBox());
    }

    public IEnumerator CheckTextBox()
    {
        while (_currentLineIndex < _endAtLineIndex)
        {
            if (_currentLineIndex + 1 > _endAtLineIndex)
            {
                DisableTextBox();
                break;
            }

            var nextTextBox = TextBoxes.Where(x => x.DialogueKey == _textLines[_currentLineIndex].Split(':')[0]).SingleOrDefault();

            if (_currentTextBox != nextTextBox)
            {
                DisableTextBox();
                _currentTextBox = nextTextBox;
                EnableTextBox();
            }

            _currentText.text = _textLines[_currentLineIndex].Split('-')[0];

            double secondsToWait = Convert.ToDouble(_textLines[_currentLineIndex + 1].Split('-')[1]) - Convert.ToDouble(_textLines[_currentLineIndex].Split('-')[1]);
            _currentLineIndex++;
            yield return new WaitForSeconds((float)secondsToWait);
        }
        DisableTextBox();
        if(player != null)
            player.EnablePlayerInput();
    }

    private void DisableTextBox()
    {
        if(_currentTextBox != null)
        {
            _currentTextBox.gameObject.SetActive(false);
        }
    }

    private void EnableTextBox()
    {
        if (_currentTextBox != null)
        {
            _currentText = _currentTextBox.GetComponentInChildren<Text>();
            _currentTextBox.gameObject.SetActive(true);
        }
    }
}
