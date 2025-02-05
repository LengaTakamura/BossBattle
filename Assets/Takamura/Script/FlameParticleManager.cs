using UnityEngine;

public class FlameParticleManager : MonoBehaviour
{
    [SerializeField]
    private float _damage = 100f;

    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Player")
        {
            damage.HitDamage(_damage);
        }
    }
}
