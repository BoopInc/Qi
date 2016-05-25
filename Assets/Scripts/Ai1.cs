using UnityEngine;
using System.Collections;

public class Ai1 : MonoBehaviour {

    private Vector3 Player;
    private Vector2 PlayerDirection;
    private float Xdif;
    private float Ydif;
    private int Wall;
    private float distance;
    private bool stun;
    private float stuntime;
    private int directionX, directionY;
    private bool isBlockedXUp, isBlockedXDown, isBlockedYLeft, isBlockedYRight;
    private bool stopWalking;
    private float myWidth;
    private float myHeight;
    public LayerMask enemyMask;

    private Vector2 lineCastXUp;
    private Vector2 lineCastXDown;
    private Vector2 lineCastYLeft;
    private Vector2 lineCastYRight;
    private Quaternion healthRot;

    [Header("Stats")]
    public float speed;
    public float xAgro;
    public float yAgro;
    public float xAAgro;
    public float yAAgro;
    public GameObject HealthBar;
    public GameObject AttackField;
    public float chaseSpeed;

    public enum State
    {
        Sit,
        Chase,
        Wander,
        nothing,
        Attack
    }

    public State currentState;

    void Start() {
        currentState = State.Wander;
        StartCoroutine("WanderCR");
        stopWalking = false;
        stuntime = 0;
        stun = false;
        Wall = 1 << 11;
        healthRot = HealthBar.transform.rotation;
    }

	// Update is called once per frame
	void Update () {
        updateStates();

        //Setting
        HealthBar.transform.position = transform.position + new Vector3(0, .06f, 0);
        HealthBar.transform.rotation = healthRot;

    }

    void updateStates()
    {
        switch (currentState)
        {
            case State.Sit:
                Sit();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Wander:
                Wander();
                break;
            case State.nothing:
                doNothing();
                break;
        }

        print(currentState);
    }
    void Sit() {
        Xdif = Player.x - transform.position.x;
        Ydif = Player.y - transform.position.y;
        PlayerDirection = new Vector2(Xdif, Ydif);

        myWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        myHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        Player = GameObject.FindGameObjectWithTag("Player").transform.position;
        distance = Vector2.Distance(Player, transform.position);

        //If player is in range, chase them
        if (Player.x <= transform.position.x + xAgro && Player.x >= transform.position.x - xAgro
            && Player.y <= transform.position.y + yAgro && Player.y >= transform.position.y - yAgro && !Physics2D.Raycast(transform.position, PlayerDirection, .1f, Wall))
        { 
            currentState = State.Chase;
        }
    }

    void Chase() {
        Player = GameObject.FindGameObjectWithTag("Player").transform.position;
        distance = Vector2.Distance(Player, transform.position);

        if (stuntime > 0)
        {
            stuntime -= Time.deltaTime;
        }
        else
        {
            stun = false;
        }

        if (stun == false)
        {
            Xdif = Player.x - transform.position.x;
            Ydif = Player.y - transform.position.y;
            PlayerDirection = new Vector2(Xdif, Ydif);

            //Move towards player if not stunned
            if (!Physics2D.Raycast(transform.position, PlayerDirection, .1f, Wall))
            {
                GetComponent<Rigidbody2D>().AddForce(PlayerDirection.normalized * chaseSpeed);
            }
            else {
                currentState = State.Wander;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }


        Debug.DrawLine(transform.position, PlayerDirection * .1f);


        /*if (GetComponent<Rigidbody2D>().velocity.x == 0 && GetComponent<Rigidbody2D>().velocity.y == 0
            && Physics2D.Raycast(transform.position, PlayerDirection, .1f, Wall))
        {
            currentState = State.nothing;
        }
        */

       /*  if (distance > .6f && !Physics2D.Raycast(transform.position, PlayerDirection, .03f, Wall))
         {
             currentState = State.Wander;
             StartCoroutine("WanderCR");

         }*/

        if (distance > .6f && Physics2D.Raycast(transform.position, PlayerDirection, .01f, Wall)) {
            currentState = State.nothing;
            StartCoroutine("delaySwitchToWander2");
        }

        if (Player.x <= transform.position.x + xAAgro && Player.x >= transform.position.x - xAAgro
           && Player.y <= transform.position.y + yAAgro && Player.y >= transform.position.y - yAAgro && !Physics2D.Raycast(transform.position, PlayerDirection, .1f, Wall))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            StartCoroutine("delaySwitchToAttack");
            currentState = State.Attack;
        }
    }

    void Attack() {

    }

    void doNothing() {

    }

    void Wander()
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

        Vector2 myVel = GetComponent<Rigidbody2D>().velocity;

