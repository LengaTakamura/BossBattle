using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemo : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    int speed = 0 ;    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
        
    }
}
