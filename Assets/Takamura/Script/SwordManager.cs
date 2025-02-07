
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public float DamageMag = 1;

    private void Start()
    {
      
      
    }


    private void Update()
    {
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out IDamageable damage) && other.transform.parent.gameObject.tag == "Enemy")
        {
            var player = transform.root.GetComponent<IDamageable>();
            damage.HitDamage(player.AttackPower * DamageMag);
        }
    }

  

}
