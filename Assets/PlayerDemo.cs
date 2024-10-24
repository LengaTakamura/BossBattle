using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerDemo : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    int speed = 0 ;
    MotionIndex motionIndex;
    Animator anim;
    public float waitTime = 0;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody>();
        anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        AnimationManagement();
    }

    void Moving()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);

        
    }



    void AnimationManagement()
    {
        if(rb.velocity.x >= 0.1f || rb.velocity.x >= 0.1f)
        {
            anim.SetInteger("MotionIndex", (int)MotionIndex.Walk);
        }

        if (Input.GetKey(KeyCode.W))
        {
            waitTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            waitTime = 0f;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetTrigger("Skil");
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetTrigger("Ult");
        }

        if(Input.GetKey(KeyCode.W) && waitTime >= 3f)
        {
            anim.SetInteger("MotionIndex", (int)MotionIndex.Run);
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
