using UnityEngine;

public class PlayerMash : PlayerBase
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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

    public void AttackAnim()
    {
        _anim.SetTrigger("Attack");
    }
}
