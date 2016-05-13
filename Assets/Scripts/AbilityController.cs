using UnityEngine;
using System.Collections;

public class AbilityController : MonoBehaviour {

    //public GameObject cursor;
    //private Vector3 mousePosition;
    public GameObject projectile;
    public GameObject melee;
    private bool meleeCD;
    private bool shootCD;
    private int shootCount;

    // Use this for initialization
    void Start() {
        //mousePosition = cursor.GetComponent<CursorController>().cursorPosition;
        meleeCD = false;
        shootCD = false;
        shootCount = 0;
    }

    // Update is called once per frame
    void Update() {
        UpdateAbilities();
    }

    void UpdateAbilities()
    {
        updateShoot();
        updateMelee();
    }

    void updateShoot() {
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        cursorPosition.x = cursorPosition.x - objectPos.x;
        cursorPosition.y = cursorPosition.y - objectPos.y;

        float angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x);

        if (Input.GetMouseButtonDown(1) && shootCD == false) {
            print(angle);
            GameObject obj = Instantiate(projectile, new Vector2(transform.position.x,transform.position.y), Quaternion.identity) as GameObject;
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * 1.8f, Mathf.Sin(angle) * 1.8f);
            shootCount += 1;
            
        }


        if (shootCount == 3)
        {
            StartCoroutine("waitShootCD");

        }
    }

    void updateMelee() {
        if (Input.GetMouseButtonDown(0) & meleeCD == false) {
            meleeCD = true;
            StartCoroutine("waitMeleeCD");
            Instantiate(melee, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
    }

    IEnumerator waitMeleeCD() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<CharacterController>().canMove = false;
        yield return new WaitForSeconds(.2f);
        GetComponent<CharacterController>().canMove = true;

        yield return new WaitForSeconds(.3f);
        meleeCD = false;
    }

    IEnumerator waitShootCD() {
        shootCD = true;
        yield return new WaitForSeconds(1f);
        shootCD = false;
    }

}
