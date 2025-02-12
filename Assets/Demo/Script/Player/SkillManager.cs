using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private float _damage = 100f;

    CapsuleCollider _collider;

    public Action<float> HitAction;

    [SerializeField]
    private float _energy = 1.5f;
    private async void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        Destroy(transform.root.gameObject);
    }

    private void Update()
    {
        if(_collider.height < 14)
        {
            ChangeColliderHeight(_collider);
        }

        if(_collider.center.z < 7)
        {
            ChangeColliderCenter(_collider);
        }
       
    
    }


    void ChangeColliderHeight(CapsuleCollider capsuleCollider)
    {
        capsuleCollider.height += 0.1f;
        
    }
    void ChangeColliderCenter(CapsuleCollider capsuleCollider)
    {
        capsuleCollider.center += new Vector3(0, 0, 0.05f);
    }



    private void OnTriggerEnter(Collider other)
    {
        if ( other.transform.parent != null && other.transform.parent.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Enemy")
        {
            if (damage.CurrentHealth > 0)
            {
                damage.HitDamage(_damage);
                HitAction?.Invoke(_energy);
            }
               
              
             
        }
    }
}
