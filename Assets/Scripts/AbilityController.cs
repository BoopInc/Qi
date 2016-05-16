using UnityEngine;
using System.Collections;

public class AbilityController : MonoBehaviour {

    [Header("Ability Objects")]
    public GameObject projectile;
    public GameObject melee;

    private bool meleeCD;
    private bool shootCD;
    private int shootCount;

    void Start() {
        //Set Bools
        meleeCD = false;
        shootCD = false;

        //Set numbers
        shootCount = 0;
    }

    // Update is called once per frame
    void Update() {
        //update abilities
        UpdateAbilities();
    }

    void UpdateAbilities()
    {
        //run eat ability function
        updateShoot();
        updateMelee();
    }

    //ranged ability
    void updateShoot() {
        //Get mouse position and get rid of z value
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 0;

        //find character position on screen
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);

        //Find differences in the x,y components of player and cursor
        cursorPosition.x = cursorPosition.x - playerPos.x;
        cursorPosition.y = cursorPosition.y - playerPos.y;

        //Calculate angle between player and cursor
        float angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x);

        //If player right clicks and ability is not on cooldown, then spawn ranged object
        if (Input.GetMouseButtonDown(1) && shootCD == false) {
            GameObject obj = Instantiate(projectile, new Vector2(transform.position.x,transform.position.y), Quaternion.identity) as GameObject;
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * 1.8f, Mathf.Sin(angle) * 1.8f);
            shootCount += 1;
        }

        //If ammo is zero, then shoot goes on cooldown
        if (shootCount == 3)
        {
            StartCoroutine("waitShootCD");

        }
    }

    //basic attack, melee ability
    void updateMelee() {
        //If left click is down, and attack is off cooldown, spawn melee object
        if (Input.GetMouseButtonDown(0) & meleeCD == false) {
            meleeCD = true;
            StartCoroutine("waitMeleeCD");
            Instantiate(melee, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
    }

    //melee attack cooldown
    IEnumerator waitMeleeCD() {
        //Set movement to false
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<CharacterController>().canMove = false;

        yield return new WaitForSeconds(.2f);
        
        //Set movement to true
        GetComponent<CharacterController>().canMove = true;

        yield return new WaitForSeconds(.3f);
        meleeCD = false;
    }

    //ranged attack cooldown
    IEnumerator waitShootCD() {
        shootCD = true;
        yield return new WaitForSeconds(1f);
        shootCD = false;
    }

}
