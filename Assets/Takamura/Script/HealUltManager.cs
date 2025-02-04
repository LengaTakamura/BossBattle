using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class HealUltManager : MonoBehaviour
{
    [SerializeField]
    float _delayTime =  1f;
    [SerializeField]
    float _healValue = 100f;
    CancellationTokenSource _cts;
    CancellationToken _ct;
    bool _isHealing = false;
    private void Start()
    {
        
    }

    async void OnTriggerEnter(Collider other)
    {
        _isHealing = true;
        _cts = new CancellationTokenSource();
        _ct = _cts.Token;
        if (other.TryGetComponent(out IDamageable damage))
        {
            while (_isHealing)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_delayTime), cancellationToken: _ct);
                damage.HitHeal(_healValue);
            }

        }
    }
    private void OnTriggerStay(Collider other )
    {
        
    }

    async void OnTriggerExit(Collider other)
    {
        _isHealing = false;
        await UniTask.Delay(TimeSpan.FromSeconds(3),cancellationToken: _ct);
        _cts.Cancel();
    }

}
