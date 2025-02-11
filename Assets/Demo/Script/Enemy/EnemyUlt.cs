using UnityEngine;

public class EnemyUlt : StateMachineBehaviour
{

    [SerializeField]
    GameObject _flameErea;
    [SerializeField]
    int _ereaValue = 10;
    GameObject _ground;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ground = GameObject.Find("Ground");
       
        for(int i = 0;i < _ereaValue; i++)
        {
            var randomX = Random.Range(-25f,25f);
            var randomZ = Random.Range(-25f,25f);
            var erea = Instantiate(_flameErea, new Vector3(_ground.transform.position.x + randomX, animator.transform.position.y, _ground.transform.position.z + randomZ), Quaternion.identity);
        }
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
