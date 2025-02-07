using R3;
using UnityEngine;

public class AimMoving : StateMachineBehaviour
{
    Rigidbody _parent;
    private Subject<Vector3> _onStateUpdate = new Subject<Vector3>();
    public Observable<Vector3> OnStateUpdateObservable {  get { return _onStateUpdate; } }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _parent = animator.transform.parent.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        animator.rootPosition = _parent.transform.position - Vector3.up;
        _onStateUpdate.OnNext(animator.rootPosition);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _onStateUpdate?.OnCompleted();
        _onStateUpdate?.Dispose();
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
}
