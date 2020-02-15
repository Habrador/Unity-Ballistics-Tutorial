using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticsController : MonoBehaviour 
{
    public static BallisticsController current;

    //The target we want to hit
    public Transform targetTrans;
    //Current wind speed
    public Vector3 windSpeed;



    void Start() 
	{
        current = this;
	}
	


    //Get a fire solution for whoever wants to fire 
    //Should consists of two angles and we should hit the target if we are using these angles
    public FireSolution GetFireSolution(float bulletStartSpeed, Vector3 barrelPos)
    {
        FireSolution fireSolution = null;



        //Alternative 1. Use the basic ballistics equation to calculate a fire solution
        Vector3 targetPos = targetTrans.position;
    
        //Get the 2 angles
        float? highAngle = 0f;
        float? lowAngle = 0f;

        CalculateAngleToHitTarget(out highAngle, out lowAngle, bulletStartSpeed, targetPos, barrelPos);

        //Which angle should we use, gun or artillery? This is the xAngle
        float? xAngle = highAngle;

        if (xAngle != null)
        {
            //We can hit a target and now we need the position
            fireSolution = new FireSolution((float)xAngle, targetPos);
        }



        //Alternative 2. Use gradient descent to calculate a fire solution



        return fireSolution;
    }


    //Which integration method should we use

    //The classic ballistic equation with no drag, wind, etc
    //Which angle do we need to hit the target?
    //Returns 0, 1, or 2 angles depending on if we are within range
    private void CalculateAngleToHitTarget(out float? theta1, out float? theta2, float bulletStartSpeed, Vector3 targetPos, Vector3 barrelPos)
    {
        //Initial speed
        float v = bulletStartSpeed;

        Vector3 targetVec = targetPos - barrelPos;

        //Vertical distance
        float y = targetVec.y;

        //Reset y so we can get the horizontal distance x to the target
        targetVec.y = 0f;

        float x = targetVec.magnitude;

        //Gravity
        float g = 9.81f;


        //Calculate the angles
        float vSqr = v * v;

        float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

        //Check if we are within range
        if (underTheRoot >= 0f)
        {
            float rightSide = Mathf.Sqrt(underTheRoot);

            float top1 = vSqr + rightSide;
            float top2 = vSqr - rightSide;

            float bottom = g * x;

            theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
            theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
        }
        //Out of range!
        else
        {
            theta1 = null;
            theta2 = null;
        }
    }
}



public class FireSolution
{
    //The angles the gun should have to hit the target
    public float xAngle;
    public float yAngle;

    //Sometimes we can only find the xAngle which is the gun elevation angle
    //So we need the position of the target to rotate the gun itself around y axis
    public Vector3 targetPos;

    public FireSolution(float xAngle, Vector3 targetPos)
    {
        this.xAngle = xAngle;

        this.targetPos = targetPos;
    }

    public FireSolution(float xAngle, float yAngle)
    {
        this.xAngle = xAngle;

        this.yAngle = yAngle;
    }
}
