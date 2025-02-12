using System;
using UnityEngine;

public class BatEffectManager : MonoBehaviour
{
    [SerializeField]
    float _speed = 3f;

    GameObject _bat;

    [SerializeField]
    float _damage = 5f;

    private Vector3 _currentTargetPosition;

    public Action<float> HitAction;
    [SerializeField]
    private float _energy ; 
    public void Initialized(BatManager bat)
    {
        bat.GetTargetPos += UpdateTartgetPos;
    }

    private void Start()
    {
        var archer =GameObject.Find("PlayerManager").transform;
        var archerManager = archer.Find("Archer").GetComponent<PlayerArcher>();
        HitAction += archerManager.AddEnergy;
    }

    private void Update()
    {
        MoveToTarget();
    }

    private void UpdateTartgetPos(Vector3 newPos)
    {
        _currentTargetPosition = newPos;
    }
    public void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTargetPosition + new Vector3(0,3,0), Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Enemy")
        {
            if(damage.CurrentHealth > 0)
            damage.HitDamage(_damage);
            HitAction?.Invoke(_energy);
            Destroy(gameObject);
        }
    }
}
