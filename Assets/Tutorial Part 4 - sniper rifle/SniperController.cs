using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperController : MonoBehaviour
{
    //The bullet we will fire
    public GameObject sniperBullet;

    //Camera stuff
    private Camera mainCamera;
    //Need the initial camera FOV so we can zoom
    private float initialFOV;
    //Different zoom levels we can have zoom
    private int currentZoom = 1;

    //To change sensitivity when zoomed
    private BodyController bodyController;

    //Used so we can only fire one bullet when pressing a key
    private bool canFire = true;



    void Start()
    {
        bodyController = GetComponent<BodyController>();

        mainCamera = bodyController.cameraTrans.GetComponent<Camera>();

        initialFOV = mainCamera.fieldOfView;

        StartCoroutine(FireBulletAllTheTime());
    }

    

    void Update()
    {
        ZoomSight();

        FireBullet();
    }



    //The sniper rifle can zoom between 3 and 12 times
    void ZoomSight()
    {
        //Remove the scope sight if we are not zooming
        if (currentZoom == 1)
        {
            //sightImage.SetActive(false);

            //Change sensitivity
            bodyController.useSlowSensitivity = false;
        }
        else
        {
            //sightImage.SetActive(true);

            //Change sensitivity
            bodyController.useSlowSensitivity = true;
        }

        //Zoom with mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentZoom += 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentZoom -= 1;
        }

        //Clamp zoom
        //Keep it between 1 and 11, then add 1 when zoom because zoom is between 3 and 12 times
        currentZoom = Mathf.Clamp(currentZoom, 1, 11);

        //No zoom
        if (currentZoom == 1)
        {
            //If the zoom is 6x, then the FOV is FOV / 6 (according to unscientific research on Internet)
            mainCamera.fieldOfView = initialFOV / (float)currentZoom;
        }
        //Zoom the sight
        else
        {
            mainCamera.fieldOfView = initialFOV / ((float)currentZoom + 1f);
        }
    }



    void FireBullet()
    {
        //Sometimes when we move the mouse is really high in the webplayer, so the mouse cursor ends up outside
        //of the webplayer so we cant fire, despite locking the cursor, so add alternative fire button
        if ((Input.GetKeyDown(KeyCode.F)) && canFire)
        {
            //Create a new bullet
            GameObject newBullet = Instantiate(sniperBullet) as GameObject;

            //Give it speed and position
            Vector3 startPos = mainCamera.transform.position;

            Vector3 startDir = mainCamera.transform.forward;

            newBullet.GetComponent<MoveBullet>().SetStartValues(startPos, startDir);

            canFire = false;
        }

        //Has to release the trigger to fire again
        if (Input.GetKeyUp(KeyCode.F))
        {
            canFire = true;
        }
    }



    //Fire bullets continuously
    public IEnumerator FireBulletAllTheTime()
    {
        while (true)
        {
            //Create a new bullet at the base of the cannon barrel
            GameObject newBullet = Instantiate(sniperBullet) as GameObject;

            //Parent it to get a less messy workspace
            //newBullet.transform.parent = bulletsParent;

            //The start speed vector
            //Vector3 startSpeedVec = bulletData.muzzleVelocity * transform.forward;

            //Add velocity to the bullet with a rigidbody
            //newBullet.GetComponent<Rigidbody>().velocity = startSpeedVec;

            //Give it speed and position
            Vector3 startPos = mainCamera.transform.position;

            Vector3 startDir = mainCamera.transform.forward;

            //Add start values to the bullet that has no rigidbody
            newBullet.GetComponent<MoveBullet>().SetStartValues(startPos, startDir);

            //Wait 2 seconds until we fire another bullet
            yield return new WaitForSeconds(2f);
        }
    }
}
