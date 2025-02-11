using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HealSkill : StateMachineBehaviour
{
    CancellationTokenSource _cts;
    List<IDamageable> _players = new();
    int _healCount = 5;
    [SerializeField]
    float _healValue = 100;
    [SerializeField]
    float _healDelay = 1f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public async void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _healCount = 5;
        _cts = new CancellationTokenSource();
        var parent = animator.transform.root;
        foreach(Transform player in parent)
        {
            if (player.TryGetComponent(out IDamageable damage))
            {
                _players.Add(damage);
            }
            
        }
        try
        {
            while (_healCount > 0)
            {
                foreach(IDamageable player in _players)
                {
                    player.HitHeal(_healValue);
                    Debug.Log(player.CurrentHealth);
                }
                _healCount--;
                await UniTask.Delay(TimeSpan.FromSeconds(_healDelay));
            }
        }
        catch { }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
}
