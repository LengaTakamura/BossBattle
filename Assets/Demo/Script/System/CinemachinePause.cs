using Unity.Cinemachine;
using UnityEngine;

public class CinemachinePause : MonoBehaviour,IPause
{
   CinemachineInputAxisController _input;

    private void Start()
    {
        _input = GetComponent<CinemachineInputAxisController>();
    }

    void IPause.Pause()
    {
        _input.enabled = false;
    }

    void IPause.Resume()
    {
        _input.enabled= true;
    }
}
