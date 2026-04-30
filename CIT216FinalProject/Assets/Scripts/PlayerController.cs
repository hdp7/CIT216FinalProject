using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerController : MonoBehaviour
{
    private float health;
    private float boostAmount;
    public float maxHealth;
    public float maxBoost;
    public bool isDead;
    private float damageAmount;
    private float damageInterval = .5f; 
    private MechMovementController movement;
    private Rigidbody rb;


    private ParticleSystem explosion;
    private ParticleSystem smoke;
    public GameObject HUD;
    public Slider healthSlider;
    public Slider boostSlider;
    public float fillRate;
    private bool boostExhausted;
    private float boostCooldown;

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
            }
        }
    }

    void OnEnable()
    {
        movement = GetComponent<MechMovementController>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        HandleBoost();
    }

    private void HandleBoost()
    {
        bool isBoosting = movement.boostActive;
        //Cooldown
        if (boostExhausted)
        {
            boostCooldown -= Time.deltaTime;
            movement.boostActive = false;
            if (boostCooldown <= 0f)
            {
                boostExhausted = false;
            }

            return;
        }

        if (isBoosting && boostSlider.value > 0)
        {
            boostSlider.value -= fillRate * Time.deltaTime;
            //Temporarily disable boost if boost is fully used up
            if (boostSlider.value <= 0)
            {
                rb.linearVelocity = Vector3.zero;
                boostSlider.value = 0;
                boostExhausted = true;
                boostCooldown = 3f;
                movement.boostActive = false;
            }
        }
        else if (!isBoosting && boostSlider.value < maxBoost)
        {
            Debug.Log("Recovering Boost");
            boostSlider.value += fillRate * Time.deltaTime;
        }
        //Avoid over/under flow
        boostSlider.value = Mathf.Clamp(boostSlider.value, 0f, maxBoost);
    }
    public void Death()
    {
        movement.enabled = false;
        explosion.Play();
        smoke.Play();
        HUD.SetActive(false);
        movement.enabled = false;
    }
}
