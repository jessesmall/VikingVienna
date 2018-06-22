using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAudio/Audio")]
public class ScriptableAudio : ScriptableObject {

    public AudioClip[] meleeAttackAudio;
    public AudioClip[] rangedAttackAudio;
    public AudioClip[] jumpAudio;
    public AudioClip[] deathAudio;
    public AudioClip[] hurtAudio;
    public AudioClip[] walkAudio;
    public AudioClip[] specialAudio;
}
