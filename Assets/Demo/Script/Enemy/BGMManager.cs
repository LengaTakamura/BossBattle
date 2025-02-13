using UnityEngine;

public class BGMManager : MonoBehaviour,IPause
{
    AudioSource _audioSorce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSorce = GetComponent<AudioSource>();
    }

    void IPause.Pause()
    {
        _audioSorce.Pause();
    }

    void IPause.Resume()
    {
        _audioSorce.Play();
    }
}
