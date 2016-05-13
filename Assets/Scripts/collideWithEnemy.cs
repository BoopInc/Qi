using UnityEngine;
using System.Collections;

public class collideWithEnemy : MonoBehaviour {

    private GameObject Player;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "EnemyAlt") {
            GetComponentInParent<Rigidbody2D>().GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
