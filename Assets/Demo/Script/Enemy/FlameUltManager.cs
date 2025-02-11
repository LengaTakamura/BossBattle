using UnityEngine;

public class FlameUltManager : MonoBehaviour, IPause
{
   
    ParticleSystem _particleSystem;


    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }


    void IPause.Pause()
    {
        _particleSystem.Pause();
    }

    void IPause.Resume()
    {
        _particleSystem.Play();
    }

   
  
}
