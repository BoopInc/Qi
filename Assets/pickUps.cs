using UnityEngine;
using System.Collections;

public class pickUps : MonoBehaviour {

    private StatManager myStats;

	// Use this for initialization
	void Start () {
        myStats = GetComponent<StatManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "health orb") {
            myStats.changeHealth(1);
            Destroy(other.gameObject);
        }
    }
}
