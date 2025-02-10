
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public float DamageBuff = 1;

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
            var player = transform.root.transform.Find("Player");
            var playerDamage = player.GetComponent<IDamageable>();
            if (damage.CurrentHealth > 0)
                damage.HitDamage(playerDamage.AttackPower + DamageBuff);
            var playerBase = player.GetComponent<PlayerBase>();
            playerBase.AttackAction?.Invoke();
        }
    }

  

}
