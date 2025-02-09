using UnityEngine;

public class HeadManager : MonoBehaviour
{
    public float _damageMag = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            var enemy = transform.root.GetComponent<IDamageable>();
            if(damage.CurrentHealth > 0)
            damage.HitDamage(enemy.AttackPower * _damageMag);
        }
    }
}
