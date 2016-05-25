using UnityEngine;
using System.Collections;

//This script controls ranged attack collision and death

public class projectileDeath : MonoBehaviour {

	void Start () {
        StartCoroutine("attackFrame");
	}

    IEnumerator attackFrame() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //If ranged attack hits enemy, deal damage to it and delete hitbox
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Ai1>().currentState = Ai1.State.Chase;
            other.gameObject.GetComponent<EnemyHealthManager>().changeHealth(-1f);
            StopCoroutine("attackFrame");
            Destroy(gameObject);
        }
        //If ranged attack hits wall, delete hitbox
        if (other.gameObject.tag == "Wall") {
            StopCoroutine("attackFrame");
            Destroy(gameObject);
        }
    }
}
