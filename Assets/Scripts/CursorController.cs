using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public Vector3 cursorPosition;

	void Start () {
        //Get rid of windows cursor
        Cursor.visible = false;
	}
	
	void Update () {
        //Update cursor position
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;

        //Move cursor object to cursor
        transform.position = cursorPosition;
    }
}
