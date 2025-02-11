using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using Unity.Cinemachine;
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
    protected CinemachineCamera _camera;

    [SerializeField]
    float _radiusOffset = 0.2f;

    [SerializeField]
    float _maxHealth = 100;

    float IDamageable.MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    [SerializeField]
    private float _currentHealth = 100;
    float IDamageable.CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    [SerializeField]
    protected float _attackPower = 100;
    float IDamageable.AttackPower { get { return _attackPower; } set { _attackPower = value; } }

    Vector3 _cameraF = Vector3.zero;
    Vector3 _cameraR = Vector3.zero;
    float _gapSum = 0;
    Vector3 _normal = Vector3.zero;

    private Subject<float> _onDamage = new Subject<float>();
    public float MaxStamina = 3f;
    public static float CurrentStamina = 3f;
    public Observable<float> Ondamaged
    {
        get { return _onDamage; }
    }

    public static Action<float> OnStaminaChanged;

    public Action AttackAction;

    [SerializeField]
    float _coolTime = 15f;

    private bool _isCoolDown = false;

    CancellationTokenSource _cts;

    public float CoolDownTime = 0;

    public Action<PlayerBase> OnCoolDownChanged;

    public Action<int> DeathAction;

    public int CharaIndex = 0;

    public float UltEnergy = 10f;

    private float _currentEnergy = 0f;

    public static bool OnPause = false;

    [SerializeField]
    private float _defense = 10;

    public float Defense { get { return _defense; } set => _defense = value; }

    public float CurrentEnergy
    {
        get
        { 
            return _currentEnergy;
            
        }
        set { _currentEnergy = value; }
    }

    public Action<PlayerBase> OnEnergyChanged;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _rb.useGravity = false;
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _cts = new CancellationTokenSource();
        CurrentEnergy = 0f;
       
    }

    protected virtual void Start()
    {       
        InitOnDamage();
        CoolDownTime = 0f;
        CurrentStamina = MaxStamina;
    }

    private void InitOnDamage()
    {
        _onDamage.Subscribe(damage =>
        {
            if (_currentHealth <= 0)
            {
                Debug.Log($"Ž€–S{gameObject}");
                _anim.SetTrigger("Death");
            }
        }
        ).AddTo(this);
    }

   
       

    private async UniTask UseSkill(CancellationToken token)
    {
        try
        {
            _isCoolDown = true;
            CoolDownTime = _coolTime;
            _anim.SetTrigger("Skil");
            while(CoolDownTime > 0)
            {
                await UniTask.Delay(100, cancellationToken: token);
                CoolDownTime -= 0.1f;
                OnCoolDownChanged?.Invoke(this);
            }
            CoolDownTime = 0;
            _isCoolDown = false;
            OnCoolDownChanged?.Invoke(this);
        }
        catch { }
       

    }

    protected virtual void Update()
    {

       
        GroundCheck();

        if (!OnPause)
        {
            AnimationManagement();
            if (State == MotionIndex.Skating)
            {

                WallRun(WallCheck());
            }

            if (Input.GetKeyDown(KeyCode.E) && !_isCoolDown && gameObject.activeSelf)
            {
                UseSkill(_cts.Token).Forget();
            }
        }
        

    }

    protected virtual void FixedUpdate()
    {
        if (!OnPause)
        {
            if (State != MotionIndex.Skating)
            {
                Moving();
            }
            else
            {
                SkatingMove();

            }
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

        if (stateInfo.IsTag("CanMove"))
        {
          
            _canMove = true;

        }
        else
        {
            _canMove = false;
            _rb.linearVelocity = Vector3.zero;

        }

        if (State == MotionIndex.Skating)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Q) && CanUseUlt())
        {
            _anim.SetTrigger("Ult");
            CurrentEnergy = 0;
        }
  
        _anim.SetFloat("Blend", _rb.linearVelocity.magnitude);
    }

    public void StateChange(MotionIndex motion)
    {
        State = motion;
        _anim.SetInteger("MotionIndex", (int)motion);
    }

    
    void IDamageable.HitDamage(float damage)
    {
        if(State != MotionIndex.Avoid)
        {
            damage = damage - Defense;
            if(damage < 0)
            {
                return ;
            }
            _currentHealth -= (damage);
            _onDamage.OnNext(damage);

        }

    }

    void IDamageable.HitHeal(float value)
    {
        _currentHealth += value;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        } 
        _onDamage.OnNext(value);
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

    bool CanUseUlt()
    {
        if(CurrentEnergy >= UltEnergy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

     public void AddEnergy(float energy)
     {
        CurrentEnergy += energy;
        OnEnergyChanged?.Invoke(this);
    }

    public void RecoveryStamina()
    {
        if (CurrentStamina < MaxStamina && State != MotionIndex.Avoid)
            CurrentStamina += 1;
        OnStaminaChanged?.Invoke(CurrentStamina / MaxStamina);
    }

    public float ReduceStamina()
    {
        CurrentStamina -= 1;
        OnStaminaChanged?.Invoke(CurrentStamina / MaxStamina);
        if (CurrentStamina < 0)
        {
            CurrentStamina = 0;
            return 0;
        }
        
        return CurrentStamina / MaxStamina;
    }


    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public enum MotionIndex
    {
       NonAvoid = 0,Avoid = 10,Skating = 30
    }

  
}
