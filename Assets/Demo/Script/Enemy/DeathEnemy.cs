using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DeathEnemy : StateMachineBehaviour
{
    CancellationTokenSource _cts;
    [SerializeField]
    private float _awaitTime = 3.3f;    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _cts = new CancellationTokenSource();
        DeathCallback(_cts.Token,animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //
    // }

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

    private async void DeathCallback(CancellationToken token,Animator animator)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_awaitTime), cancellationToken: token);
            animator.speed = 0;
            GameManager.Instance.GameEnded();
        }
        catch 
        {
           
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
        }
    }
}
