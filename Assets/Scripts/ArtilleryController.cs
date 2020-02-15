using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Attach this to an artillery gun
public class ArtilleryController : MonoBehaviour 
{
    //Drags
    //public Transform targetObj;
    //This is actually the barrel that's rotating
    public Transform barrelTrans;
    
    //For debugging - should be attached to this game object
    private LineRenderer lineRenderer;


    //Initial speed of the bullet which always depends on gun type
    public float bulletStartSpeed = 15f;

    FireBullets fireBulletsScript;

    float angle = 0f;

    //The step size
    //private float h;

    float updateTimer = 0f;

    //public static Vector3 windSpeed = Vector3.zero;

    //The target we are tracking
    Vector3 targetPos;



    void Awake()
    {
        //Can use a less precise h to speed up calculations
        //h = Time.fixedDeltaTime * 1f;

        lineRenderer = GetComponent<LineRenderer>();
    }



	void Start() 
	{
        //fireBulletsScript = gunObj.GetComponent<FireBullets>();

        //lineRenderer = GetComponent<LineRenderer>();
	}
	
	

	void Update() 
	{
        updateTimer += Time.deltaTime;

        if (updateTimer > 0.1f)
        {
            updateTimer = 0f;

            //Get a fire solution
            FireSolution fireSolution = BallisticsController.current.GetFireSolution(bulletStartSpeed, barrelTrans.position);

            if (fireSolution != null)
            {
                RotateGun(fireSolution);
                            
                DrawBulletTrajectory();
            }
            else
            {
                //Hide the line renderer
                lineRenderer.enabled = false;
            }
        }
	}



    //Rotate the gun and the tank with the classic ballistic equation with no wind, drag, etc
    //Stop fire bullets when we are out of range
    void RotateGun(FireSolution fireSolution)
    {
        //Rotate the barrel
        barrelTrans.localEulerAngles = new Vector3(fireSolution.xAngle - 90f, 0f, 0f);

        //Rotate the tank towards the target
        transform.LookAt(fireSolution.targetPos);

        transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
    }



    //How long did it take to reach the target?
    //public float CalculateTimeToHitTarget() { 
    //    //Init values
    //    Vector3 currentVelocity = gunObj.transform.forward * bulletStartSpeed;
    //    Vector3 currentPosition = gunObj.transform.position;
        
    //    Vector3 newPosition = Vector3.zero;
    //    Vector3 newVelocity = Vector3.zero;

    //    float time = 0f;

    //    //The distance from the tank to the target
    //    Vector3 tankTargetVec = targetObj.transform.position - transform.position;
    //    tankTargetVec.y = 0f;
    //    float distSqrTankTarget = tankTargetVec.sqrMagnitude;

    //    //Stop after some time so we dont loop endlessly
    //    for (time = 0f; time < 30f; time += h)
    //    {
    //        //Ballistics.HeunsMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    //        IntegrationMethods.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

    //        //If we are moving downwards and are below the target, then we have hit
    //        if (newPosition.y < currentPosition.y && newPosition.y < targetObj.position.y)
    //        {
    //            time += h * 2f;

    //            break;
    //        }

    //        currentPosition = newPosition;
    //        currentVelocity = newVelocity;
    //    }

    //    return time;
    //}



    //Where did a fake bullet hit the ground?
    //Vector3 CalculateWhereBulletHit()
    //{
    //    //Init values
    //    Vector3 currentVelocity = gunObj.transform.forward * bulletStartSpeed;
    //    Vector3 currentPosition = gunObj.transform.position;

    //    Vector3 newPosition = Vector3.zero;
    //    Vector3 newVelocity = Vector3.zero;

    //    for (float time = 0f; time < 15f; time += h)
    //    {
    //        //Ballistics.HeunsMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    //        IntegrationMethods.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

    //        //If we are moving downwards and are below the target, then we have hit
    //        if (newPosition.y < currentPosition.y && newPosition.y < targetPos.y)
    //        {
    //            //Calculate the real value of the coordinates which is between the current and the new
    //            //Not sure if this is really needed
    //            //http://mathcentral.uregina.ca/QQ/database/QQ.09.01/murray2.html

    //            Vector3 dir = newPosition - currentPosition;

    //            float t = (currentPosition.y - targetPos.y) / dir.y;

    //            newPosition = currentPosition + (dir * t);
                
    //            break;
    //        }

    //        currentPosition = newPosition;
    //        currentVelocity = newVelocity;
    //    }

    //    return newPosition;
    //}



