using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDemo : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    int speed = 0 ;
    MotionIndex motionIndex;
    Animator _anim;
    public float waitTime = 0;
    private Vector3 pos;
    private Transform _transform;
    private float _currentVelocity = 0;
    public float _smoothTime = 0; 
    public float _maxSpeed = 360f;
    bool _canMove = true;
    
    void Awake()
    {
        _transform = transform;
        
        
        _anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody>();
        _anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
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
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed , 0, Input.GetAxis("Vertical") * speed);
   
    }

    void Rotating()
    {
        //var position = _transform.position;
        //var movement = position - _lastPosition;
        //_lastPosition = position;
        var movement = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (movement.magnitude != 0f)
        {
            Debug.Log("kaiten");
            var rota = Quaternion.LookRotation(movement, Vector3.up);
            var diffAngle = Vector3.Angle(_transform.forward, movement);
            var targetAngle = Mathf.SmoothDampAngle(0, diffAngle, ref _currentVelocity, _smoothTime, _maxSpeed);
            var nextRot = Quaternion.RotateTowards(_transform.rotation, rota, targetAngle);
            _transform.rotation = nextRot;
        }

        Debug.Log(movement);

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

        if (rb.velocity.x != 0f || rb.velocity.z != 0f)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
        }

        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.D))
        {
            waitTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.W)&& Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.D))
        {
            waitTime = 0f;
            speed = 5;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            _anim.SetTrigger("Skil");
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            _anim.SetTrigger("Ult");
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.D)) && waitTime >= 3f)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Run);
            speed = 10;
        }

        if(rb.velocity == Vector3.zero)
        {
            _anim.SetInteger("MotionIndex", (int)MotionIndex.Idol);
        }

        

    }

    enum MotionIndex 
    {
        Idol = 0,Walk = 10,Run = 20, Avoid = 30
    }
}
