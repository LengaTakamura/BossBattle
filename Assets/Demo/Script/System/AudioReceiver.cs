using UnityEngine;

public class AudioReceiver : MonoBehaviour
{
    public void PlaySe(string name)
    {
        var audio = transform.root.GetComponent<PlayerAudioManager>();
        audio.PlaySE(name);
    }
}
