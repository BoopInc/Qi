using UnityEngine;
using System.Collections;

//This class controls enemy attack hitbox times

public class EnemyFieldController : MonoBehaviour {

	void Start () {
        StartCoroutine("attackFrame");
	}
	
    IEnumerator attackFrame() {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        //If hitbox hits player than damage player and delete hitbox
        if (other.tag == "Player")
        {
            other.GetComponent<StatManager>().changeHealth(-GetComponent<AttackProperties>().damage);
            Destroy(gameObject);
        }
    }
}
