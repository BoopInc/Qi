using UnityEngine;
using System.Collections;

public class collideWithEnemy : MonoBehaviour {

    private GameObject Player;
    
    //Find player
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}

    //If player collides with enemy, slow player completely
    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "EnemyAlt") {
            GetComponentInParent<Rigidbody2D>().GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
