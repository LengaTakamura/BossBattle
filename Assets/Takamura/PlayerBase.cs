using Unity.Burst.CompilerServices;
using UnityEngine;
using R3;
using R3.Collections;

public abstract class PlayerBase : MonoBehaviour
{

    protected Rigidbody _rb;
    [SerializeField]
    private float _speed = 1;
    public float Speed { get { return _speed; } set { _speed = value; } }
    Animator _anim;
    private Vector3 _pos;
    private float _currentVelocity = 0;
    private float _smoothTime = 0;
    [SerializeField]
    private float _maxSpeed = 360f;
    private bool _canMove = true;
    [SerializeField]
    private int _dashSpeed = 10;
    [SerializeField]
    private int _defalutSpeed = 5;

    private MotionIndex _motionIndex;

    public MotionIndex State { get { return _motionIndex; } set { _motionIndex = value; } }

    [SerializeField]
    private float _length;

    private Vector3 _lastPosition;

    RaycastHit _hit;

    private bool _isGround;
    public bool IsGround { get => _isGround  ; set { _isGround = value; } }

    public bool CanSkating = false;
    private void Awake()
    {
        
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _rb.useGravity = false;
       
    }

   protected virtual void Update()
    {
        AnimationManagement();
    }

    protected virtual void FixedUpdate()
    {
       
        if (_canMove)
        {         
            Moving();
        }
        _rb.AddForce(Physics.gravity, ForceMode.Acceleration);

     }
    
    void Moving()
    {
         IsGround = Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down).normalized * _length, out _hit,1.3f);
        Debug.DrawRay(transform.position ,transform.TransformDirection(Vector3.down).normalized * _length, Color.magenta);
        if (IsGround && State != MotionIndex.Skate)
        {
            var velo = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            _rb.linearVelocity = velo * _speed;

            velo.y = 0;

            if (velo.magnitude > 0 && State != MotionIndex.Skate)
            {
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velo), 10f);
                transform.rotation = rot;
            }

            
        }

    }
    void AnimationManagement()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Skil") || stateInfo.IsName("Ult"))
        {
            _canMove = false;
            _rb.linearVelocity = Vector3.zero;

        }
        else
        {
            _canMove = true;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && IsGround)
        {
            _speed = _defalutSpeed;
            AnimSet(0);
            State = MotionIndex.Idol;
        }


        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && IsGround)
        {
            AnimSet(10);
            State = MotionIndex.Walk;
        }

        if (Input.GetKeyUp(KeyCode.E) && _canMove)
        {
            _anim.SetTrigger("Skil");

        }

        if (Input.GetKeyUp(KeyCode.Q) && _canMove)
        {
            _anim.SetTrigger("Ult");
        }
        Vector3 vect = transform.position - _lastPosition;
        _lastPosition = transform.position;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && (Mathf.Abs(vect.z) >= 0.03f || Mathf.Abs(vect.x) >= 0.03f) && IsGround)
        {
            AnimSet(20);
            State = MotionIndex.Run;
            _speed = _dashSpeed;
        }

        _anim.SetFloat("Blend", _rb.linearVelocity.magnitude);
    }


    public void StateChange(MotionIndex motion)
    {
        State = motion;
    }


    public void AnimSet(int motion)
    {
        _anim.SetInteger("MotionIndex", motion);
    }

    public enum MotionIndex
    {
        Idol = 0, Walk = 10, Run = 20, Avoid = 30, Skate = 40
    }
}
