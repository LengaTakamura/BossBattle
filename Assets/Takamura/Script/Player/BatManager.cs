using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Threading;
using UnityEngine;

public class BatManager : MonoBehaviour
{
    CancellationTokenSource _cts;
    [SerializeField]
    float _lifeTime = 5f;
    [SerializeField]
    GameObject _attackEffect;

    GameObject _target;

    public Action<Vector3> GetTargetPos;
    PlayerBase _archer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        var players = Resources.FindObjectsOfTypeAll<PlayerBase>();
        foreach (var player in players)
        {
            player.AttackAction += AttackWith;
            if(player.gameObject.name == "Archer")
            {
                _archer = player;
            }
        }
        _target = FindAnyObjectByType<EnemyManager>().transform.Find("EnemyCol").gameObject;
        _cts = new CancellationTokenSource();
        await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime));
        foreach (var player in players)
        {
            player.AttackAction -= AttackWith;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var rot = _target.transform.position;
        rot.y = transform.position.y;
        transform.LookAt(rot);
        OnTargetPosUpdate();
    }

   private void OnTargetPosUpdate()
    {
        GetTargetPos?.Invoke(_target.transform.position);
    }

    public void AttackWith()
    {
        Quaternion lookTarget = Quaternion.LookRotation(_target.transform.position);
        var effect = Instantiate(_attackEffect,transform.position +transform.forward + new Vector3(0,1.6f,0),lookTarget * _attackEffect.transform.rotation);
        var effectManager = effect.GetComponent<BatEffectManager>();
        effectManager.Initialized(this);
        effectManager.HitAction += _archer.AddEnergy;
    }



    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
