using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;

public class BuffUltManager : MonoBehaviour
{
    [SerializeField]
    float _delayTime = 1f;
    [SerializeField]
    float _isBuffValue = 100f;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.TryGetComponent(out IDamageable damage))
        {
            damage.AttackPower += _isBuffValue;
            Debug.Log("Buff");
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.TryGetComponent(out IDamageable damage))
        {
            damage.AttackPower -= _isBuffValue;
        }
    }
        

    private void OnDestroy()
    {
        
    }

}
