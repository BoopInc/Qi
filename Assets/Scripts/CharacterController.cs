using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    private float moveH,moveY;
    private float speedMultiplier;
    public int directionX, directionY;
    private StatManager myStats;
    private Transform beginningParent;

    private float onObjectHeight;

    //Conditions
    private bool onObject;
    private bool inDropArea;
    private bool isFalling;
    private bool inClimbArea;
    private bool isClimbing;
    //Dash
    private int tapCount = 0;
    private float tapCooler = 0.3f;
    private bool taped = false;
    private int lastTapKey = 0;

    [Header("Numbers")]
    public float speed;
    public float rollCost;
    public float dashCost;

    private bool canJump;
    public bool canMove;
    private bool canRoll;
    public bool canDash;
    private bool wasSprinting;
    private bool onStairsLeft;
    private bool onStairsRight;
    private bool onPlatform;
    private bool onMovingPlatform;
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
        canJump = true;
        onObject = false;
        onMovingPlatform = false;
        inDropArea = false;
        isFalling = false;
        inClimbArea = false;
        isClimbing = false;

        speedMultiplier = 1f;
        tapCount = 0;
        directionX = 0;
        directionY = 0;

        beginningParent = transform.parent;
	}

    // Update is called once per frame
    void Update() {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (!isClimbing || !isFalling)
        {
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
            else
            {
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
        }else
        {
            moveY = 0;
            moveH = 0;
            directionX = 0;
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
            if(Input.GetKeyDown(KeyCode.Space) && canRoll && GetComponent<StatManager>().getStamina() >= rollCost)
            {
                StartCoroutine("dodge");
                canRoll = false;
            }
        //doubleTap();

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && GetComponent<StatManager>().getStamina() >= dashCost) {
            canDash = false;
            // dashStart = new Vector2(transform.position.x,transform.position.y);
            dashStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dashDistance = 0;
            dashStop = false;

            //Cursor Distance
            dashStartDistance = Vector2.Distance(dashStart,Camera.main.ScreenToWorldPoint(Input.mousePosition));
            dashTime = (float) ((.15 *dashStartDistance) / (0.3719999));
            if (dashTime >= .15)
                dashTime = .15f;
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

            Physics2D.IgnoreLayerCollision(9, 10);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;

            Physics2D.IgnoreLayerCollision(9, 17);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }
        else {
            Physics2D.IgnoreLayerCollision(9, 10,false);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;

            Physics2D.IgnoreLayerCollision(9, 17, false);
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

        if (isFalling || isClimbing) {
            Physics2D.IgnoreLayerCollision(9, 11);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }else
        {
            Physics2D.IgnoreLayerCollision(9, 11, false);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        }
     
        dashDistance = Vector2.Distance(dashStart, new Vector2(transform.position.x, transform.position.y));
        Interactables();

        if (onStairsLeft && canDash == false)
        {
            StopCoroutine("dash");
            StartCoroutine("movementPause");
            canDash = true;
            dashStop = true;
            speedMultiplier = 0; 
        }
       

        if (onStairsRight && canDash == false)
        {
            StopCoroutine("dash");
            StartCoroutine("movementPause");
            canDash = true;
            dashStop = true;
            speedMultiplier = 0;
        }
    }

    void FixedUpdate() {
        //If you are dashing
        if (!canDash && dashStop == false) {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position,dashStart,.07f));
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

    void Interactables()
    {
        if (inDropArea && Input.GetKeyDown(KeyCode.LeftControl)) {
            isFalling = true;
        }

        if(isFalling)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,-1.2f);
        }

        if(inClimbArea && !isFalling && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isClimbing = true;
        }

        if(isClimbing)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0.6f);
        }
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

    IEnumerator jump() {
        yield return new WaitForSeconds(0f);
        canJump = true;
    }

    IEnumerator dodge() {
        StopCoroutine("staminaRegen");
        myStats.regenStamina = false;
        GetComponent<StatManager>().changeStamina(-rollCost);
        canRoll = false;
        speedMultiplier = 4.60f;
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
        StopCoroutine("staminaRegen");
        GetComponent<StatManager>().changeStamina(-dashCost);
        canDash = false;
        //get distance of cursor
        yield return new WaitForSeconds(.1f);
        canDash = true;
        speedMultiplier = 0f;
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
        if (other.gameObject.tag == "MovingPlatform") {
            transform.parent = other.transform.parent;
        }
        if (other.tag == "Camera")
        {
            GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraController>().inBounds = true;
        }
        if (other.tag == "Camera Zone") {
            GameObject cam = GameObject.FindGameObjectWithTag("Camera");
            cam.GetComponent<CameraController>().smoothTimeX = other.GetComponent<CameraZone>().SmoothTimeX;
            cam.GetComponent<CameraController>().smoothTimeY = other.GetComponent<CameraZone>().SmoothTimeY;
            cam.GetComponent<CameraController>().xSmoothX = other.GetComponent<CameraZone>().xSmoothX;
            cam.GetComponent<CameraController>().xSmoothY = other.GetComponent<CameraZone>().xSmoothY;
            cam.GetComponent<CameraController>().targetZoom = other.GetComponent<CameraZone>().zoom;
            cam.GetComponent<CameraController>().lockCam = other.GetComponent<CameraZone>().lockCam;
            cam.GetComponent<CameraController>().targetPos = other.GetComponent<CameraZone>().pos;
            cam.GetComponent<CameraController>().zoomSpeed = other.GetComponent<CameraZone>().zoomSpeed;
            cam.GetComponent<CameraController>().goToLockSpeed = other.GetComponent<CameraZone>().goToLockSpeed;
            cam.GetComponent<CameraController>().reset = false;
        }
        if(other.tag == "Drop Area")
        {
            inDropArea = true;
        }
        if (other.tag == "Climb Area")
        {
            inClimbArea = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "StairLeft")
        {
            onStairsLeft = true;
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
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = beginningParent;
        }
        if (other.tag == "Camera")
        {
            GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraController>().inBounds = false;
        }
        if (other.tag == "Camera Zone")
        {
            GameObject cam = GameObject.FindGameObjectWithTag("Camera");
            cam.GetComponent<CameraController>().smoothTimeX = cam.GetComponent<CameraController>().startSmootTimeX;
            cam.GetComponent<CameraController>().smoothTimeY = cam.GetComponent<CameraController>().StartSmoothTimeY;
            cam.GetComponent<CameraController>().xSmoothX = other.GetComponent<CameraZone>().leaveZoomSpeed;
            cam.GetComponent<CameraController>().xSmoothY = cam.GetComponent<CameraController>().StartXSmoothY;
            cam.GetComponent<CameraController>().targetZoom = 0.6f;
            cam.GetComponent<CameraController>().lockCam = false;
            cam.GetComponent<CameraController>().zoomSpeed = other.GetComponent<CameraZone>().zoomSpeed;
            cam.GetComponent<CameraController>().goToLockSpeed = other.GetComponent<CameraZone>().goToLockSpeed;
            cam.GetComponent<CameraController>().reset = true;
        }
        if (other.tag == "Drop Area")
        {
            if(isFalling)
                StartCoroutine("movementPause");

            inDropArea = false;
            isFalling = false;
        }
        if (other.tag == "Climb Area")
        {
            if(isClimbing)
                StartCoroutine("movementPause");

            inClimbArea = false;
            isClimbing = false;
        }

    }
}
