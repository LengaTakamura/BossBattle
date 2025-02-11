using UnityEngine;

public class FlameUltManager : MonoBehaviour
{
    [SerializeField]
    private float _damage = 10f;

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            if (damage.CurrentHealth > 0)
                damage.HitDamage( _damage);
        }
    }
}
