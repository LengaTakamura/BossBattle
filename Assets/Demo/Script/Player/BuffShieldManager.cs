using UnityEngine;

public class BuffShieldManager : MonoBehaviour
{
    PlayerBase _player;
    [SerializeField]
    float _buffValue = 5;

    private void Start()
    {
       
        _player = transform.root.GetComponentInChildren<PlayerBase>(false);
        DefenseBuff();
    }

    private void Update()
    {
        transform.position = _player.transform.position;
    }

    public void SetTarget(PlayerBase playerBase)
    {
        _player = playerBase;
    }

    void DefenseBuff()
    {
        _player.Defense += _buffValue;
    }

    private void OnDestroy()
    {
        _player.Defense -= _buffValue;
    }
}
