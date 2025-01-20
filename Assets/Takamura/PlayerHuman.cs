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

        if (Anim == MotionIndex.Skate)
        {
            var velo = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0 ).normalized;

            _rb.linearVelocity = velo * _skateSpeed;

            _rb.useGravity = false;
        }
       
    }
    async void Rotate()
    {
        bool raycastHit = Physics.Raycast(transform.position + transform.TransformDirection(Vector3.up),transform.forward, out _hit, 1.2f);
        Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.up) * 2, transform.forward, Color.red);
        if (raycastHit && Anim != MotionIndex.Skate)
        {
            var hitNormal = _hit.normal;
            _rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;
            Debug.Log("maehoukouKabe");
            Quaternion targetRot = Quaternion.AngleAxis(-90, Vector3.right);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 100f);
            _rb.constraints |= RigidbodyConstraints.FreezeRotationX;
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Anim = MotionIndex.Skate;
            Speed = _skateSpeed ;
           
        }
       
    }



}
