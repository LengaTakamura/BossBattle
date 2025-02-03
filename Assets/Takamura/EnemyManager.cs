using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class EnemyManager : MonoBehaviour,IDamageable
{
    GameObject _target;

    [SerializeField]
    float _distance;

    public float _health = 1000;

    float IDamageable.MaxHealth {  get { return _health; } set { _health = value; } }

    [SerializeField]
    private float _currentHealth = 1000;
    float IDamageable.CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    [SerializeField]
    EnemyHealthBarManager _enemyHealthBarManager;

    Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        _currentHealth = _health;
    }

    private void Update()
    {
        LookTarget();
    }

    private void LookTarget()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) > _distance)
        {
            var rot = _target.transform.position;
            rot.y = transform.position.y;
            transform.LookAt(rot);
        }

    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    void IDamageable.HitDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"hit{damage}");
        _enemyHealthBarManager.FillUpdate(_currentHealth / _health);
        if(_currentHealth / _health < 0.2)
        {
            _animator.SetTrigger("Ult");
        }
    }

 
}
public interface IDamageable
{
    float MaxHealth { get; set; }

    float CurrentHealth { get; set; }

    void HitDamage(float damage);

}
