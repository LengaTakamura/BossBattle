
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
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

    [SerializeField]
    float _minDistance = 1;

    private bool _onDamaged = false;

    [SerializeField]
    float _timer;
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
                //Move();
                break;

        }

    }


    public  EnemyState ChangeState(EnemyState state)
    {
         
        switch (_enemyState) 
        {
            case EnemyState.Sleep:
                {
                    if(GetDistance(_target.transform.position) < _distance)
                    {
                        state = EnemyState.None;
                    }

                    if (_onDamaged)
                    {
                        state = EnemyState.None;
                    }
                }
                break;
            case EnemyState.Attack:
                {
                   
                }
                break;
            case EnemyState.None:
                {
                    _timer = Time.time;
                     Debug.Log(_timer);

                    if (_timer + 3f < Time.time)
                    {
                        state = EnemyState.Attack;
                    }
                    if (GetDistance(_target.transform.position) > _distance)
                    {
                        state = EnemyState.Move;
                    }
                }
               
                break;
            case EnemyState.Move:
                {
                    if (GetDistance(_target.transform.position) < _minDistance)
                    {
                        state = EnemyState.None;                    
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

    void Attack()
    {

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
    None,
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
