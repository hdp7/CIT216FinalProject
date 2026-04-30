using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private float health;
    private float boostAmount;
    public float maxHealth;
    public float maxBoost;
    public bool isDead;
    private float damageAmount;
    private float damageInterval = .5f; 
    private MechMovementController movement;


    private ParticleSystem explosion;
    private ParticleSystem smoke;
    public GameObject HUD;
    public Slider healthSlider;
    public Slider boostSlider;
    public float fillRate;

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
                StartCoroutine("Death");
            }
        }
    }

    void OnEnable()
    {
        movement = GetComponent<MechMovementController>();
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
        StartCoroutine(Fill());
        boostSlider.maxValue = maxBoost;
        boostSlider.value = boostSlider.maxValue;
        health = maxHealth;
        healthSlider.maxValue = health;
        healthSlider.value = health;
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
    }
    public void Death()
    {
        movement.enabled = false;
        explosion.Play();
        smoke.Play();
        HUD.SetActive(false);
        movement.enabled = false;
    }

    IEnumerator Fill()
    {
       
        while (true)
        {
            bool isBoosting = movement.boostActive;
            if (isBoosting && boostSlider.value > 0)
            {
                Debug.Log("Using Boost");
                boostSlider.value -= fillRate;
                yield return new WaitForSeconds(.1f);
            }
            else if (!isBoosting && boostSlider.value < maxBoost)
            {
                Debug.Log("Recovering Boost");
                boostSlider.value += fillRate;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
