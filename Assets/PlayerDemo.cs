
using UnityEngine;


public class PlayerDemo : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    int _speed = 0 ;
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
        
    }

    void Moving()
    {
        _rb.velocity = new Vector3(Input.GetAxis("Horizontal") * _speed , 0, Input.GetAxis("Vertical") * _speed);
   
    }

    void Rotating()
    {
        //var position = _transform.position;
        //var movement = position - _lastPosition;
        //_lastPosition = position;
        var movement = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

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
        }
        else
        {
            _canMove = true;
        }
 
        if (!Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            _speed = _defalutSpeed;
            AnimSet(0);
            _motionIndex = MotionIndex.Idol;
        }
        else
        {
            AnimSet(10);
            _motionIndex =MotionIndex.Walk;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            _anim.SetTrigger("Skil");
           
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            _anim.SetTrigger("Ult");
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))&& _rb.velocity.magnitude >= 5)
        {
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
    {
        Idol = 0,Walk = 10,Run = 20, Avoid = 30
    }
}
