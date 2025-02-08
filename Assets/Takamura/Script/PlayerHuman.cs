using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class PlayerHuman : PlayerBase
{
    private void Start()
    {


    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            AttackAnim();
            AttackAction?.Invoke();
        }
    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();

    }


    public void AttackAnim()
    {
        _anim.SetTrigger("Attack");
    }

}
