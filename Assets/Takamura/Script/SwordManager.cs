using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public float Damage = 100;
    [SerializeField]
    private GameObject _model;

    private Animator _animator;

    AnimatorClipInfo[] _clipInfo;
    private void Start()
    {
        _animator = _model.GetComponent<Animator>();
        _clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
    }


    private void Update()
    {
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out IDamageable damage) && other.transform.parent.gameObject.tag == "Enemy")
        {
            damage.HitDamage(Damage);
        }
    }

  

}
