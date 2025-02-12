using UnityEngine;

public class CameraPauseManager : MonoBehaviour,IPause
{

    [SerializeField]
    GameObject _camera;
    void IPause.Pause()
    {
        _camera.SetActive(false);
    }

    void IPause.Resume()
    {
        _camera.SetActive(true);

    }
}
