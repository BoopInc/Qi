﻿using UnityEngine;
using System.Collections;

public class SittingEnemyController : MonoBehaviour
{

    private float walkX, walkY;
    private bool stopWalking;
    private Vector2 myVel;
    private int randomDirectionX, randomDirectionY;
    private float myWidth;
    private float myHeight;
    private bool isBlockedXUp, isBlockedXDown, isBlockedYLeft, isBlockedYRight;

    public float agroWidth, agroHeight;
    private GameObject player;

    private Vector2 lineCastXUp;
    private Vector2 lineCastXDown;
    private Vector2 lineCastYLeft;
    private Vector2 lineCastYRight;

    [Header("Numbers")]
    public float maxWalkDistance;
    public float walkStopTime;
    public float speed;
    public LayerMask enemyMask;


    private enum State
    {
        Sit,
        Chase,
        Wander
    }

    State state;

    // Use this for initialization
    void Start()
    {
        stopWalking = false;
        myVel = GetComponent<Rigidbody2D>().velocity;
        state = State.Sit;

        myWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        myHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        lineCastYLeft = toVector2(transform.position) - toVector2(transform.up) * myHeight - toVector2(transform.right) * myWidth;
        lineCastYRight = toVector2(transform.position) - toVector2(transform.up) * myHeight + toVector2(transform.right) * myWidth;
        lineCastXUp = toVector2(transform.position) - toVector2(transform.right) * myWidth + toVector2(transform.up) * myHeight;
        lineCastXDown = toVector2(transform.position) - toVector2(transform.right) * myWidth - toVector2(transform.up) * myHeight;
        isBlockedXUp = Physics2D.Linecast(lineCastXUp, lineCastXUp - toVector2(transform.right) * .05f, enemyMask);
        isBlockedXDown = Physics2D.Linecast(lineCastXDown, lineCastXDown - toVector2(transform.right) * .05f, enemyMask);
        isBlockedYLeft = Physics2D.Linecast(lineCastYLeft, lineCastYLeft - toVector2(transform.up) * .05f, enemyMask);
        isBlockedYRight = Physics2D.Linecast(lineCastYRight, lineCastYRight - toVector2(transform.up) * .051f, enemyMask);
        Debug.DrawLine(lineCastXUp, lineCastXUp - toVector2(transform.right) * .05f);
        Debug.DrawLine(lineCastXDown, lineCastXDown - toVector2(transform.right) * .05f);
        Debug.DrawLine(lineCastYLeft, lineCastYLeft - toVector2(transform.up) * .05f);
        Debug.DrawLine(lineCastYRight, lineCastYRight - toVector2(transform.up) * .05f);

        myVel = GetComponent<Rigidbody2D>().velocity;

        updateStates();

        GetComponent<Rigidbody2D>().velocity = myVel;
    }

    void updateStates()
    {
        switch (state)
        {
            case State.Wander:
                Wander();
                break;
            case State.Sit:
                Sit();
                break;
            case State.Chase:
                Chase();
                break;
        }
    }

    void Wander()
    {
        if (isBlockedXDown && isBlockedXUp && randomDirectionY == 0)
        {
            turnAroundX();
        }
        else if (randomDirectionY == 0 && isBlockedXUp || randomDirectionY == 0 && isBlockedXDown)
        {
            turnAroundX();
        }

        if (isBlockedYLeft && isBlockedYRight && randomDirectionX == 0)
        {
            turnAroundY();
        }
        else if (randomDirectionX == 0 && isBlockedYLeft || randomDirectionX == 0 && isBlockedYRight)
        {
            turnAroundY();
        }

        print(randomDirectionX + " " + randomDirectionY);

        if (!stopWalking)
        {
            myVel = new Vector2(randomDirectionX * speed * transform.right.x, randomDirectionY * speed * transform.up.y);
        }
        else
        {
            myVel = new Vector2(0, 0);
        }
    }

    void Sit() {
        //Draw agro range;
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + agroWidth,transform.position.y,transform.position.z));
        Debug.DrawLine(transform.position, new Vector3(transform.position.x - agroWidth, transform.position.y, transform.position.z));
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + agroHeight, transform.position.z));
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - agroHeight, transform.position.z));

        //If in range switch to chase
        //PROTO
        if (player.transform.position.x <= transform.position.x + agroWidth && player.transform.position.x >= transform.position.x - agroWidth
            && player.transform.position.y <= transform.position.y + agroHeight && player.transform.position.y >= transform.position.y - agroHeight) {
            state = State.Chase;
        }

    }

    void Chase() {
        //Follow code
        /*
        if (player.transform.position.x > transform.position.x)
        {
            randomDirectionX = 1;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            randomDirectionX = -1;
        }
        else if (player.transform.position.x == transform.position.x) {
            randomDirectionX = 0;
        }

        if (player.transform.position.y > transform.position.y)
        {
            randomDirectionY = 1;
        }
        else if (player.transform.position.y < transform.position.y)
        {
            randomDirectionY = -1;
        }
        else if (player.transform.position.y == transform.position.y)
        {
            randomDirectionY = 0;
        }
        */
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position, .4f * Time.deltaTime);

        myVel = new Vector2(randomDirectionX * speed * transform.right.x, randomDirectionY * speed * transform.up.y);
    }

    IEnumerator WalkAround()
    {
        randomDirectionX = -1;
        randomDirectionY = -1;

        if (Random.Range(-1, 1) > -1)
        {
            turnAroundX();
        }

        if (Random.Range(-1, 1) > -1)
        {
            turnAroundY();
        }

        //if both are moving set one component to 0
        if (randomDirectionX != 0 && randomDirectionY != 0)
        {
            int leftRight = Random.Range(0, 2);
            if (leftRight == 0)
            {
                randomDirectionX = 0;
            }
            else
            {
                randomDirectionY = 0;
            }
        }

        float randomDistance = Random.Range(3.75f, maxWalkDistance);

        yield return new WaitForSeconds(randomDistance);

        stopWalking = true;
        yield return new WaitForSeconds(walkStopTime);
        stopWalking = false;

        StartCoroutine("WalkAround");
    }

    void turnAroundX()
    {
        //Flip player
        Vector3 currentRot = transform.eulerAngles;
        currentRot.y += 180;
        transform.eulerAngles = currentRot;
        //Flip ui
        //Vector3 hbRot = HealthBar.transform.eulerAngles;
        //Vector2 hPos = HealthBar.transform.position;
        // hbRot.y += 180;
        // HealthBar.transform.eulerAngles = hbRot;

    }

    void turnAroundY()
    {
        //Flip player
        Vector3 currentRot = transform.eulerAngles;
        currentRot.x += 180;
        transform.eulerAngles = currentRot;
        //Flip ui
        //Vector3 hbRot = HealthBar.transform.eulerAngles;
        //Vector2 hPos = HealthBar.transform.position;
        // hbRot.y += 180;
        // HealthBar.transform.eulerAngles = hbRot;

    }

    Vector2 toVector2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

}
