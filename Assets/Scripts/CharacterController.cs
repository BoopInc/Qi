using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    private float moveH,moveY;
    private float speedMultiplier;
    public int directionX, directionY;
    private StatManager myStats;

    //Dash
    private int tapCount = 0;
    private float tapCooler = 0.3f;
    private bool taped = false;
    private int lastTapKey = 0;

    [Header("Numbers")]
    public float speed;
    public float rollCost;

    public bool canMove;
    private bool canRoll;
    private bool canDash;
    private bool wasSprinting;
    private bool onStairsLeft;
    private bool onStairsRight;
    private bool onPlatform;
    private bool inWater;
    private bool dashStop;
    float angle = 0;
    Vector3 cursorPosition;

    private bool useFrameCounter = true;

    private float dashDistance;
    private float dashStartDistance;
    private Vector2 dashStart;
    private float dashTime;

	// Use this for initialization
	void Start () {
        myStats = GetComponent<StatManager>();

        //Bools
        canMove = true;
        canRoll = true;
        canDash = true;
        wasSprinting = false;
        onStairsLeft = false;
        onStairsRight = false;
        onPlatform = false;
        inWater = false;
        dashStop = false;

        speedMultiplier = 1f;
        tapCount = 0;
        directionX = 0;
        directionY = 0;
	}

    // Update is called once per frame
    void Update() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

            //Movement
            if (Input.GetKey(KeyCode.A))
            {
                moveH = -1;
                directionX = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveH = 1;
                directionX = 1;
            }
            else {
                moveH = 0;
                directionX = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                moveY = 1;
                directionY = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveY = -1;
                directionY = -1;
            }
            else
            {
                moveY = 0;
                directionY = 0;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                moveH = directionX * 0.7f;
                moveY = directionY * 0.7f;
            }
            else {
                moveH = directionX;
                moveY = directionY;
            }

        //Dodge
       
            doubleTap();
        
        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            canDash = false;
            // dashStart = new Vector2(transform.position.x,transform.position.y);
            dashStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dashDistance = 0;
            dashStop = false;

            //Cursor Distance
            dashStartDistance = Vector2.Distance(dashStart,Camera.main.ScreenToWorldPoint(Input.mousePosition));
            dashTime = (float) ((.1 *dashStartDistance) / (0.3719999));
            if (dashTime >= .1)
                dashTime = .1f;
            StartCoroutine("dash");
        }

        //If on stairs change y vel
        if (onStairsLeft) {
            if (directionX == -1) {
                moveY = -1;
            }
            if (directionX == 1)
            {
                moveY = 1;
            }

        }

        if (onStairsRight)
        {
            if (directionX == -1)
            {
                moveY = 1;
            }
            if (directionX == 1)
            {
                moveY = -1;
            }
        }
        //Water check
        if (inWater && !onPlatform) {
            moveH *= .5f;
            moveY *= .5f;
        }

        //Calculate velocity with cases
        if (canMove)
        {
            rb.velocity = new Vector2(moveH * speed * speedMultiplier, moveY * speed * speedMultiplier);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (!canRoll)
        {
            //Ignore collision with enemies
            Physics2D.IgnoreLayerCollision(9, 10);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }
        else {
            Physics2D.IgnoreLayerCollision(9, 10,false);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }

        if (!canDash)
        {
            //Ignore collision with enemies
            Physics2D.IgnoreLayerCollision(9, 10);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 10, false);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }

        if (!canDash || !canRoll)
        {
            Physics2D.IgnoreLayerCollision(9, 17);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }
        else {
            Physics2D.IgnoreLayerCollision(9, 17, false);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }

        //Prediction
        //Vector2 predictedPosition = new Vector2(transform.position.x, transform.position.y) + Time.deltaTime * rb.velocity;
        Vector2 predictedPosition = CalcFuturePos(1);
        //if is dashing
        /*if (!canDash) {
            speedMultiplier = 3.10f;
            
             if (!dashStop)
             {
                 cursorPosition = Input.mousePosition;
                 cursorPosition.z = 0;

                 Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
                 cursorPosition.x = cursorPosition.x - objectPos.x;
                 cursorPosition.y = cursorPosition.y - objectPos.y;
                 angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x);
                 dashStop = true;
             }


             GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * speedMultiplier, Mathf.Sin(angle) * speedMultiplier);
             
            if (dashDistance >= 0.37  || dashDistance >= dashStartDistance) {
                StartCoroutine("staminaRegen");
                StopCoroutine("dash");
                StartCoroutine("movementPause");
                speedMultiplier = 0;
                dashStop = true;
            }
      
            transform.position = Vector2.MoveTowards(transform.position, dashStart,.1f);
         

        }
        */
        dashDistance = Vector2.Distance(dashStart, new Vector2(transform.position.x, transform.position.y));
    }

    void FixedUpdate() {
        //If you are dashing
        if (!canDash) {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position,dashStart,.1f));
            if (new Vector2(transform.position.x, transform.position.y) == dashStart)
            {
                StartCoroutine("staminaRegen");
                StopCoroutine("dash");
                StartCoroutine("movementPause");
                dashStop = true;
            }
        }
    }

    Vector2 CalcFuturePos(float i) {
        return new Vector2(transform.position.x, transform.position.y) + GetComponent<Rigidbody2D>().velocity * Time.deltaTime;
    }

    void doubleTap()
    {
        if (Input.GetKeyDown(KeyCode.W) && taped == false)
        {
            taped = true;
            lastTapKey = 1;
        }
        else if (Input.GetKeyDown(KeyCode.W) && taped == true)
        {
            if (lastTapKey == 1)
            {
                taped = true;
                tapCount = 2;
            }
            else
            {
                tapCooler = 0.3f;
                tapCount = 0;
                taped = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && taped == false)
        {
            taped = true;
            lastTapKey = 2;
        }
        else if (Input.GetKeyDown(KeyCode.D) && taped == true)
        {
            if (lastTapKey == 2)
            {
                taped = true;
                tapCount = 2;
            }
            else
            {
                tapCooler = 0.3f;
                tapCount = 0;
                taped = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && taped == false)
        {
            taped = true;
            lastTapKey = 3;
        }
        else if (Input.GetKeyDown(KeyCode.S) && taped == true)
        {
            if (lastTapKey == 3)
            {
                taped = true;
                tapCount = 2;
            }
            else
            {
                tapCooler = 0.3f;
                tapCount = 0;
                taped = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && taped == false)
        {
            taped = true;
            lastTapKey = 4;
        }
        else if (Input.GetKeyDown(KeyCode.A) && taped == true)
        {
            if (lastTapKey == 4)
            {
                taped = true;
                tapCount = 2;
            }
            else
            {
                tapCooler = 0.3f;
                tapCount = 0;
                taped = false;
            }
        }

        if (taped)
        {
            if (tapCooler > 0)
            {
                tapCooler -= 1 * Time.deltaTime;
            }
        }
        else
        {
            tapCooler = 0.3f;
        }

        if (tapCount >= 2 && taped)
        {
            StartCoroutine("dodge");
            tapCount = 0;
            taped = false;
            tapCooler = 0.3f;
        }

        if (tapCooler < 0)
        {
            if (tapCount >= 2)
            {
                StartCoroutine("dodge");
            }
            tapCount = 0;
            taped = false;
            tapCooler = 0.3f;
        }
    }

    IEnumerator sprintChargeUp() {
        bool heldDown = true;

        for (int i = 0; i < 2; i++) {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                yield return new WaitForSeconds(.25f);
            }
            else {
                heldDown = false;
                canMove = true;
                wasSprinting = false; ;
                myStats.regenStamina = true;
                speedMultiplier = 1f;
                break;
            }
        }

        if (heldDown && Input.GetKey(KeyCode.LeftShift))
        {
            wasSprinting = true;
            speedMultiplier = 1.77f;
            canMove = true;
        }
        else {
            canMove = true;
            wasSprinting = false; ;
        }

    }

    IEnumerator dodge() {
        StopCoroutine("staminaRegen");
        myStats.regenStamina = false;
        GetComponent<StatManager>().changeStamina(-rollCost);

        canRoll = false;
        speedMultiplier = 4.10f;
        yield return new WaitForSeconds(.04f);
        speedMultiplier = 1f;
        canRoll = true;

        //Start stamina regen after 1 second
        StartCoroutine("staminaRegen");
    }

    IEnumerator movementPause() {
        speedMultiplier = 0;
        yield return new WaitForSeconds(.2f);
        canDash = true;
        speedMultiplier = 1f;
        //Start stamina regen after 1 second
        StartCoroutine("staminaRegen");
    }

    IEnumerator dash() {
        canDash = false;
        //get distance of cursor
        yield return new WaitForSeconds(.075f);
        canDash = true;
        speedMultiplier = 0f;
        print(dashDistance + ", " + dashStartDistance);
        yield return new WaitForSeconds(.2f);
        speedMultiplier = 1f;
        dashStop = false;

        //Start stamina regen after 1 second
        StartCoroutine("staminaRegen");
    }

    IEnumerator staminaRegen() {
        yield return new WaitForSeconds(1f);
        myStats.regenStamina = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall") { 
        
        }
       // if (other.gameObject.tag == "Enemy") {
         //   GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //}
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "StairLeft") {
            onStairsLeft = true;
        }
        if (other.gameObject.tag == "StairRight")
        {
            onStairsRight = true;
        }
        if (other.gameObject.tag == "PlatformArea")
        {
            onPlatform = true;
        }
        if (other.gameObject.tag == "Water")
        {
            inWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "StairLeft")
        {
            onStairsLeft = false;
        }
        if (other.gameObject.tag == "StairRight")
        {
            onStairsRight = false;
        }
        if (other.gameObject.tag == "PlatformArea")
        {
            onPlatform = false;
        }
        if (other.gameObject.tag == "Water")
        {
            inWater = false;
        }
    }
}
