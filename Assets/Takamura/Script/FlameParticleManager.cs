using UnityEngine;

public class FlameParticleManager : MonoBehaviour
{
    [SerializeField]
    private float _damageBuff = 10f;

    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            var enemy = transform.root.GetComponent<IDamageable>();
            damage.HitDamage(enemy.AttackPower + _damageBuff);
        }
    }
}
