using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Will move a bullet with greater accuracy than if we move it with Unity's built-in rigidbody
public class MoveBullet : MonoBehaviour
{
    private Vector3 currentPos;
    private Vector3 currentVel;

    private Vector3 newPos;
    private Vector3 newVel;



    void Awake()
    {
        currentPos = transform.position;
    }



    void FixedUpdate()
    {
        MoveBulletOneStep();
    }



    void MoveBulletOneStep()
    {
        //Use an integration method to calculate the new position of the bullet
        float timeStep = Time.fixedDeltaTime;
        
        IntegrationMethods.Heuns(timeStep, currentPos, currentVel, transform.up, out newPos, out newVel);

        //Debug.DrawRay(transform.position, transform.up * 5f);

        //Set the new values to the old values for next update
        currentPos = newPos;
        currentVel = newVel;

        //Add the new position to the bullet
        transform.position = currentPos;

        //Change so the bullet points in the velocity direction
        transform.forward = currentVel.normalized;
    }



    //Set start values when we create the bullet
    public void SetStartValues(Vector3 startPos, Vector3 startVel)
    {
        this.currentPos = startPos;
        this.currentVel = startVel;
    }
}
