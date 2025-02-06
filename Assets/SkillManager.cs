using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private float _damage = 100f;

  
    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out IDamageable damage) && other.gameObject.tag == "Enemy")
        {
            Debug.Log("HIT");
            damage.HitDamage(_damage);
            Destroy(this.gameObject);
        }
    }
}
