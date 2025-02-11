using UnityEngine;

public class FlameUltHitManager : MonoBehaviour
{
     [SerializeField]
    private float _damage = 15f;

    private void OnTriggerStay(Collider other)
    {

        
        if (other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            
            if (damage.CurrentHealth > 0)
            {
                damage.HitDamage(_damage);
            }
               
        }
    }
}
