using UnityEngine;
using System.Collections;



public class SniperController : MonoBehaviour 
{
    //Drags
    public Camera mainCamera;
    public GameObject sightImage;
    public GameObject bulletObj;
    public Transform targetObj;
    public Transform mirrorTargetObj;
    public GameObject hitMarker;
    
    //The bullet's initial speed
    //Sniper rifle
    public float bulletSpeed = 850f;

    //Need the initial camera FOV so we can zoom
    float initialFOV;
    //To change the zoom
    int currentZoom = 1;

    //To change sensitivity when zoomed
    //MouseLook mouseLook;
    float standardSensitivity;
    float zoomSensitivity = 0.1f;

    bool canFire = true;

    public static Vector3 windSpeed = new Vector3(2f, 0f, 3f);



	void Start() 
	{
        //Lock and hide the mouse cursor 
        UnityEngine.Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        initialFOV = mainCamera.fieldOfView;

        //mouseLook = GetComponent<RigidbodyFirstPersonController>().mouseLook;

        //standardSensitivity = mouseLook.XSensitivity;
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
            sightImage.SetActive(false);

            //Change sensitivity
            //mouseLook.XSensitivity = standardSensitivity;
            //mouseLook.YSensitivity = standardSensitivity;
        }
        else 
        {
            sightImage.SetActive(true);

            //Change sensitivity
            //mouseLook.XSensitivity = zoomSensitivity;
            //mouseLook.YSensitivity = zoomSensitivity;
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
        //Keep it between 1 and 11, then add 1 when zoom because zoom is between 3 and 12
        currentZoom = Mathf.Clamp(currentZoom, 1, 11);

        //No zoom
        if (currentZoom == 1)
        {
            //If the zoom is 6x, then the FOV is FOV / 6 (according to unscientific resaerch on Internet)
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
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)) && canFire)
        {
            //Create a new bullet
            GameObject newBullet = Instantiate(bulletObj, mainCamera.transform.position, mainCamera.transform.rotation) as GameObject;

            //Give it speed
            newBullet.GetComponent<TutorialBullet>().currentVelocity = bulletSpeed * mainCamera.transform.forward;

            canFire = false;
        }

        //Has to release the trigger to fire again
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.F))
        {
            canFire = true;
        }
    }



    //Add marker where we hit target
    //Called from the bullet script
    public void AddMarker(Vector3 hitCoordinates) 
    {
        //Add a marker where we hit the target
        Instantiate(hitMarker, hitCoordinates, Quaternion.identity);

        //The coordinates of the hit in localPosition of the target
        Vector3 localHitCoordinates = targetObj.InverseTransformPoint(hitCoordinates);

        //The global coordinates of the hit but in relation to the mirror target
        //The marker has the same local position in relation to both the target and the mirror
        Vector3 globalMirrorHit = mirrorTargetObj.transform.TransformPoint(localHitCoordinates);

        //Add another marker
        Instantiate(hitMarker, globalMirrorHit, Quaternion.identity);
    }
}
