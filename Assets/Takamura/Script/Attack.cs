using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Attack : StateMachineBehaviour
{

    Collider _sword;
    [SerializeField]
    float _awaitTime = 0.2f;
    [SerializeField]
    float _delayTime = 0.5f;
    CancellationTokenSource cts = null;
    [SerializeField]
    float _damageMag = 1;
    [SerializeField]
    GameObject _slashObj;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_sword)
        {
            Transform obj =FindChildRecursive(animator.transform,"Sword");     
            _sword = obj.GetComponent<Collider>();
        }
        cts = new CancellationTokenSource();
        AttackTiming(cts.Token,stateInfo);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cts.Cancel();
        cts.Dispose();
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

    async void AttackTiming(CancellationToken ct,AnimatorStateInfo info)
    {
        var sword = _sword.GetComponent<SwordManager>();
        sword.DamageMag = _damageMag;
        await UniTask.Delay(TimeSpan.FromSeconds(_awaitTime), cancellationToken: ct);
        _sword.enabled = true;
        if(!info.IsName("Attack"))
        {
            var inst = Instantiate(_slashObj, _sword.transform.position, Quaternion.identity);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_delayTime), cancellationToken: ct);
        _sword.enabled = false;
    }
}
