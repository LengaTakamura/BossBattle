using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;

public class FrameAttack : StateMachineBehaviour
{

    Transform _jaw3;
    [SerializeField]
    GameObject _frame;
    [SerializeField]
    float _awaitTime;
    CancellationTokenSource _cts;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_jaw3)
        {
            _jaw3 = FindChildRecursive(animator.transform, "Jaw3");
            
        }
        _cts = new CancellationTokenSource();
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

    Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform result = FindChildRecursive(child, childName);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    async void FrameAttackTiming(CancellationToken ct, AnimatorStateInfo info)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_awaitTime), cancellationToken: ct);
        var frame = Instantiate(_frame, _jaw3.position, _jaw3.transform.rotation,_jaw3);
        
    }
}
