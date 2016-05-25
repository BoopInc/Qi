using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{

    [Header("Stats")]
    public float staminaRate;
    public GameObject healthBar;
    public float maxHealth;
    public GameObject healthOrb;

    public bool regenStamina;
    private float health, stamina;

    void Start()
    {
        //Set Stat Values
        health = maxHealth;
        stamina = 3;
        regenStamina = true;
    }

    void Update()
    {
        //Update ui numbers
        healthBar.GetComponent<Slider>().value = health;
        healthBar.GetComponent<Slider>().maxValue = maxHealth;

        //If health is zero or negative delete enemy, DEATH
        if (health <= 0)
        {
            float ran = Random.Range(0, 3);
            if(ran < 2)
            {
                Instantiate(healthOrb, transform.position, Quaternion.identity);
            }
         
            Destroy(gameObject);
        }

        if (stamina < 0)
        {
            setStamina(0);
        }

        //If stamina regen is enabled increase stamina
        if (regenStamina)
            changeStamina(staminaRate * Time.deltaTime);

    }

    void setHealth(float num)
    {
        health = num;
    }

    public void changeHealth(float num)
    {
        health = health + num;
        //GameObject cam = GameObject.FindGameObjectWithTag("Camera");
        //cam.GetComponent<CameraController>().StartCoroutine("LightShake");
    }

    void setStamina(float num)
    {
        stamina = num;
    }

    public void changeStamina(float num)
    {
        stamina = stamina + num;
    }

    public float getStamina()
    {
        return stamina;
    }

}
