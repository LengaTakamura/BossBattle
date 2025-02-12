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
    /// “G‚ÌUŒ‚‚Ìí—Ş
    /// </summary>
    public void PlaySe(string name)
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
        Debug.Log("Clip‚Ì–¼‘O‚ªˆê’v‚µ‚Ü‚¹‚ñ");
        return null;
    }
}
