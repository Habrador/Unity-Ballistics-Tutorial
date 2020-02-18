using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controlls a gun that fire bullets
public class GunController : MonoBehaviour
{
    //Drags
    //The target we are aiming for
    public Transform targetTrans;
    //The barrel: the part of the barrel we rotate - not the barrel mesh
    public Transform barrelTrans;
    //The end of the cannon barrel where we will create the bullets
    //If we create them at the bottom they will cut through the barrel because we are not simulating
    //the bullets first travelling straight along the barrel
    public Transform endOfBarrelTrans;

    //Data belonging to the bullet we fire
    private BulletData bulletData;

    //The step size
    private float timeStep;

    //For debugging
    private LineRenderer lineRenderer;



    void Awake()
    {
        //Can use a less precise time step to speed up calculations
        //Or a more precise to get a more accurate result
        //BUT lower is not always better because of floating point precision issues!!!
        timeStep = Time.fixedDeltaTime * 1f;

        lineRenderer = GetComponent<LineRenderer>();

        bulletData = barrelTrans.GetComponent<FireBullets>().bulletObj.GetComponent<BulletData>();
    }



    void Update()
    {
        AimGun();

        DrawTrajectoryPath();
    }



    //Aim the gun and the barrel towards the target
    void AimGun()
    {
        //If we want to calculate the barrel should have to hit the target, we will get 0, 1, or 2 angles
        //Angle used if we want to simulate artillery, where the bullet hits the target from above
        float? highAngle = 0f;
        //Angle used if we want to simulate a rifle, where the bullet hits the target from the front
        float? lowAngle = 0f;

        CalculateAngleToHitTarget(out highAngle, out lowAngle);

        //We will here simulate artillery cannon where we fire in a trajetory so the shell lands from above the target
        //If you want to simulate a rifle, you just use lowAngle instead
        
        //If we are within range
        if (highAngle != null)
        {
            float angle = (float)highAngle;

            //Rotate the barrel
            //The equation we use assumes that if we are rotating the gun up from the
            //pointing "forward" position, the angle increase from 0, but our gun's angles
            //decreases from 360 degress when we are rotating up
            barrelTrans.localEulerAngles = new Vector3(360f - angle, 0f, 0f);

            //Rotate the gun turret towards the target
            transform.LookAt(targetTrans);
            transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }



    //Which angle do we need to hit the target?
    //This is a Quadratic equation so we will get 0, 1 or 2 answers
    //These answers are in this case angles
    //If we get 0 angles, it means we cant solve the equation, meaning that we can't hit the target because we are out of range
    //https://en.wikipedia.org/wiki/Projectile_motion
    void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
    {
        //Initial speed
        float v = bulletData.muzzleVelocity;

        Vector3 targetVec = targetTrans.position - endOfBarrelTrans.position;

        //Vertical distance
        float y = targetVec.y;

        //Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

        //Horizontal distance
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
        else
        {
            theta1 = null;
            theta2 = null;
        }
    }



    //Display the trajectory path with a line renderer
    void DrawTrajectoryPath()
    {
        //Start values
        Vector3 currentVel = barrelTrans.forward * bulletData.muzzleVelocity;
        Vector3 currentPos = endOfBarrelTrans.position;

        Vector3 newPos = Vector3.zero;
        Vector3 newVel = Vector3.zero;

        List<Vector3> bulletPositions = new List<Vector3>();

        //Build the trajectory line
        bulletPositions.Add(currentPos);

        //I prefer to use a maxIterations instead of a while loop 
        //so we always avoid stuck in infinite loop and have to restart Unity
        //You might have to change this value depending on your values
        int maxIterations = 10000;

        for (int i = 0; i < maxIterations; i++)
        {
            //Calculate the bullets new position and new velocity
            CurrentIntegrationMethod(timeStep, currentPos, currentVel, out newPos, out newVel);

            //Set the new value to the current values
            currentPos = newPos;
            currentVel = newVel;

            //Add the new position to the list with all positions
            bulletPositions.Add(currentPos);

            //The bullet has hit the ground because we assume 0 is ground height
            //This assumes the bullet is fired from a position above 0 or the loop will stop immediately
            if (currentPos.y < 0f)
            {
                break;
            }

            //A warning message that something might be wrong
            if (i == maxIterations - 1)
            {
                Debug.Log("The bullet newer hit anything because we reached max iterations");
            }
        }


        //Display the bullet positions with a line renderer
        lineRenderer.positionCount = bulletPositions.Count;

        lineRenderer.SetPositions(bulletPositions.ToArray());
    }



    //Choose which integration method you want to use to simulate how the bullet fly
    public static void CurrentIntegrationMethod(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
    {
        //IntegrationMethods.BackwardEuler(timeStep, currentPos, currentVel, out newPos, out newVel);

        //IntegrationMethods.ForwardEuler(timeStep, currentPos, currentVel, out newPos, out newVel);

        //IntegrationMethods.Heuns(timeStep, currentPos, currentVel, out newPos, out newVel);

        IntegrationMethods.HeunsNoExternalForces(timeStep, currentPos, currentVel, out newPos, out newVel);
    }
}
