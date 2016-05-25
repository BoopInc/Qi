using UnityEngine;
using System.Collections;

public class dashHitbox : MonoBehaviour {
    private GameObject Player;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.GetComponent<CharacterController>().canDash)
        {
            transform.position = Player.transform.position + new Vector3(2000, 2000, 0);
        }
        else {
            transform.position = Player.transform.position;
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            // other.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector3(Player.GetComponent<Rigidbody2D>().velocity.x * -200, Player.GetComponent<Rigidbody2D>().velocity.y, 0) * 200, ForceMode2D.Impulse);
            other.GetComponent<EnemyHealthManager>().changeHealth(-3f);
            other.GetComponent<Ai1>().StopCoroutine("delaySwitchToAttack");
            if (Random.Range(0, 2) == 1)
            {
                other.GetComponent<Ai1>().StartCoroutine("delaySwitchToAttack");
            }
            else
            {
                other.GetComponent<Ai1>().StartCoroutine("recover");
            }
        }
    }
}
