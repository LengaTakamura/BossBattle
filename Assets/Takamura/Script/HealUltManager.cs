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
            try
            {
                while (_isHealing)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_delayTime), cancellationToken: _ct);
                    damage.HitHeal(_healValue);
                }
            }
            catch (Exception ex)
            {
                
            }
            

        }
    }
    private void OnTriggerStay(Collider other )
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        _isHealing = false;
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

}
