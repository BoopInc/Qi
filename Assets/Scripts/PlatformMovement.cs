using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlatformMovement : MonoBehaviour {
    [Header("Platform Details")]
    public List<Vector2> checkPoints = new List<Vector2>();
    public int startPoint;
    public int point;
    public float pause;
    public float speed;
    public bool reverse;

    private float direction;
    private int maxPoints;
    private bool atPoint;

	void Start () {
        //Set platform details to default
        point = 0;
        direction = 1;
        checkPoints[0] = new Vector2(transform.position.x,transform.position.y);
        maxPoints = checkPoints.Count;
        atPoint = false;
    }
	
	void Update () {
        //If platform is not at a point, move platform
        if (!atPoint)
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), checkPoints[point], speed);

        //If platform is at a point pause then move
        if (new Vector2(transform.position.x, transform.position.y) == checkPoints[point] && !atPoint)
        {
            atPoint = true;
            StartCoroutine("checkPointPause");
        }

	}

    IEnumerator checkPointPause() {
        yield return new WaitForSeconds(pause);
        atPoint = false;

        //Change target point based on type of platform movement selected in inspector
        if (!reverse)
        {
            point++;

            if (point >= maxPoints)
                point = 0;
        }
        else
        {
            if (direction == 1)
            {
                point++;

                if (point >= maxPoints)
                {
                    direction *= -1;
                    point--;
                }
            }
            else if(direction == -1) {
                point--;

                if (point == 0)
                {
                    direction *= -1;
                }
            }
        }
    }
}
