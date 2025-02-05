using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;

public class Playermanager : MonoBehaviour
{
    [SerializeField] GameObject[] _players;

    [SerializeField] CinemachineCamera _camera;

    [SerializeField]
    EnemyManager _enemyManager;
    [SerializeField]
    PlayerheealthBarManager _playerheealthBarManager;
    [SerializeField]
    StaminaBarManager _staminaBarM;

    GameObject _target;

    private void Start()
    {
        _players[0].SetActive(true);
        _target = _players[0];
        for (int i = 1; i < _players.Length; i++)
        {
            _players[i].SetActive(false);
        }
        InitCamera();
        _enemyManager.SetTarget(_players[0]);
        var damage = _players[0].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
        PlayerBase.OnStaminaChanged += _staminaBarM.SliderUpdate;
        foreach ( var player in _players)
        {
            var pl = player.GetComponent<PlayerBase>();
            pl.Ondamaged.Subscribe(damage =>
            {
                PlayerHPBarUpdate(pl.GetComponent<IDamageable>());
            }).AddTo(this);
        }
  
    }
   
    void InitCamera()
    {
        _camera.Target.TrackingTarget = _players[0].transform.GetChild(1);
    }



    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChasngeCara(1);
            _enemyManager.SetTarget(_players[1]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChasngeCara(0);
            _enemyManager.SetTarget(_players[0]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChasngeCara(2);
            _enemyManager.SetTarget(_players[2]);

        }

        if (Input.GetMouseButtonDown(1))
        {
            var player = _target.GetComponent<PlayerBase>();
            if( PlayerBase.CurrentStamina != 0)
            {
                var anim = _target.GetComponentInChildren<Animator>();
                anim.SetTrigger("Avoid");

            }

        }
    }


  
    
    private void ChasngeCara(int i)
    {


        Vector3 pos = Vector3.zero;
        Vector3 forward = Vector3.zero;
        int a = 0;
        foreach (var player in _players)
        {
            if (player.activeSelf)
            {
                pos = player.transform.position;
                forward = player.transform.forward;
                player.SetActive(false);
            }

        }
        _players[i].SetActive(true);
        _target = _players[i];
        _players[i].transform.position = pos;
        _players[i].transform.forward = forward;
        _players[i].GetComponent<PlayerBase>().StateChange((PlayerBase.MotionIndex)a);
        _camera.Target.TrackingTarget = _players[i].transform.GetChild(1);
        var damage = _players[i].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
    }
    
    public void PlayerHPBarUpdate(IDamageable damage)
    {
        _playerheealthBarManager?.FillUpdate(damage.CurrentHealth / damage.MaxHealth);
    }

 

}
    
