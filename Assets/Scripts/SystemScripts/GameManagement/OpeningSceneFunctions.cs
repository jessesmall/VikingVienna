using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSceneFunctions : MonoBehaviour {

    public Transform jessePlayer;
    public Transform YetiArm;
    public GameObject particleEffect;
    public GameObject vikingVienna;
    public GameObject normalVienna;

    public AudioClip thunderClip;

    public MusicClass musicClass;

    public AudioSource soundEffectsAudio;

    public void StartEvent()
    {
        musicClass.PlayMusic("Hiking");
    }

    public void YetiEvent()
    {
        musicClass.PlayMusic("Yeti");
    }

    public void GrabEvent()
    {
        jessePlayer.transform.SetParent(YetiArm);
    }


    public void VikingViennaIntro()
    {
        particleEffect.SetActive(true);
        soundEffectsAudio.PlayOneShot(thunderClip);
    }

    public void VikingViennaEnable()
    {
        normalVienna.SetActive(false);
        vikingVienna.SetActive(true);
        particleEffect.SetActive(false);
    }

    public void FadeoutMusic()
    {
        musicClass.FadeOut();
    }
}
