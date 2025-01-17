
using System;
using UnityEngine;
using UnityEngine.Android;


public class PlayerDemo : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    int _speed = 0 ;
    Animator _anim;
    private Vector3 _pos;
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

    [SerializeField]
    private Vector3 _offSet ;

    private Vector3 _lastPosition ;
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        AnimationManagement();
       
        
        
    }

    private void FixedUpdate()
    {
       
        if (_canMove)
        {
            Rotating();
            Down();
        }
    }

    void Down()
    {

        bool raycastHit = Physics.Raycast(transform.position + new Vector3(0,1,0), _offSet * -1, out RaycastHit hit,1.2f);
        
        if (raycastHit)
        {
            var angle = Vector3.Angle(hit.normal, Vector3.up);
            Moving();

        }
        else
        {
            
        }
    }

    void Moving()
    {
        var velo = new Vector3(Input.GetAxis("Horizontal") , 0, Input.GetAxis("Vertical")).normalized;

        _rb.linearVelocity = velo * _speed;

        velo.y = -1;

        if (Mathf.Abs(velo.x) > 0 && Mathf.Abs(velo.z) > 0)
        {
            var rot = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(velo),0.1f);
            rot.x = 0;
            rot.z = 0;
            transform.rotation = rot;
        }

    }

    void Rotating()
    {
        var velo = _rb.linearVelocity;
        
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

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            _speed = _defalutSpeed;
            AnimSet(0);
            _motionIndex = MotionIndex.Idol;
        }
        else
        {
            AnimSet(10);
            _motionIndex = MotionIndex.Walk;
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

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && (Mathf.Abs(vect.z) >= 0.03f || Mathf.Abs(vect.x) >= 0.03f))
        {
            AnimSet(20);
            _motionIndex = MotionIndex.Run;
            _speed = _dashSpeed;
        }

        _anim.SetFloat("Blend", _rb.linearVelocity.magnitude);
        
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
