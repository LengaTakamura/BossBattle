
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
    private float _attackPower = 100;
    float IDamageable.AttackPower {  get { return _attackPower; } set {_attackPower = value; } }

    [SerializeField]
    EnemyHealthBarManager _enemyHealthBarManager;

    Animator _animator;
    [SerializeField]
    float _speed = 5f;

    CancellationTokenSource _cts;

    private EnemyState _enemyState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cts = new CancellationTokenSource();
    }
    void Start()
    {
        _currentHealth = _health;
    }

    private void Update()
    {       
        LookTarget();
        ChangeState();
    }

    private void LookTarget()
    {
        var rot = _target.transform.position;
        rot.y = transform.position.y;
        transform.LookAt(rot);
    }


    public void ChangeState()
    {
        EnemyState state = _enemyState;
        switch (_enemyState) 
        {
            case EnemyState.Sleep:
                Sleep();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.OnBattle:
                OnBattle();
                break;
            case EnemyState.Move:
                Move();
                break;

        }

    }

    void Sleep()
    {

    }

    void OnBattle()
    {

    }

    void Attack()
    {

    }

    void Move()
    {

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
        if(_currentHealth / _health < 0.5)
        {
            _animator.SetTrigger("Ult");
            _cts.Cancel();
        }
        if (_currentHealth <= 0)
        {
            _animator.SetTrigger("Death");
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    void IDamageable.HitHeal(float value)
    {


    }


}


/// <summary>
/// 敵の行動の種類
/// </summary>
public enum EnemyState
{
    OnBattle,
    Attack,
    Sleep,
    Move


}


/// <summary>
/// HP増減処理を管理するインターフェース
/// </summary>
public interface IDamageable
{
    float MaxHealth { get; set; }

    float CurrentHealth { get; set; }

    void HitDamage(float damage);

    void HitHeal(float value);

    float AttackPower {  get; set; }

}
