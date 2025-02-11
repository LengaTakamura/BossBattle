using UnityEngine;

public class FlameParticleManager : MonoBehaviour,IPause
{
    [SerializeField]
    private float _damageBuff = 10f;

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
        if(other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            var enemy = transform.root.GetComponent<IDamageable>();
            if (damage.CurrentHealth > 0)
                damage.HitDamage(enemy.AttackPower + _damageBuff);
        }
    }
}
