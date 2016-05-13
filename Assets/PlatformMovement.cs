using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlatformMovement : MonoBehaviour {
    [Header("Platform Details")]
    public List<Vector2> checkPoints = new List<Vector2>();
    public int startPoint;
    public int point;
    public float pause;
    public bool reverse;

    private float direction;
    private int maxPoints;
    private bool atPoint;

	// Use this for initialization
	void Start () {
        point = 0;
        checkPoints[0] = new Vector2(transform.position.x,transform.position.y);
        atPoint = false;
        maxPoints = checkPoints.Count;
        direction = 1;
	}
	
	// Update is called once per frame
	void Update () {
        //Update list
       
        if (!atPoint)
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), checkPoints[point], .1f);

        //If at a checkpoint go to the next one
        if (new Vector2(transform.position.x, transform.position.y) == checkPoints[point] && !atPoint)
        {
            atPoint = true;
            StartCoroutine("checkPointPause");
            print("at point");
        }
        print(atPoint);
	}

    IEnumerator checkPointPause() {
        yield return new WaitForSeconds(pause);
    
        atPoint = false;
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
