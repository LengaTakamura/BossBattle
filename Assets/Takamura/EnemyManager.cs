using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class EnemyManager : MonoBehaviour,IDamageable
{
    GameObject _target;

    [SerializeField]
    float _distance;

    public int _health = 1000;

    int IDamageable.Health {  get { return _health; } set { _health = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

    void IDamageable.AddDamage(int damage)
    {

    }

 
}
public interface IDamageable
{
    int Health { get; set; }

    void AddDamage(int damage);

}
