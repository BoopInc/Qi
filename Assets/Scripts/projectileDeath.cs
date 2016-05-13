using UnityEngine;
using System.Collections;

public class projectileDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("timer2");
	}

    IEnumerator timer2() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Ai1>().currentState = Ai1.State.Chase;
            other.gameObject.GetComponent<EnemyHealthManager>().changeHealth(-2f);
            StopCoroutine("timer2");
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Wall") {
            StopCoroutine("timer2");
            Destroy(gameObject);
        }
    }
}
