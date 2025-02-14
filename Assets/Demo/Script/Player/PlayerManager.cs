using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    TextMeshProUGUI _tmp;
    [SerializeField]
    Slider _ultSlider;
    public List<GameObject> DeadPlayers = new();
    bool _canAvoid = true;
    float _timer;
    public float AvoidTime = 0.75f;
    private void Start()
    {
        Initialized();
    }

    private void Initialized()
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
        var playerBase = _players[0].GetComponent<PlayerBase>();
        UpdateSkillText(playerBase);
        UpdateUltText(playerBase);
        playerBase.DeathAction += ChangeNextChara;
        playerBase.OnCoolDownChanged += UpdateSkillText;
        playerBase.OnEnergyChanged += UpdateUltText;
        PlayerBase.OnStaminaChanged += _staminaBarM.SliderUpdate;
        foreach (var player in _players)
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

    void UpdateSkillText(PlayerBase player)
    {
        _tmp.text = player.CoolDownTime > 0 ? $"{player.CoolDownTime.ToString("0.0")}" : "ƒXƒLƒ‹”­“®‰Â”\";
    }

    void UpdateUltText(PlayerBase player)
    {
        _ultSlider.DOValue(player.CurrentEnergy / player.UltEnergy, 0.1f);
    }

   

    private void Update()
    {
        InputManagement();
    }

    void InputManagement()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !DeadPlayers.Contains(_players[1]))
        {
            ChangeChara(1);
            _enemyManager.SetTarget(_players[1]);
            SetKinokoUltTarget(_players[1]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !DeadPlayers.Contains(_players[0]))
        {
            ChangeChara(0);
            _enemyManager.SetTarget(_players[0]);
            SetKinokoUltTarget(_players[0]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !DeadPlayers.Contains(_players[2]))
        {
            ChangeChara(2);
            _enemyManager.SetTarget(_players[2]);
            SetKinokoUltTarget(_players[2]);

        }

        if (Input.GetMouseButtonDown(1))
        {
            if(!_canAvoid)
                return;
            var player = _target.GetComponent<PlayerBase>();
            if (PlayerBase.CurrentStamina != 0 )
            {
                _canAvoid = false;
                var anim = _target.GetComponentInChildren<Animator>();
                anim.SetTrigger("Avoid");
               
            }

        }
    }

    private void FixedUpdate()
    {
        Timer();
    }

    private void Timer()
    {
        if (!_canAvoid)
        {
            _timer += Time.deltaTime;
            if (_timer > AvoidTime)
            {
                _timer = 0;
                _canAvoid = true;
            }
        }   
    }

    void SetKinokoUltTarget(GameObject player)
    {
        try
        {
            var kinokoUlt = transform.GetComponentInChildren<BuffShieldManager>();
            kinokoUlt.SetTarget(player.GetComponent<PlayerBase>());
        }
        catch { }
        
       
    }


    private void ChangeChara(int i)
    {
        Vector3 pos = Vector3.zero;
        Vector3 forward = Vector3.zero;
        int a = 0;
        foreach (var player in _players)
        {
            if (player.activeSelf)
            {
                var pl = player.GetComponent<PlayerBase>();
                if (pl.CharaIndex == i)
                {
                    return;
                }
                pos = player.transform.position;
                forward = player.transform.forward;
                player.SetActive(false);
                pl.OnCoolDownChanged -= UpdateSkillText;
                pl.DeathAction -= ChangeNextChara;
                pl.OnEnergyChanged -= UpdateUltText;
            }

        }
        _players[i].SetActive(true);
        _target = _players[i];
        _players[i].transform.position = pos;
        _players[i].transform.forward = forward;
        var playerBase = _players[i].GetComponent<PlayerBase>();
        playerBase.StateChange((PlayerBase.MotionIndex)a);
        UpdateSkillText(playerBase);
        UpdateUltText(playerBase);
        playerBase.OnCoolDownChanged += UpdateSkillText;
        playerBase.DeathAction += ChangeNextChara;
        playerBase.OnEnergyChanged += UpdateUltText;
        _camera.Target.TrackingTarget = _players[i].transform.GetChild(1);
        var damage = _players[i].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
    }

    void ChangeNextChara(int index)
    {

        DeadPlayers.Add(_players[index]);

        if (DeadPlayers.Count == _players.Length)
        {
            Debug.Log("GameOver");
        }
        if (index != 2)
        {
            if (DeadPlayers.Contains(_players[index + 1]))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!DeadPlayers.Contains(_players[i]))
                    {
                        ChangeChara(i);
                    }
                }
            }
            else
            {
                ChangeChara(index + 1);
            }


        }
        else
        {
            if (DeadPlayers.Contains(_players[0]))
            {
                ChangeChara(1);
            }
            ChangeChara(0);
        }

    }

    public void PlayerHPBarUpdate(IDamageable damage)
    {
        _playerheealthBarManager?.FillUpdate(damage.CurrentHealth / damage.MaxHealth);
    }

    public void OnPause()
    {

        PlayerBase.OnPause = true;
        foreach (var player in _players)
        {

            if (player.activeSelf)
            {
                var anim = player.GetComponentInChildren<Animator>();
                anim.SetBool("OnPause", true);

            }

        }
    }

    public void OnResume()
    {

        PlayerBase.OnPause = false;
        foreach (var player in _players)
        {

            if (player.activeSelf)
            {
                var anim = player.GetComponentInChildren<Animator>();
                anim.SetBool("OnPause", false);

            }

        }
    }

}

