using Unity.Cinemachine;
using UnityEngine;

public class PlayermanagerDemo : MonoBehaviour
{
    [SerializeField] GameObject[] _players;

    [SerializeField] CinemachineCamera _camera;

    [SerializeField]
    EnemyManager _enemyManager;
    [SerializeField]
    PlayerheealthBarManager _playerheealthBarManager;
    private void Awake()
    {
       
        _players[0].SetActive(true);
        for (int i = 1; i < _players.Length; i++)
        {
            _players[i].SetActive(false);
        }
        InitCamera();
        _enemyManager.SetTarget(_players[0]);
        var damage = _players[0].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
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
                a = (int)player.GetComponent<PlayerBase>().State;
                Debug.Log(a);
                player.SetActive(false);
            }

        }
        _players[i].SetActive(true);
        _players[i].transform.position = pos;
        _players[i].transform.forward = forward;
        _players[i].GetComponent<PlayerBase>().StateChange((PlayerBase.MotionIndex)a);
        _camera.Target.TrackingTarget = _players[i].transform.GetChild(1);
        var damage = _players[i].GetComponent<IDamageable>();
        PlayerHPBarUpdate(damage);
        Debug.Log(_camera.Target.TrackingTarget);


    }

    public void PlayerHPBarUpdate(IDamageable damage)
    {
        _playerheealthBarManager?.FillUpdate(damage.CurrentHealth / damage.MaxHealth);
    }

  
}
