using Cysharp.Threading.Tasks;
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
    [SerializeField]
    GameObject _target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        var archer = transform.root.GetComponent<PlayerArcher>();

        if (archer != null)
        {
            archer.AttackAction += AttackWith;

        }
        _cts = new CancellationTokenSource();
        await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime));
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var rot = _target.transform.position;
        rot.y = transform.position.y;
        transform.LookAt(rot);
    }



    public void AttackWith()
    {
        var effect = Instantiate(_attackEffect,transform.forward,transform.rotation);
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
