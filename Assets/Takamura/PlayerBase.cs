using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{

    protected Rigidbody _rb;
    [SerializeField]
    private float _speed = 1;
    public float Speed { get { return _speed; } set { _speed = value; } }
    Animator _anim;
    private bool _canMove = true;
    private MotionIndex _motionIndex;

    public MotionIndex State { get { return _motionIndex; } set { _motionIndex = value; } }

    [SerializeField]
    private float _length;

    private Vector3 _lastPosition;

    private bool _isGround;
    public bool IsGround { get => _isGround; set { _isGround = value; } }

    public bool WallFlg = false;

    [SerializeField]
    private float _skatingSpeed;

    Vector3 _footPosition;

    [SerializeField]
    private float _wallRunSpeed;

    CapsuleCollider _capsuleCollider;

    private void Awake()
    {

        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _rb.useGravity = false;
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Update()
    {
        AnimationManagement();
        GroundCheck();
        WallCheck();
    }

    protected virtual void FixedUpdate()
    {
        if (State != MotionIndex.Skating)
        {
            Moving();
        }
        else
        {
            SkatingMove();
            WallRun();
        }

        AddGravity();
    }

    void AddGravity()
    {
        if (State != MotionIndex.Skating)
        {
            _rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
        else
        {
            if (IsGround)
                _rb.AddForce(_footPosition - transform.position, ForceMode.Acceleration);
        }

    }

    void GroundCheck()
    {
        IsGround = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down).normalized * _length, out RaycastHit hit, 1.3f);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down).normalized * _length, Color.magenta);
        _footPosition = hit.point;

    }

    void Moving()
    {
        if (IsGround && _canMove)
        {
            var velo = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            _rb.linearVelocity = velo * _speed;

            if (velo.magnitude > 0)
            {
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velo), 10f);
                transform.rotation = rot;
            }


        }


    }
    void AnimationManagement()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Skil") || stateInfo.IsName("Ult") || stateInfo.IsName("Attack")
            || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3"))
        {
            _canMove = false;
            _rb.linearVelocity = Vector3.zero;

        }
        else
        {
            _canMove = true;
        }

        if (State == MotionIndex.Skating)
        {
            return;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && IsGround)
        {
            AnimSet(0);
            State = MotionIndex.Idol;
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

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && IsGround)
        {
            AnimSet(20);
            State = MotionIndex.Run;
        }

        _anim.SetFloat("Blend", _rb.linearVelocity.magnitude);

        if (Input.GetMouseButton(0))
        {
            AttackAnim();
        }
    }

    public void StateChange(MotionIndex motion)
    {
        State = motion;
        _anim.SetInteger("MotionIndex", (int)motion);
    }

    public void AttackAnim()
    {
        _anim.SetTrigger("Attack");
    }

    public void SkateIn()
    {
    }

    public void WallCheck()
    {
        WallFlg = Physics.Raycast(transform.position, transform.forward.normalized * 0.5F, out RaycastHit hit, 0.6f);
        Debug.DrawRay(transform.position, transform.forward.normalized * 0.5f, Color.red);
    }

    void SkatingMove()
    {
        if (IsGround && _canMove)
        {
            var velo = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            _rb.linearVelocity = velo * _skatingSpeed;

            if (velo.magnitude > 0)
            {
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velo), 10f);
                transform.rotation = rot;
            }
        }
        if (WallFlg && IsGround)
        {
            transform.position += new Vector3(0, 1, 0);
            Debug.Log("Hop");
        }

    }

    public void WallRun()
    {
        if (!IsGround)
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            if (move == Vector3.zero)
            {
                return;
            }
            
            var bottum = (gameObject.transform.GetChild(0).transform.position) + _capsuleCollider.center - Vector3.up *( _capsuleCollider.height / 2 - _capsuleCollider.radius);
        }
    }


    public void Jumping()
    {

    }

    public void AnimSet(int motion)
    {
        _anim.SetInteger("MotionIndex", motion);
    }

    public enum MotionIndex
    {
        Idol = 0, Run = 20, Avoid = 30, Skating = 40
    }
}
