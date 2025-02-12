using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] _audioClip;
    
    AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// AnimationEventで呼び出すメソッド
    /// </summary>
    public void PlaySE(string name)
    {
        _audioSource.PlayOneShot(SearchClip($"{name}"));
    }

    public void AudioPause()
    {
        _audioSource.Stop();
        _audioSource.resource = null;
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
        Debug.Log("Clipの名前が一致しません");
        return null;
    }
}
