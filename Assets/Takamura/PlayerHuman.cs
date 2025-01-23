using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerHuman : PlayerBase
{
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _skateSpeed;

    RaycastHit _hit;


    protected override void Update()
    {
        base.Update();
        Rotate();
    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();

        //if (State == MotionIndex.Skate)
        //{
        //    var velo = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0 ).normalized;

        //    _rb.linearVelocity = velo * _skateSpeed;

        //}

       

    }
    async void Rotate()
    {
        CanSkating = Physics.Raycast(transform.position ,transform.forward.normalized * 0.5f, out _hit, 1.2f);
        Debug.DrawRay(transform.position , transform.forward.normalized * 0.5f, Color.red);
        if (CanSkating && Input.GetKeyDown(KeyCode.F))
        { 
            _rb.AddForce(new Vector3(0,10,0),ForceMode.Impulse);   
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            State = MotionIndex.Skate;  
            Speed = _skateSpeed ;
           
        }
       
    }



}
