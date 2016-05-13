using UnityEngine;
using System.Collections;

public class MeleeController : MonoBehaviour {

    private GameObject player;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        //Tilt
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        cursorPosition.x = cursorPosition.x - objectPos.x;
        cursorPosition.y = cursorPosition.y - objectPos.y;

        float angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x) * (180 / Mathf.PI);

        CharacterController cc = player.GetComponent<CharacterController>();
        //float angle = Mathf.Atan2(cc.directionY, cc.directionX) * (180 / Mathf.PI);
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        StartCoroutine("timer2");
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    IEnumerator timer2() {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyHealthManager>().changeHealth(-3f);
        }
    }
}
