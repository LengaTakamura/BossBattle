using R3;
using UnityEngine;
public class PlayerArcher : PlayerBase
{
    bool _isAiming = false;
    [SerializeField]
    float _aimMoveSpeed = 1f;

    float _speedX = 0f;
    float _speedY = 0f;
    [SerializeField]
    RectTransform _cursor;
    [SerializeField]
    float _bowDamageBuff = 1f; 
    private void Start()
    {
        Aiming();
    }

    protected override void Update()
    {
        base.Update();

        Attack();


    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();
        if (_isAiming)
        {
            MoveParam();
            MoveWithAim();

        }

    }

    private void Aiming()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        var attack = Observable.EveryUpdate()
           .Where(_ => Input.GetKeyDown(KeyCode.R))
           .Where(_ => gameObject.activeSelf)
           .Where(_ => !stateInfo.IsName("AimStart"))
           .Scan(0, (count, _) => count + 1)
           .Subscribe(count =>
           {
               if (count % 2 != 0)
               {
                   _isAiming = true;
                   _cursor.gameObject.SetActive(true);

               }
               else
               {
                   _isAiming = false;
                   _cursor.gameObject.SetActive(false);
               }
               _anim.SetBool("Aim", _isAiming);
           });

    }

    private void Attack()
    {
        if (_isAiming && Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
            var screenPoint = RectTransformUtility.WorldToScreenPoint(null,_cursor.position);
            Ray screeenRay = Camera.main.ScreenPointToRay(screenPoint);
            var dir = screeenRay.direction.normalized;
            Ray ray = new Ray(transform.position, dir);
            if(Physics.Raycast(ray,out RaycastHit hit))
            {

                if (hit.transform.root != null && hit.transform.root.TryGetComponent(out IDamageable damage) && hit.transform.gameObject.tag == "Enemy")
                {
                  
                    
                    if(hit.transform.gameObject.name == "Jaw3")
                    {
                        _bowDamageBuff = 100;
                        damage.HitDamage(_attackPower + _bowDamageBuff );
                        Debug.Log("Headshot");
                    }
                    else
                    {
                        Debug.Log("HitBow");
                        _bowDamageBuff = 1;
                        damage.HitDamage(_attackPower + _bowDamageBuff);
                    }
                }
            }
            Debug.DrawRay(transform.position, dir * 10000f, Color.magenta);
        }
    }

    private void MoveWithAim()
    {
        var velo = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        Vector3 moveDirection = cameraForward * velo.z + cameraRight * velo.x;
        _rb.linearVelocity = moveDirection * _aimMoveSpeed;
        if (velo.magnitude > 0)
        {
            var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(cameraForward), 10f);
            transform.rotation = rot;
        }

    }

    private void MoveParam()
    {
        if (Input.GetKey(KeyCode.W) && _speedY < 0.5)
        {
            _speedY += 0.01f;
        }
        else if (Input.GetKey(KeyCode.A) && _speedX > -0.5)
        {
            _speedX -= 0.01f;
        }
        else if (Input.GetKey(KeyCode.S) && _speedY > -0.5)
        {
            _speedY -= 0.01f;
        }
        else if (Input.GetKey(KeyCode.D) && _speedX < 0.5)
        {
            _speedX += 0.01f;
        }

        if (!Input.GetKey(KeyCode.W) && _speedY > 0)
        {
            _speedY -= 0.01f;
        }
        if (!Input.GetKey(KeyCode.A) && _speedX < 0)
        {
            _speedX += 0.01f;
        }
        if (!Input.GetKey(KeyCode.S) && _speedY < 0)
        {
            _speedY += 0.01f;
        }
        if (!Input.GetKey(KeyCode.D) && _speedX > 0)
        {
            _speedX -= 0.01f;
        }


        _anim.SetFloat("BlendX", _speedX);
        _anim.SetFloat("BlendY", _speedY);
    }


}
