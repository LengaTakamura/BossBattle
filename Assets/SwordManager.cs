using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public int Damage = 100;
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

   

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damage))
        {

            var name = _clipInfo[0].clip.name;

            if (name == "Attack")
            {
                damage.HitDamage(Damage);
                Debug.Log("Attack1");

            }
            else if (name == "Attack2")
            {
                damage.HitDamage(Damage * 2);
                Debug.Log("Attack2");
            }
            else if (name == "Attack3")
            {
                damage.HitDamage(Damage * 3);
                Debug.Log("Attack3");
            }
        }
    }

    async void AwaitAnim(IDamageable damage)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.25));

    }

}
