
using UnityEngine;


public class PlayerDemo : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    int _speed = 0 ;
<<<<<<< HEAD
    public MotionIndex motionIndex;
=======
>>>>>>> origin/sub
    Animator _anim;
    private Vector3 _pos;
    private Transform _transform;
    private float _currentVelocity = 0;
    public float _smoothTime = 0; 
    public float _maxSpeed = 360f;
    public bool _canMove = true;
    [SerializeField]
    private int _dashSpeed = 10;
    [SerializeField]
    private int _defalutSpeed = 5;

    private MotionIndex _motionIndex;

    public MotionIndex Anim { get { return _motionIndex; } set { _motionIndex = value; } }


    void Awake()
    {
        _transform = transform;
        _anim = GetComponent<Animator>();
        _rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationManagement();
        if (_canMove)
        {
            Moving();
            Rotating();
        }
<<<<<<< HEAD

=======
        
>>>>>>> origin/sub
    }

    void Moving()
    {
        _rb.linearVelocity = new Vector3(Input.GetAxis("Horizontal") * _speed , 0, Input.GetAxis("Vertical") * _speed);
   
    }

    void Rotating()
    {
<<<<<<< HEAD
        var movement = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
=======
        //var position = _transform.position;
        //var movement = position - _lastPosition;
        //_lastPosition = position;
        var movement = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
>>>>>>> origin/sub

        if (movement.magnitude != 0f)
        {
            var rota = Quaternion.LookRotation(movement, Vector3.up);
            var diffAngle = Vector3.Angle(_transform.forward, movement);
            var targetAngle = Mathf.SmoothDampAngle(0, diffAngle, ref _currentVelocity, _smoothTime, _maxSpeed);
            var nextRot = Quaternion.RotateTowards(_transform.rotation, rota, targetAngle);
            _transform.rotation = nextRot;
        }
    }

    void AnimationManagement()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Skil") || stateInfo.IsName("Ult"))
        {
            _canMove = false;
            _rb.linearVelocity = Vector3.zero;
            
        }
        else
        {
            _canMove = true;
        }
<<<<<<< HEAD

        if (_rb.velocity.magnitude > 0.05f)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
            motionIndex = MotionIndex.Walk;
        }

        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D) && motionIndex == MotionIndex.Walk)
        {
            _waitTime += Time.deltaTime;
        }
        else if (!Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            _waitTime = 0f;
=======
 
        if (!Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
>>>>>>> origin/sub
            _speed = _defalutSpeed;
            AnimSet(0);
            _motionIndex = MotionIndex.Idol;
        }
        else
        {
            AnimSet(10);
            _motionIndex =MotionIndex.Walk;
        }

        if (Input.GetKeyUp(KeyCode.E)&& _canMove)
        {
            _anim.SetTrigger("Skil");
           
        }

        if (Input.GetKeyUp(KeyCode.Q) && _canMove)
        {
            _anim.SetTrigger("Ult");
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))&& _rb.linearVelocity.magnitude >= 5)
        {
<<<<<<< HEAD
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Run);
            motionIndex = MotionIndex.Run;
            _speed = _dashSpeed;
        }

        if(_rb.velocity == Vector3.zero)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Idol);
            motionIndex = MotionIndex.Idol;
        }

    }

    public void ChangeMotionIndex(MotionIndex mo)
=======
            AnimSet(20);
            _motionIndex =MotionIndex.Run;  
            _speed = _dashSpeed;
        }
        

    }


    public void AnimSet(int motion)
    {
        _anim.SetInteger("MotionIndex", motion);
    }

    public  enum MotionIndex
>>>>>>> origin/sub
    {
        motionIndex = mo;
    }
    public enum MotionIndex
    {
        Idol = 0, Walk = 10, Run = 20, Avoid = 30
    }
}
