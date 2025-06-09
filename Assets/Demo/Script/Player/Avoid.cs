using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Avoid : StateMachineBehaviour
{
    PlayerBase _player;
    CancellationTokenSource _cts;
    CancellationTokenSource _cts2;
    [SerializeField]
    float _avoidDelay = 0.3f;
    [SerializeField]
    float _avoidTime = 0.1f;

    public Action OnExit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override async public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _cts2 = new CancellationTokenSource();
        _player = animator.transform.parent.GetComponent<PlayerBase>();
        await UniTask.Delay(TimeSpan.FromSeconds(_avoidDelay),cancellationToken:_cts2.Token);
        _player.State = PlayerBase.MotionIndex.Avoid;
        await UniTask.Delay(TimeSpan.FromSeconds(_avoidTime), cancellationToken: _cts2.Token);
        _player.State = PlayerBase.MotionIndex.NonAvoid;
    }


   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.gameObject.transform.position = animator.rootPosition + Vector3.up;
    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override  public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.ReduceStamina();
        PlayerBase.OnStaminaChanged?.Invoke(PlayerBase.CurrentStamina / _player.MaxStamina);
    }

   
}
