using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {

    public AudioClip[] audioClips;

    public void PlayAudioClip(int i)
    {
        GetComponentInParent<AudioSource>().PlayOneShot(audioClips[i]);
    }
}
