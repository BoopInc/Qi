using UnityEngine;
using System.Collections;

public class MeleeController : MonoBehaviour {

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        //Set melee attack direction
        //Find cursor position
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 0;

        //Find player position
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        cursorPosition.x = cursorPosition.x - playerPos.x;
        cursorPosition.y = cursorPosition.y - playerPos.y;

        //Calculate angle between player and cursor
        float angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x) * (180 / Mathf.PI);

        //Assign melee direction
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        StartCoroutine("attackFrame");
    }
	
	void Update () {
        //Make melee hitbox follow player
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    IEnumerator attackFrame() {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        //If ability hits player, then do damage
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyHealthManager>().changeHealth(-3f);
        }
    }
}
