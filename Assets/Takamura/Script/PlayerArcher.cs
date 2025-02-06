using R3;
using UnityEngine;

public class PlayerArcher : PlayerBase
{
    bool _isAiming = false;
    private void Start()
    {
        Aiming();
    }

    protected override void Update()
    {
        base.Update();
       
        Attack();
    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();

    }

    private void Aiming()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        var attack = Observable.EveryUpdate()
           .Where(_ => Input.GetKeyDown(KeyCode.R))
           .Where(_ => gameObject.activeSelf)
           .Where(_ => !stateInfo.IsName("AimStart"))
           .Scan(0, (count, _) => count + 1)
           .Subscribe(count =>
           {
               if (count % 2 != 0)
               {
                   _isAiming = true;

               }
               else
               {
                   _isAiming = false;
               }
               _anim.SetBool("Aim", _isAiming);
           });

    }

    private void Attack()
    {
        if (_isAiming && Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
        }
    }


}