        if (isBlockedXDown && isBlockedXUp && directionY == 0)
        {
            turnAroundX();
        }
        else if (directionY == 0 && isBlockedXUp || directionY == 0 && isBlockedXDown)
        {
            turnAroundX();
        }

        if (isBlockedYLeft && isBlockedYRight && directionX == 0)
        {
            turnAroundY();
        }
        else if (directionX == 0 && isBlockedYLeft || directionX == 0 && isBlockedYRight)
        {
            turnAroundY();
        }

        if (!stopWalking)
        {
            myVel = new Vector2(directionX * speed * transform.right.x * .1f, directionY * speed * transform.up.y * .1f);
        }
        else
        {
            myVel = new Vector2(0, 0);
        }

        Xdif = Player.x - transform.position.x;
        Ydif = Player.y - transform.position.y;
        PlayerDirection = new Vector2(Xdif, Ydif);

        distance = Vector2.Distance(Player, transform.position);
        Player = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (Player.x <= transform.position.x + xAgro && Player.x >= transform.position.x - xAgro
            && Player.y <= transform.position.y + yAgro && Player.y >= transform.position.y - yAgro && !Physics2D.Raycast(transform.position, PlayerDirection, .1f, Wall))
        {
            currentState = State.Chase;
        }

        GetComponent<Rigidbody2D>().velocity = myVel;
    }

    IEnumerator WanderCR()
    {
        directionX = -1;
        directionY = -1;

        if (Random.Range(-1, 1) > -1)
        {
            turnAroundX();
        }

        if (Random.Range(-1, 1) > -1)
        {
            turnAroundY();
        }

        //if both are moving set one component to 0
        if (directionX != 0 && directionY != 0)
        {
            int leftRight = Random.Range(0, 2);
            if (leftRight == 0)
            {
                directionX = 0;
            }
            else
            {
                directionY = 0;
            }
        }

        float randomDistance = Random.Range(3.75f,5);

        yield return new WaitForSeconds(randomDistance);

        stopWalking = true;
        yield return new WaitForSeconds(.5f);
        stopWalking = false;

        StartCoroutine("WanderCR");
    }

    void WalkAroundSkip()
    {
        directionX = -1;
        directionY = -1;

        if (Random.Range(-1, 1) > -1)
        {
            turnAroundX();
        }

        if (Random.Range(-1, 1) > -1)
        {
            turnAroundY();
        }

        //if both are moving set one component to 0
        if (directionX != 0 && directionY != 0)
        {
            int leftRight = Random.Range(0, 2);
            if (leftRight == 0)
            {
                directionX = 0;
            }
            else
            {
                directionY = 0;
            }
        }

        stopWalking = false;
        StartCoroutine("WanderCR");
    }

    void turnAroundX()
    {
        //Flip player
        Vector3 currentRot = transform.eulerAngles;
        currentRot.y += 180;
        transform.eulerAngles = currentRot;
        HealthBar.transform.position = transform.position + new Vector3(0, .06f, 0);
        HealthBar.transform.rotation = healthRot;
    }

    void turnAroundY()
    {
        //Flip player
        Vector3 currentRot = transform.eulerAngles;
        currentRot.x += 180;
        transform.eulerAngles = currentRot;
        HealthBar.transform.position = transform.position + new Vector3(0, .06f, 0);
        HealthBar.transform.rotation = healthRot;
    }

    Vector2 toVector2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    IEnumerator delaySwitchToWander() {
        distance = Vector2.Distance(Player, transform.position);
        Player = GameObject.FindGameObjectWithTag("Player").transform.position;

        yield return new WaitForSeconds(1f);

        if (Physics2D.Raycast(transform.position, PlayerDirection, 100f, Wall)) {
            currentState = State.Wander;
            StopCoroutine("WalkAround");
            WalkAroundSkip();
        }

    }

    public IEnumerator delaySwitchToAttack()
    {
        //Tilt


        Vector2 PlayerPosition;
        PlayerPosition.x = Player.x - transform.position.x;
        PlayerPosition.y = Player.y - transform.position.y;

        float angle = Mathf.Atan2(PlayerPosition.y, PlayerPosition.x) * (180 / Mathf.PI);
        yield return new WaitForSeconds(.34f);


        Instantiate(AttackField,transform.position, Quaternion.Euler(new Vector3(0, 0, angle - 90)));

        yield return new WaitForSeconds(.6f);

        currentState = State.Chase;
    }

    public IEnumerator delaySwitchToWander2() {
        yield return new WaitForSeconds(2);
        currentState = State.Wander;
        StartCoroutine("WanderCR");
    }

    public IEnumerator recover() {
        yield return new WaitForSeconds(.20f);
        currentState = State.Chase;
    }

    IEnumerator resetShoot() {
        yield return new WaitForSeconds(1f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            stun = true;
            stuntime = 1;
        }
    }

}
