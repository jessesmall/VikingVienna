using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHandler : MonoBehaviour {

    [SerializeField]
    private AudioClip meleeAttackAudio;
    [SerializeField]
    private AudioClip rangedAttackAudio;
    [SerializeField]
    private AudioClip jumpAudio;
    [SerializeField]
    private AudioClip deathAudio;
    [SerializeField]
    private AudioClip walkAudio;
    [SerializeField]
    private AudioClip hitPlayerAudio;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MeleeAttackAudio()
    {
        audioSource.PlayOneShot(meleeAttackAudio);
    }

    public void RangedAttackAudio()
    {
        audioSource.PlayOneShot(rangedAttackAudio);
    }

    public void JumpAudio()
    {
        audioSource.PlayOneShot(jumpAudio);
    }

    public void DeathAudio()
    {
        audioSource.PlayOneShot(deathAudio);
    }

    public void WalkAudio()
    {
        audioSource.PlayOneShot(walkAudio);
    }
}
