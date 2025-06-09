using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Playermanager : MonoBehaviour
{
    [SerializeField] private PlayerBase[] _players;

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
    public List<PlayerBase> DeadPlayers = new();
    bool _canAvoid = true;
    float _timer;
    public float AvoidTime = 0.75f;
    private CancellationTokenSource _cts;
    private bool _isRecovering;
    private void Start()
    {
        Initialized();
        _cts = new CancellationTokenSource();
    }

    private void Initialized()
    {
        _players[0].gameObject.SetActive(true);
        _target = _players[0].gameObject;
        for (int i = 1; i < _players.Length; i++)
        {
            _players[i].gameObject.SetActive(false);
        }
        InitCamera();
        _enemyManager.SetTarget(_players[0].gameObject);
        var damage = _players[0].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
        UpdateSkillText(_players[0]);
        UpdateUltText(_players[0]);
        _players[0].DeathAction += ChangeNextChara;
        _players[0].OnCoolDownChanged += UpdateSkillText;
        _players[0].OnEnergyChanged += UpdateUltText;
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
        _tmp.text = player.CoolDownTime > 0 ? $"{player.CoolDownTime.ToString("0.0")}" : "スキル発動可能";
    }

    void UpdateUltText(PlayerBase player)
    {
        _ultSlider.DOValue(player.CurrentEnergy / player.UltEnergy, 0.1f);
    }

   

    private void Update()
    {
        InputManagement();
        
        if (PlayerBase.CurrentStamina < 3 && !_isRecovering)
        { 
            _isRecovering = true;
            RecoveryDelay(_cts.Token);
        }
        
       
    }

    void InputManagement()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !DeadPlayers.Contains(_players[1]))
        {
            ChangeChara(1);
            _enemyManager.SetTarget(_players[1].gameObject);
            SetKinokoUltTarget(_players[1].gameObject);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !DeadPlayers.Contains(_players[0]))
        {
            ChangeChara(0);
            _enemyManager.SetTarget(_players[0].gameObject);
            SetKinokoUltTarget(_players[0].gameObject);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !DeadPlayers.Contains(_players[2]))
        {
            ChangeChara(2);
            _enemyManager.SetTarget(_players[2].gameObject);
            SetKinokoUltTarget(_players[2].gameObject);

        }

        if (Input.GetMouseButtonDown(1))
        {
            if(!_canAvoid)
                return;
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
            if (player.gameObject.activeSelf)
            {
                var pl = player.GetComponent<PlayerBase>();
                if (pl.CharaIndex == i)
                {
                    return;
                }
                pos = player.transform.position;
                forward = player.transform.forward;
                player.gameObject.SetActive(false);
                pl.OnCoolDownChanged -= UpdateSkillText;
                pl.DeathAction -= ChangeNextChara;
                pl.OnEnergyChanged -= UpdateUltText;
                
            }

        }
        _players[i].gameObject.SetActive(true);
        _target = _players[i].gameObject;
        _players[i].transform.position = pos;
        _players[i].transform.forward = forward;
        _players[i].StateChange((PlayerBase.MotionIndex)a);
        UpdateSkillText(_players[i]);
        UpdateUltText(_players[i]);
        _players[i].OnCoolDownChanged += UpdateSkillText;
        _players[i].DeathAction += ChangeNextChara;
        _players[i].OnEnergyChanged += UpdateUltText;
        _camera.Target.TrackingTarget = _players[i].transform.GetChild(1);
        var damage = _players[i].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
    }

    void ChangeNextChara(int index)
    {

        DeadPlayers.Add(_players[index]);

        if (DeadPlayers.Count == _players.Length)
        {
           GameManager.Instance.GameOvered();
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

            if (player.gameObject.activeSelf)
            {
                var anim = player.GetComponentInChildren<Animator>();
                anim.SetBool("OnPause", true);

            }

        }
    }
    
    private async void RecoveryDelay(CancellationToken token)
    {
        while (PlayerBase.CurrentStamina <3)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            RecoveryStamina();
            Debug.Log(PlayerBase.CurrentStamina);
        }
        _isRecovering = false;
    }
    
    public void RecoveryStamina()
    {
        if (PauseManager.PauseFlg)
        {
            return;
        }
        PlayerBase.CurrentStamina += 1; 
        _staminaBarM.SliderUpdate(PlayerBase.CurrentStamina / 3);
        
    }

    public void OnResume()
    {

        PlayerBase.OnPause = false;
        foreach (var player in _players)
        {

            if (player.gameObject.activeSelf)
            {
                var anim = player.GetComponentInChildren<Animator>();
                anim.SetBool("OnPause", false);

            }

        }
    }

}

