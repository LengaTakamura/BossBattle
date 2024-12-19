using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDemo : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField]
    int _speed = 0 ;
    public MotionIndex motionIndex;
    Animator _anim;
    public float _waitTime = 0;
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
        _rb.linearVelocity = new Vector3(Input.GetAxis("Horizontal") * _speed , 0, Input.GetAxis("Vertical") * _speed);
   
    }

    void Rotating()
    {
        var movement = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);

        if (movement.magnitude != 0f)
        {
            var rota = Quaternion.LookRotation(movement, Vector3.up);
            var diffAngle = Vector3.Angle(_transform.forward, movement);
            var targetAngle = Mathf.SmoothDampAngle(0, diffAngle, ref _currentVelocity, _smoothTime, _maxSpeed);
            var nextRot = Quaternion.RotateTowards(_transform.rotation, rota, targetAngle);
            _transform.rotation = nextRot;
        }
    }

    bool GroundCheck()
    {
        bool isGround = false;
        return isGround;
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

        if (_rb.linearVelocity.magnitude > 0.05f)
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
            _speed = _defalutSpeed;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            _anim.SetTrigger("Skil");
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            _anim.SetTrigger("Ult");
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && _waitTime >= 3f)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Run);
            motionIndex = MotionIndex.Run;
            _speed = _dashSpeed;
        }

        if(_rb.linearVelocity == Vector3.zero)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Idol);
            motionIndex = MotionIndex.Idol;
        }

    }

    public void ChangeMotionIndex(MotionIndex mo)
    {
        motionIndex = mo;
    }
    public enum MotionIndex
    {
        Idol = 0, Walk = 10, Run = 20, Avoid = 30
    }
}
