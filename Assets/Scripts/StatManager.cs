using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatManager : MonoBehaviour {


    [Header("Stats")]
    public float staminaRate;
    public bool regenStamina;

    [Header("Stat Objects")]
    public GameObject staminaBar;
    public GameObject healthBar;

    private float health,stamina;

	void Start () {
        //Set Stat Values
        health = 10;
        stamina = 20;
        regenStamina = true;
	}
	
	void Update () {
        //Update ui numbers
        staminaBar.GetComponent<Slider>().value = stamina;
        healthBar.GetComponent<Slider>().value = health;

        //Cap health
        if (health > 10) {
            setHealth(10);
        }

        //Lower stamins limit(0)
        if (stamina < 0) {
            setStamina(0);
        }

        //Cap stamina
        if (stamina > 10) {
            setStamina(10);
        }

        //if stamina regen is activated then add to stamina
        if (regenStamina)
            changeStamina(staminaRate * Time.deltaTime);

	}

    void setHealth(float num) {
        health = num;
    }

    public void changeHealth(float num) {
        health = health + num;
    }

    void setStamina(float num) {
        stamina = num;
    }

    public void changeStamina(float num) {
        stamina = stamina + num;
    }

    public float getStamina() {
        return stamina;
    }
}
