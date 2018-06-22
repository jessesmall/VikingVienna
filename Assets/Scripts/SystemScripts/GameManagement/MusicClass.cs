using System.Collections;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource[] _audioSources;
    public float fadeSpeed = 0.5f;
    public float fadeInOutMultiplier = 0.0f;
    public bool isPlaying;

    public string playingTrackName = "Nothing";
    public int playingTrackIndex = -1;
    public float playingTrackVolume = 0.000f;

    public string lastTrackName = "Nothing";
    public int lastTrackIndex = -1;
    public float lastTrackVolume = 0.000f;

    public IEnumerator FadeOutOldMusic()
    {
        while (_audioSources[playingTrackIndex].volume > 0f)
        {
            _audioSources[playingTrackIndex].volume -= fadeSpeed * 2;
            yield return new WaitForSeconds(0.1f);
            playingTrackVolume = _audioSources[playingTrackIndex].volume;
        }
        _audioSources[playingTrackIndex].volume = 0.000f; // Just In Case....
        _audioSources[playingTrackIndex].Stop();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutOldMusic());
    }

    public IEnumerator FadeOutOldMusic_FadeInNewMusic()
    {
        _audioSources[playingTrackIndex].volume = 0.000f;
        _audioSources[playingTrackIndex].Play();
        while (_audioSources[playingTrackIndex].volume < 1f)
        {
            _audioSources[lastTrackIndex].volume -= fadeSpeed * 2;
            _audioSources[playingTrackIndex].volume += fadeSpeed * 2;
            yield return new WaitForSeconds(0.001f);
            lastTrackVolume = _audioSources[lastTrackIndex].volume;
            playingTrackVolume = _audioSources[playingTrackIndex].volume;
        }
        _audioSources[lastTrackIndex].volume = 0.000f; // Just In Case....
        _audioSources[lastTrackIndex].Stop();

        lastTrackIndex = playingTrackIndex;
        lastTrackName = playingTrackName;
        isPlaying = true;
    }

    public IEnumerator FadeInNewMusic()
    {
        _audioSources[playingTrackIndex].volume = 0.000f;
        _audioSources[playingTrackIndex].Play();
        while (_audioSources[playingTrackIndex].volume < 1f)
        {
            _audioSources[playingTrackIndex].volume += fadeSpeed * 2;
            yield return new WaitForSeconds(0.001f);
            playingTrackVolume = _audioSources[playingTrackIndex].volume;
        }
        lastTrackIndex = playingTrackIndex;
        lastTrackName = playingTrackName;
        isPlaying = true;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSources = GetComponentsInChildren<AudioSource>();
    }

    public void PlayMusic(string transformName)
    {
        for (int a = 0; a < _audioSources.Length; a++)
        {
            if (_audioSources[a].name == transformName)
            {
                playingTrackIndex = a;
                playingTrackName = transformName;
                break;
            }
        }
        if (playingTrackIndex == lastTrackIndex)
        {
            return;
        }
        else
        {
            if (isPlaying)
            {
                StartCoroutine(FadeOutOldMusic_FadeInNewMusic());
            }
            else
            {
                StartCoroutine(FadeInNewMusic());
            }
        }
    }
}