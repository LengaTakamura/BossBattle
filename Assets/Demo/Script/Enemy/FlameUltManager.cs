using UnityEngine;

public class FlameUltManager : MonoBehaviour, IPause
{
    [SerializeField]
    private float _damage = 10f;

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

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            if (damage.CurrentHealth > 0)
                damage.HitDamage( _damage);
        }
    }
}
