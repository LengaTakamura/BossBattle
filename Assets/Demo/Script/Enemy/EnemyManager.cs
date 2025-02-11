using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

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

    [SerializeField]
    float _minDistance = 1;

    private bool _onDamaged = false;

    [SerializeField]
    float _timer;

    public bool IsAttackEnd = false;

    private bool _onPause = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cts = new CancellationTokenSource();
    }
    void Start()
    {
        _currentHealth = _health;
        _enemyState = EnemyState.Sleep;
    }

    private void Update()
    {   
        if (_onPause)
        {
            return;
        }
        LookTarget();
        EnemyState state = ChangeState(_enemyState);
        if(state != _enemyState)
        {
            _enemyState = state;
            SwitchState(state);
        }
        if(_enemyState == EnemyState.Move)
        MoveUpdate(_target.transform.position);
    }

    private void LookTarget()
    {
        var rot = _target.transform.position;
        rot.y = transform.position.y;
        transform.LookAt(rot);
    }

    public void SwitchState(EnemyState state)
    {
        
        switch (_enemyState)
        {
            case EnemyState.Sleep:
                Sleep();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.None:
                None();
                break;
            case EnemyState.Move:
                Move();
                break;

        }

    }


    public  EnemyState ChangeState(EnemyState state)
    {
         
        switch (state) 
        {
            case EnemyState.Sleep:
                {
                    if(GetDistance(_target.transform.position) < _distance)
                    {
                        state = EnemyState.None;
                        _timer = Time.time;
                    }

                    if (_onDamaged)
                    {
                        state = EnemyState.None;
                        _timer = Time.time;
                    }
                }
                break;
            case EnemyState.Attack:
                {
                    if (IsAttackEnd)
                    {
                        _animator.SetBool("Attack", false);
                        state = EnemyState.None;
                        _timer = Time.time;
                        IsAttackEnd = false;
                    }
                }
                break;
            case EnemyState.None:
                {

                    if (_timer + 3f < Time.time)
                    {
                        state = EnemyState.Attack;
                        _timer = 0;
                    }
                    if (GetDistance(_target.transform.position) > _distance)
                    {
                        state = EnemyState.Move;
                        _timer = 0;
                    }
                }
               
                break;
            case EnemyState.Move:
                {
                    if (GetDistance(_target.transform.position) < _minDistance)
                    {
                        _animator.SetBool("Move", false);
                        state = EnemyState.None;
                        _timer = Time.time;
                    }
                }
                break;

        }
        return state;

    }

    float  GetDistance(Vector3 vect)
    {
        return Vector3.Distance(transform.position, vect);
    }

   

    void Sleep()
    {

    }

    void None()
    {

    }

    void Move()
    {
        _animator.SetBool("Move", true);
    }

    void Attack()
    {
        _animator.SetBool("Attack", true);
    }

    void MoveUpdate(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target + new Vector3(0, -1, 0), Time.deltaTime * _speed);
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
        _onDamaged = true;
        if(_currentHealth / _health < 0.3)
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

    public float OnPause()
    {
        _onPause = true;
        _enemyState = EnemyState.None;
        float speed = _animator.speed;
        _animator.speed = 0;
        return speed;
    }

    public void OnResume(float speed)
    {
        _onPause = false;
        _animator.speed = speed;
    }


}


/// <summary>
/// 敵の行動の種類
/// </summary>
public enum EnemyState
{
    None,
    Attack,
    Sleep,
    Move
}


/// <summary>
/// 敵の攻撃の種類
/// </summary>
public enum EnemyAttack
{
    Normal = 0,
    Claw = 1,
    Jump = 2,
    Flame = 3,

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
