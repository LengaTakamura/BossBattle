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
        if (other.transform.parent.TryGetComponent(out IDamageable damage))
        {
            damage.HitDamage(Damage);
        }
    }

    async void AwaitAnim(IDamageable damage)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.25));

    }

}
