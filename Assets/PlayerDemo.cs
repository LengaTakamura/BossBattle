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
    Animator anim;
    public float waitTime = 0;
    private Vector3 _lastPosition;
    private Transform _transform;
    private float _currentVelocity = 0;
    public float _smoothTime = 0; 
    public float _maxSpeed = 360f;
    bool canMove = true;
    void Awake()
    {
        _transform = transform;
        
        
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody>();
        anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
    }

    // Update is called once per frame
    void Update()
    {
        AnimationManagement();

        if (canMove)
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
        var position = _transform.position;
        var movement = position - _lastPosition;
        _lastPosition = position;

        if (movement.magnitude > 0.01f)
        {
            Debug.Log("kaiten");
            var rota = Quaternion.LookRotation(movement, Vector3.up);
            var diffAngle = Vector3.Angle(_transform.forward, movement);
            var targetAngle = Mathf.SmoothDampAngle(0, diffAngle, ref _currentVelocity, _smoothTime, _maxSpeed);
            var nextRot = Quaternion.RotateTowards(_transform.rotation, rota, targetAngle);
            Debug.Log(movement);
            _transform.rotation = rota;
        }
            
        

    }



    void AnimationManagement()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Skil") || stateInfo.IsName("Ult"))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        if (rb.velocity.x >= 0.1f || rb.velocity.z >= 0.1f)
        {
            anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
        }

        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.D))
        {
            waitTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.W)|| Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            waitTime = 0f;
            speed = 5;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetTrigger("Skil");
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetTrigger("Ult");
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.D)) && waitTime >= 3f)
        {
            anim.SetInteger("MotionIndex", (int)MotionIndex.Run);
            speed = 10;
        }

        if(rb.velocity == Vector3.zero)
        {
            anim.SetInteger("MotionIndex", (int)MotionIndex.Idol);
        }

        

    }

    enum MotionIndex 
    {
        Idol = 0,Walk = 10,Run = 20, Avoid = 30
    }
}
