using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class EnemyManager : MonoBehaviour,IDamageable
{
    GameObject _target;

    [SerializeField]
    float _distance;

    public int _health = 1000;

    int IDamageable.MaxHealth {  get { return _health; } set { _health = value; } }

    [SerializeField]
    private int _currentHealth = 1000;
    int IDamageable.CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    void IDamageable.HitDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log($"hit{damage}");
    }

 
}
public interface IDamageable
{
    int MaxHealth { get; set; }

    int CurrentHealth { get; set; }

    void HitDamage(int damage);

}
