using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public Vector3 cursorPosition;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;

        transform.position = cursorPosition;
    }
}
