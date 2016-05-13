using UnityEngine;
using System.Collections;

public class collideWithPlayer : MonoBehaviour {

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerAlt")
        {
            GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
