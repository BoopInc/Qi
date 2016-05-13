using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{

    public GameObject healthBar;
    [Header("Stat Numbers")]
    public float staminaRate;

    public bool regenStamina;
    private float health, stamina;

    // Use this for initialization
    void Start()
    {
        //Set Values
        health = 10;
        stamina = 10;
        regenStamina = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Update ui numbers
        healthBar.GetComponent<Slider>().value = health;

        //Death
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (health > 10)
        {
            setHealth(10);
        }

        if (stamina < 0)
        {
            setStamina(0);
        }

        if (stamina > 10)
        {
            setStamina(10);
        }

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
