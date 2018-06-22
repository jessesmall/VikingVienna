using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public PlayerController Player;
    public float VolumeScale;
    public int VolumeFallOffDistance;

    public void Awake()
    {
        Instance = this;
    }

    public void PlayClip3D(AudioClip clip, Vector2 effectPosition)
    {
        var volume = (1 - Vector2.Distance(Player.transform.position, effectPosition) / VolumeFallOffDistance) * VolumeScale;
        AudioSource.PlayClipAtPoint(clip, effectPosition, volume);
    }


}
