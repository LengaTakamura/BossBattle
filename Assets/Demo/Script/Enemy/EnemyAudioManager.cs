using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField]
    AudioClip[]  _audioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlayAttack()
    {
        _audioSource.PlayOneShot(SearchClip("Attack"));
    }

    public void AudioPlayMove()
    {
        _audioSource.resource = SearchClip("Move");
        _audioSource.Play();
    }
    public void AudioPauseMove()
    {
        _audioSource.Stop();
        _audioSource.resource=null;
    }

    public void AudioPlayNone()
    {
        _audioSource.resource = SearchClip("None");
        _audioSource.Play();
    }

    public void AudioPauseNone()
    {
        _audioSource.Stop();
        _audioSource.resource = null;
    }

    public void AudioPlayFlame()
    {
        _audioSource.PlayOneShot(SearchClip("Flame"));
    }

    AudioClip SearchClip(string clipName)
    {
         foreach (var clip in _audioClip)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
           
        }
         Debug.Log("Clip‚Ì–¼‘O‚ªˆê’v‚µ‚Ü‚¹‚ñ");
         return null;
    }
}
