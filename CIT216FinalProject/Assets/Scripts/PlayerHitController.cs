//4/29/26
//Herman Pagan Alvarez
//Meant to handle hits from enemy projectiles
using UnityEngine;
public class PlayerHitController : MonoBehaviour
{
    public PlayerController player;
    private Collider col;
    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            float damage = other.GetComponent<MissileController>().damage;
            player.TakeDamage(damage);
        }
    }

}
