using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
    }
}