    //Draw the bullet trajectory with a line renderer
    void DrawBulletTrajectory()
    {
        //Show the line renderer
        lineRenderer.enabled = true;

        //Find the positions we should draw with the line renderer
        List<Vector3> bulletPositions = new List<Vector3>();

        //How long did it take to hit the target?
        //float timeToHitTarget = CalculateTimeToHitTarget();


        //Start values
        Vector3 currentVelocity = barrelTrans.forward * bulletStartSpeed;
        Vector3 currentPosition = barrelTrans.position;
        
        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        bulletPositions.Add(currentPosition);

        //This is just for debugging we we can use a low resolution to make it faster
        float h = Time.fixedDeltaTime;

        //Simulate the bullet and save the position each step
        for (int i = 0; i < 500; i++)
        {
            //lineRenderer.SetPosition(index, currentPosition);

            //Use Heuns method to calculate the new position of the bullet
            //Ballistics.HeunsMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
            IntegrationMethods.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

            currentPosition = newPosition;
            currentVelocity = newVelocity;

            //Use the equation to find the next position
            //currentPosition = GetBulletPos(t);

            bulletPositions.Add(currentPosition);

            //Have we it the ground, which assumes we started above the ground
            //Could maybe use raycast but is slower
            if (currentPosition.y < 0f)
            {
                break;
            }
        }

        //Debug.Log(Vector3.Distance(bulletPositions[0], bulletPositions[bulletPositions.Count - 1]));

        //Add the positions to the line renderer
        //Important to first set how many positions we have, or the line renderer will only use 
        //the number of positions that are default or what we had last time
        lineRenderer.positionCount = bulletPositions.Count;

        lineRenderer.SetPositions(bulletPositions.ToArray());
    }



    //Easier to change integration method once in this method
    //public static void CurrentIntegrationMethod(
    //    float h,
    //    Vector3 currentPosition,
    //    Vector3 currentVelocity,
    //    out Vector3 newPosition,
    //    out Vector3 newVelocity)
    //{
    //    //IntegrationMethods.EulerForward(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    //    IntegrationMethods.Heuns(h, currentPosition, currentVelocity, windSpeed, out newPosition, out newVelocity);
    //    //IntegrationMethods.RungeKuttasMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    //}



    //Heun's method - one iteration
    //Will give a better result than Euler forward, but will not match Unity's physics engine
    //so the bullets also have to use Heuns method
    //public static void HeunsMethod(
    //    float h,
    //    Vector3 currentPosition, 
    //    Vector3 currentVelocity, 
    //    out Vector3 newPosition,
    //    out Vector3 newVelocity) 
    //{        
    //    //Init acceleration
    //    //Gravity
    //    Vector3 acceleartionFactorEuler = Physics.gravity;
    //    Vector3 acceleartionFactorHeun = Physics.gravity;

        
    //    //Init velocity
    //    //Current velocity
    //    Vector3 velocityFactor = currentVelocity;
    //    //Wind velocity
    //    //velocityFactor += new Vector3(2f, 0f, 3f);


    //    //
    //    //Main algorithm
    //    //
    //    //Euler forward
    //    Vector3 pos_E = currentPosition + h * velocityFactor;

    //    //acceleartionFactorEuler += BulletPhysics.CalculateDrag(currentVelocity);

    //    Vector3 vel_E = currentVelocity + h * acceleartionFactorEuler;


    //    //Heuns method
    //    Vector3 pos_H = currentPosition + h * 0.5f * (velocityFactor + vel_E);

    //    //acceleartionFactorHeun += BulletPhysics.CalculateDrag(vel_E);

    //    Vector3 vel_H = currentVelocity + h * 0.5f * (acceleartionFactorEuler + acceleartionFactorHeun);


    //    newPosition = pos_H;
    //    newVelocity = vel_H;
    //}


    /*
    //Calculate the bullet's drag's acceleration, not force
    static Vector3 CalculateDrag(Vector3 velocityVec) 
    {
        //F_drag = k * v^2 = m * a
        //k = 0.5 * C_d * rho * A 

        float m = 0.2f; // kg
        float C_d = 0.5f;
        float A = Mathf.PI * 0.05f * 0.05f; // m^2
        float rho = 1.225f; // kg/m3

        float k = 0.5f * C_d * rho * A;

        float vSqr = velocityVec.sqrMagnitude;

        float aDrag = (k * vSqr) / m;

        //Has to be in a direction opposite of the bullet's velocity vector
        Vector3 dragVec = aDrag * velocityVec.normalized * -1f; 

        return dragVec;
    }
    */



    //Get the bullet position as a function of time
    //with equation from report - doesnt work
    //Vector3 GetBulletPos(float time)
    //{
    //    Vector3 v_zero = gunObj.transform.forward * bulletSpeed;
    //    Vector3 p_zero = gunObj.transform.position;

    //    Vector3 v_medium = Vector3.zero;

    //    float g = 9.81f;

    //    Vector3 v_terminal = new Vector3(0f, -CalculateTerminalVelocity(), 0f);

    //    Vector3 v_infinity = v_terminal + v_medium;

    //    float k = 0.5f * (g / v_terminal.magnitude);

    //    Vector3 bulletPos = (((v_zero + (k * time * v_infinity)) * time) / (1f + (k * time))) + p_zero;

    //    //Vector3 bulletPos = (1f / (2f * k)) * (v_zero - v_infinity) * (1f - Mathf.Exp(-2f * k * time)) + (v_infinity * time) + p_zero;

    //    return bulletPos;
    //}



    //Calculate the bullets terminal velocity
    //static float CalculateTerminalVelocity() 
    //{
    //    float m = 0.2f; // kg
    //    float g = 9.81f; // m/s^2
    //    float C_d = 0.5f;
    //    float A = Mathf.PI * 0.05f * 0.05f; // m^2
    //    float rho = 1.225f; // kg/m3

    //    float v_terminal = Mathf.Sqrt((2f * m * g) / (C_d * rho * A));

    //    return v_terminal;
    //}

}
