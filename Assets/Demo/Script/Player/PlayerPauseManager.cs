using UnityEngine;

public class PlayerPauseManager : MonoBehaviour,IPause
{
    Playermanager _playerManager;

    private void Start()
    {
        _playerManager = GetComponent<Playermanager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void IPause.Pause()
    {
        _playerManager.OnPause();
    }

    void IPause.Resume()
    {
        _playerManager.OnResume();
    }
}
