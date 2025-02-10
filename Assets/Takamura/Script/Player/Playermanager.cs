using Cysharp.Threading.Tasks;
using R3;
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
        _tmp.text = player.CoolDownTime > 0 ? $"{player.CoolDownTime.ToString("0.0")}" : "E";
    }

    void UpdateUltText(PlayerBase player)
    {
        _ultSlider.value = player.CurrentEnergy / player.UltEnergy;
    }


    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !DeadPlayers.Contains(_players[1]))
        {
            ChangeChara(1);
            _enemyManager.SetTarget(_players[1]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)&& !DeadPlayers.Contains(_players[0]))
        {
            ChangeChara(0);
            _enemyManager.SetTarget(_players[0]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !DeadPlayers.Contains(_players[2]))
        {
            ChangeChara(2);
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
                if(pl.CharaIndex == i)
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

        if(DeadPlayers.Count == _players.Length)
        {
            Debug.Log("GameOver");
        }
        if(index != 2)
        {
            ChangeChara(index + 1);

        }
        else
        {
            ChangeChara(0);
        }

    }
    
    public void PlayerHPBarUpdate(IDamageable damage)
    {
        _playerheealthBarManager?.FillUpdate(damage.CurrentHealth / damage.MaxHealth);
    }

 

}
    
