
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private float _maxRayRange;

    [SerializeField]
    CinemachineCamera _camera;

    [SerializeField]
    float _radiusOffset = 0.2f;

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
        if(State == MotionIndex.Skating)
        {
            WallRun();
        }
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
            Vector3 cameraForward = Camera.main.transform.forward;          
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0; 
            cameraRight.Normalize();
            Vector3 moveDirection = cameraForward * velo.z + cameraRight * velo.x;
            _rb.linearVelocity = moveDirection * _speed;

            if (velo.magnitude > 0)
            {
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), 10f);
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
        //WallFlg = Physics.Raycast(transform.position, transform.forward.normalized * 0.5F, out RaycastHit hit, _maxRayRange,_layerMask)
        //|| Physics.Raycast(transform.position, transform.right.normalized * 0.5F, out RaycastHit hit2, _maxRayRange,_layerMask)
        // || Physics.Raycast(transform.position, -transform.right.normalized * 0.5F, out RaycastHit hit3, _maxRayRange, _layerMask)
        //  || Physics.Raycast(transform.position, -transform.forward.normalized * 0.5F, out RaycastHit hit4, _maxRayRange, _layerMask);
        //Debug.DrawRay(transform.position, transform.forward.normalized * 0.5f, Color.red);
        //Debug.DrawRay(transform.position, transform.right.normalized * 0.5F, Color.red);
        //Debug.DrawRay(transform.position, -transform.right.normalized * 0.5F, Color.red);
        //Debug.DrawRay(transform.position, -transform.forward.normalized * 0.5F, Color.red);

      
    }

    void SkatingMove()
    {
        if (IsGround && _canMove &&!WallFlg)
        {
            var velo = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            Vector3 moveDirection = cameraForward * velo.z + cameraRight * velo.x;
            _rb.linearVelocity = moveDirection * _skatingSpeed;

            if (velo.magnitude > 0)
            {
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), 10f);
                transform.rotation = rot;
            }
        }

    }

    public void WallRun()
    {

        var bottum = transform.position - Vector3.up * (_capsuleCollider.height / 2 + _capsuleCollider.radius) + _capsuleCollider.center;
        var top = transform.position + Vector3.up * (_capsuleCollider.height / 2 - _capsuleCollider.radius) + _capsuleCollider.center;
        var hits = Physics.OverlapCapsule(bottum, top, _capsuleCollider.radius + _radiusOffset, _layerMask);
        if (hits.Length == 0)
        {
            WallFlg = false;
        }
        else
        {
            WallFlg = true;
        }


        if (WallFlg)
        {
            Vector3 normal = Vector3.zero;
            float gapSum = 0;
            _rb.isKinematic = true;
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0).normalized;
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            Vector3 moveDirection = Vector3.zero;

            if (move == Vector3.zero)
            {
                return;
            }

            if (Vector3.Dot((cameraRight * move.x) + (cameraRight * move.z), move) < 0)
            {
                moveDirection = -(cameraRight * move.x + cameraRight * move.z) + move;
            }
            else
            {
                moveDirection = (cameraRight * move.x + cameraRight * move.z) + move;
            }



            foreach (var hit in hits)
            {
                var point = hit.ClosestPoint(transform.position) - transform.position;
                if(point.magnitude < 0.4)
                {
                    var gap = _capsuleCollider.radius - point.magnitude;
                    gapSum += gap;
                }
                normal += point;
                
            }
            normal.Normalize();
            if (hits.Length > 0)
            {
                normal /= hits.Length;
                var onplane = Vector3.ProjectOnPlane(moveDirection, normal).normalized;
                transform.position += -gapSum * normal.normalized + onplane * _wallRunSpeed * Time.deltaTime;
                var foot = transform.GetChild(0);
                var rot = Quaternion.RotateTowards(foot.transform.rotation, Quaternion.LookRotation(onplane), 10f);
                Debug.Log(rot);
                rot.z = 0;
                foot.transform.rotation = rot;

            }
           
        }
        else
        {
            if (!IsGround)
            {
                _radiusOffset += 0.1f;            }
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
