using UnityEngine;

public class HeadManager : MonoBehaviour
{
    public float Damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            damage.HitDamage(Damage);
        }
    }
}
