using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayermanagerDemo : MonoBehaviour
{
    [SerializeField] GameObject[] _players;

    [SerializeField] CinemachineCamera _camera;

    
    int _playerCount = 2;

    private int _priority = 10;

    private int _lessPriority = 0;

    [SerializeField]
    int _firstIndex = 0;


    private void Awake()
    {

        _players[0].SetActive(true);
        for (int i = 1; i < _players.Length; i++)
        {
            
            _players[i].SetActive(false);
        }

        InitCamera();
    }

    void InitCamera()
    {
        _camera.Follow = _players[0].transform;    
    }




    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChasngeCara(1);
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChasngeCara(0);
            
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
                a = (int)player.GetComponent<PlayerDemo>().Anim;
                Debug.Log(a);
                player.SetActive(false);
            }

        }
        _players[i].SetActive(true);
        _players[i].transform.position = pos;
        _players[i].transform.forward = forward;
        _players[i].GetComponent<PlayerDemo>().AnimSet(a);
        _camera.Follow = _players[i].transform;

       
    }

   
}
