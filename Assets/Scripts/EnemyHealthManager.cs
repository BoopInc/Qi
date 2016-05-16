using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{

    [Header("Stats")]
    public float staminaRate;
    public GameObject healthBar;

    public bool regenStamina;
    private float health, stamina;

    void Start()
    {
        //Set Stat Values
        health = 10;
        stamina = 10;
        regenStamina = true;
    }

    void Update()
    {
        //Update ui numbers
        healthBar.GetComponent<Slider>().value = health;

        //If health is zero or negative delete enemy, DEATH
        if (health <= 0)
        {
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
