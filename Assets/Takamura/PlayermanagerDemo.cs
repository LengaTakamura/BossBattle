using UnityEngine;

public class PlayermanagerDemo : MonoBehaviour
{
    [SerializeField] GameObject[] _players;

    private void Start()
    {
        _players[0].SetActive(true);

        for (int i = 1; i < _players.Length; i++)
        {
            _players[i].SetActive(false);
        }
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

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {

        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {

        }
    }

    private void ChasngeCara(int i)
    {
        Vector3 pos = Vector3.zero;
        Vector3 forward = Vector3.zero;
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
        _players[i].transform.position = pos;
        _players[i].transform.forward = forward;
    }

    private void ChangeAnim()
    {
            
    }
}
