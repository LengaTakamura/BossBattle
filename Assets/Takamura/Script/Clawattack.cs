using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;

public class Clawattack : StateMachineBehaviour
{
    Transform _parent;
    [SerializeField]
    float _awaitTime;
    [SerializeField]
    float _derayTime;
    CancellationTokenSource _cts;
    Transform _target;
    [SerializeField]
    float _distance = 5;
    [SerializeField]
    float _damageMag = 5;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _cts = new CancellationTokenSource();
        _parent = animator.transform.root;
        FrameAttackTiming(_cts.Token, stateInfo);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _cts.Cancel();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

 
    async void FrameAttackTiming(CancellationToken ct, AnimatorStateInfo info)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_awaitTime), cancellationToken: ct);
        _target = GameObject.FindAnyObjectByType<PlayerBase>().GetComponent<Transform>();
        if(Vector3.Distance(_target.position,_parent.position) < _distance)
        {
            var player = _target.GetComponent<IDamageable>();
            var enemy = _parent.GetComponent<IDamageable>();
            player.HitDamage(enemy.AttackPower * _damageMag);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_derayTime), cancellationToken: ct);
        _target = GameObject.FindAnyObjectByType<PlayerBase>().GetComponent<Transform>();
        if (Vector3.Distance(_target.position, _parent.position) < _distance)
        {
            var player = _target.GetComponent<IDamageable>();
            var enemy = _parent.GetComponent<IDamageable>();
            player.HitDamage(enemy.AttackPower * _damageMag);
        }
    }
}
