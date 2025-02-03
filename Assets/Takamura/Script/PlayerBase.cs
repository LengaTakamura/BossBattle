
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour, IDamageable
{

    protected Rigidbody _rb;
    [SerializeField]
    private float _speed = 1;
    public float Speed { get { return _speed; } set { _speed = value; } }
    public Animator _anim;
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

    [SerializeField]
    float _maxHealth = 100;

    float IDamageable.MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    [SerializeField]
    private float _currentHealth = 100;
    float IDamageable.CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    Vector3 _cameraF = Vector3.zero;
    Vector3 _cameraR = Vector3.zero;
    float _gapSum = 0;
    Vector3 _normal = Vector3.zero;
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
        if (State == MotionIndex.Skating)
        {

            WallRun(WallCheck());
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

        if (Input.GetMouseButtonDown(0))
        {
            AttackAnim();
        }

        if (!Input.GetMouseButton(0))
        {
            _anim.SetBool("AttackBool", false);
        }
    }

    public void StateChange(MotionIndex motion)
    {
        State = motion;
        _anim.SetInteger("MotionIndex", (int)motion);
    }

    public void AttackAnim()
    {
        _anim.SetBool("AttackBool", true);
        _anim.SetTrigger("Attack");
    }

    void IDamageable.HitDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth < 0)
        {
            Debug.Log($"Ž€–S{gameObject}");
        }
    }


    public void SkateIn()
    {
    }

    public Collider[] WallCheck()
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

        return hits;

    }

    void SkatingMove()
    {
        if (IsGround && _canMove && !WallFlg)
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

    public void WallRun(Collider[] hits)
    {

        if (WallFlg)
        {
            float gap = 0;
            _normal = Vector3.zero;
            _rb.isKinematic = true;
            var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0).normalized;

            if (input == Vector3.zero)
            {
                return;
            }
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
            {
                _cameraF = _camera.transform.forward;
                _cameraF.y = 0;
                _cameraF.Normalize();
                _cameraR = _camera.transform.right;
                _cameraR.y = 0;
                _cameraR.Normalize();

            }

            Vector3 moveDirectionX = Vector3.zero;
            Vector3 moveDirectionY = Vector3.zero;
            moveDirectionY = new Vector3(0,input.y,0);
            foreach (var hit in hits)
            {
                var point = transform.position - hit.ClosestPoint(transform.position);
                _normal += point;

            }
            if (hits.Length > 0)
            {
                _normal /= hits.Length;
                if (_normal.magnitude > 0.8f)
                {
                     gap = _normal.magnitude - (_capsuleCollider.radius + _radiusOffset);
                }
                _normal.Normalize();
               
                var onplaneX = Vector3.Cross(_normal, Vector3.up);
                var onplaneY = Vector3.Cross(onplaneX,_normal );
                Vector3 m = Vector3.zero;
                if (Vector3.Dot(_cameraR,onplaneX)< 0)
                {
                    m = -(onplaneX * input.x) + onplaneY * moveDirectionY.y;
                }
                else
                {
                    m = onplaneX * input.x + onplaneY * moveDirectionY.y;
                }
                m.Normalize();
                transform.position += (gap * _normal.normalized + m) * _wallRunSpeed * Time.deltaTime;
                var foot = transform.GetChild(0);
                var rot = Quaternion.RotateTowards(foot.transform.rotation, Quaternion.LookRotation(m), 10f);
                rot.x = 0;
                rot.z = 0;
                foot.transform.rotation = rot;

            }

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
