using UnityEngine;
using System.Collections;

public class EnemyFieldController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("timer");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator timer() {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            print("Hit player");
            //Damage Player
            other.GetComponent<StatManager>().changeHealth(-GetComponent<AttackProperties>().damage);
            Destroy(gameObject);
        }
    }
}
