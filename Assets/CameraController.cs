﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector2 velocity;
    public bool inBounds;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private GameObject player;
    public float smoothTimeX,smoothTimeY;
    public float xSmoothX, xSmoothY;
    public float targetZoom;
    public Vector3 targetPos;
    public bool lockCam;
    public float zoomSpeed;
    public float goToLockSpeed;
    public float leaveZoomSpeed;
    public float ResetCameraTime;
    public bool reset;

    public float startSmootTimeX, StartSmoothTimeY;
    public float startXSmoothX, StartXSmoothY;
    public float startZoom;

	// Use this for initialization
	void Start () {
        Camera cam = Camera.main;
        circleCollider = GetComponent<CircleCollider2D>() as CircleCollider2D;
        float height = 2f * cam.orthographicSize;

        targetZoom = 0.6f;
        circleCollider.radius = height / 5;
        //Preset values to default to
        startSmootTimeX = smoothTimeX;
        StartSmoothTimeY = smoothTimeY;
        startXSmoothX = xSmoothX;
        StartXSmoothY = xSmoothY;
        startZoom = 0.6f;
        lockCam = false;
        reset = false;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
       

       
    }

    void FixedUpdate()
    {
        Camera cam = Camera.main;
        cam.orthographicSize = Mathf.SmoothStep(cam.orthographicSize, targetZoom, zoomSpeed);
        if (lockCam)
            cam.transform.position = new Vector3(Mathf.SmoothStep(cam.transform.position.x, targetPos.x, goToLockSpeed), Mathf.SmoothStep(cam.transform.position.y, targetPos.y, goToLockSpeed), -10);

        if (reset)
        {
            xSmoothX = Mathf.SmoothStep(xSmoothX, startXSmoothX, ResetCameraTime);
            xSmoothY = Mathf.SmoothStep(xSmoothY, StartXSmoothY, ResetCameraTime);
            if (xSmoothX == startXSmoothX && xSmoothY == StartXSmoothY)
            {
                reset = false;
            }
        }
        if (!lockCam)
        {

            // GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
            float xPos = Mathf.SmoothDamp(cam.transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
            float yPos = Mathf.SmoothDamp(cam.transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
            cam.transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, xPos, xSmoothX), Mathf.SmoothStep(transform.position.y, yPos, xSmoothY), -10);
            //Camera.transform.position = new Vector3(xPos, yPos, -10);
        }
    }

}
