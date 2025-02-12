
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField]
    AudioClip[]  _audioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// AnimationEvent用のメソッド
    /// </summary>
    public void PlaySe(string name)
    {
        _audioSource.PlayOneShot(SearchClip($"{name}"));
    }

    public void AudioPlayNone()
    {
        _audioSource.resource = SearchClip("None");
        _audioSource.Play();
    }


    public void AudioPause()
    {
        _audioSource.Stop();
        _audioSource.resource = null;
    }


    /// <summary>
    /// アサインされているクリップの中から指定されたクリップを返す
    /// </summary>
    AudioClip SearchClip(string clipName)
    {
         foreach (var clip in _audioClip)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
           
        }
         Debug.Log("Clipの名前が一致しません");
         return null;
    }
}
