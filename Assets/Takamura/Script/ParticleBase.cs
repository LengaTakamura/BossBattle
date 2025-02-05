using UnityEngine;

public abstract class ParticleBase : MonoBehaviour
{
    [SerializeField]
    float _damage = 100;
    public virtual void OnParticleCollision(GameObject other)
    {
        
    }
}
