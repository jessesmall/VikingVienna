using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public AudioClip levelMusic;
    public AudioClip victoryMusic;
    public AudioClip deathMusic;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    public float maxSoundDistance = 10f;
    public float maxHeight = 5f;

    public GameObject _player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            _player = FindObjectOfType<PlayerController>().gameObject;
        }
        else if(instance != this)
        {
            //Destroy(gameObject);
        }
    }

    private bool IsWithinRange(Transform soundOrigin)
    {
        var heightDifference = Mathf.Abs(_player.transform.position.y - soundOrigin.position.y);
        if (Mathf.Abs(Vector2.Distance(_player.transform.position, soundOrigin.position)) < maxSoundDistance && heightDifference < maxHeight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlaySingle(Transform soundOrigin, AudioClip clip)
    {
        if (IsWithinRange(soundOrigin))
        {
            efxSource.PlayOneShot(clip);
        }
    }

    public void RandomSfx(Transform soundOrigin, params AudioClip[] clips)
    {
        if (IsWithinRange(soundOrigin))
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            efxSource.pitch = randomPitch;
            efxSource.PlayOneShot(clips[randomIndex]);
        }
    }

    public void OnPlayerDied()
    {
        musicSource.Stop();
        musicSource.PlayOneShot(deathMusic);
    }

    public void OnPlayerRespawned()
    {
        musicSource.Stop();
        musicSource.clip = levelMusic;
        musicSource.Play();
    }

    public void OnLevelComplete()
    {
        musicSource.Stop();
        musicSource.PlayOneShot(victoryMusic);
    } 
}
