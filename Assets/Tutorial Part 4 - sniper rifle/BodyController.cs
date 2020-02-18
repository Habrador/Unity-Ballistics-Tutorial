using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Will just move the sniper character (who cant walk just turn around and aim up/down)
public class BodyController : MonoBehaviour
{
    //The camera should be a child to this gameobject
    //Should not be at the top of the capsule because it may clip through if hitting something above
    public Transform cameraTrans;

    //To control up/down rotation of camera
    private float xCameraRotation;

    //Parameters
    //If we zoom in with the sniper rifle we should move slower to make it easier to aim
    private float mouseSensitivity = 100f;

    private float mouseSensitivitySlow = 10f;

    public bool useSlowSensitivity = false;



    void Start()
    {
        
    }

    

    void Update()
    {
        LookLeftRightUpDown();
    }



    //Move camera with mouse
    private void LookLeftRightUpDown()
    {
        float sensitivity = useSlowSensitivity ? mouseSensitivitySlow : mouseSensitivity;
    
        //Left-right
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        //Up-down
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xCameraRotation -= mouseY;

        //If we look directly upwards we dont want to continue looking because it would break our neck
        xCameraRotation = Mathf.Clamp(xCameraRotation, -90f, 90f);

        cameraTrans.localRotation = Quaternion.Euler(xCameraRotation, 0f, 0f);

        //Rotate left-right by moving the body and not the camera
        transform.Rotate(Vector3.up * mouseX);
    }
}
