
using UnityEngine;

public class PlayerHuman : PlayerBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && !OnPause)
        {
            AttackAnim();
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
