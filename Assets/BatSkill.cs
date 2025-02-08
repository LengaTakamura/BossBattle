using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;

public class BatSkill : StateMachineBehaviour
{
    [SerializeField]
    GameObject _batPrefab;

    CancellationTokenSource cts = null;
    [SerializeField]
    float _awaitTime = 0.5f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cts = new CancellationTokenSource();
        UltTiming(cts.Token, animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cts.Cancel();
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

    async void UltTiming(CancellationToken ct, Animator anim)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_awaitTime), cancellationToken: ct);
        var bat = Instantiate(_batPrefab, anim.transform.position + anim.transform.forward, anim.transform.rotation);
        bat.transform.SetParent(anim.transform);

    }
}
