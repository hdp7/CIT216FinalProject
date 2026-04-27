using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float health;
    public float boostAmount;
    public float maxHealth;
    public float maxBoost;
    public bool isDead;
    private float damageAmount;
    private float damageInterval = .5f; 
    private MechMovementController movement;


    private ParticleSystem explosion;
    private ParticleSystem smoke;
    public Canvas HUD;
    public Slider healthSlider;
    public Slider boostSlider;

    private UnityAction<float> damageListener;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health -= value;
            if (health <= 0)
            {
                Debug.Log("Player is Dead");
                isDead = true;
                GetComponent<MechMovementController>().enabled = false;
                StartCoroutine("Death");
            }
        }
    }

    void OnEnable()
    {
        damageListener = new UnityAction<float>(TakeDamage);
        //subscribes to the event
        EventManager.StartListening("PlayerDamager", damageListener);
    }
    void OnDisable()
    {
        EventManager.StopListening("PlayerDamager", damageListener);
    }

    private void Start()
    {
        health = -maxHealth;
        Debug.Log("Health: " + health);
    }

    public void TakeDamage(float damage)
    {
        damageAmount -= damage;
        StartCoroutine(ApplyDamage());
    }
    public IEnumerator ApplyDamage()
    {
        //Add code to reduce health
        health -= damageAmount; //calls setter and setter subtracts!
        Debug.Log("Player Health: " + health);
        //Wait for interval
        yield return new WaitForSeconds(damageInterval);
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        boostSlider.value = boostAmount;
    }
    public void Death()
    {
        movement.enabled = false;
        explosion.Play();
        smoke.Play();

    }
}
