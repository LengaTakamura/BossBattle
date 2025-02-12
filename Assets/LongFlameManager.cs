using UnityEngine;

public class LongFlameManager : MonoBehaviour,IPause
{
    [SerializeField]
    private float _damageBuff = 10f;

    ParticleSystem _particleSystem;

    private Vector3 _currentTargetPosition;

    private GameObject _target;
    EnemyManager _enemyManager;
    [SerializeField]
    float _speed;
    private void Start()
    {
        _enemyManager = transform.root.GetComponent<EnemyManager>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        _target = _enemyManager.Target;
        UpdateTartgetPos(_target.transform.position);
        MoveToTarget();
    }

    public void UpdateTartgetPos(Vector3 newPos)
    {
        _currentTargetPosition = newPos;
    }

    public void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTargetPosition + new Vector3(0, 3, 0), Time.deltaTime * _speed);
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
            var enemy = transform.root.GetComponent<IDamageable>();
            if (damage.CurrentHealth > 0)
                damage.HitDamage(enemy.AttackPower + _damageBuff);
        }
    }
}
