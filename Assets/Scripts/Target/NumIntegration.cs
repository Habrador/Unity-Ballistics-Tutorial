using UnityEngine;
using System.Collections;

//Predict future position with numerical integration
public class NumIntegration : MonoBehaviour 
{
    //Obj that shows the future position
    public GameObject showFuturePosObj;
    public GameObject cannonObj;

    //How far into the future we want to predict the position
    public float maxTime = 1f;

    Vector3 lastPos;
    Vector3 lastVel;

    //Vector3 velocityVec;
    Vector3 accVec;

    Vector3 averagePosVec = Vector3.zero;
    Vector3 averageAccVec = Vector3.zero;

    public Vector3 predictedPosition;

    //Ballistics ballisticsScript;



	void Start() 
	{
        lastPos = transform.position;
        lastVel = Vector3.zero;

        //ballisticsScript = cannonObj.GetComponent<Ballistics>();
	}
	
	
	void Update() 
	{

        //Calculate the velocity vector
        Vector3 velocityVec = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
        
        //Vector3 accVec = (velocityVec - lastVel) / Time.deltaTime;
        //lastVel = velocityVec;

        //Calculate average velocity
        //http://math.stackexchange.com/questions/22348/how-to-add-and-subtract-values-from-an-average
        averagePosVec = averagePosVec + ((velocityVec - averagePosVec) / 50f);
        //averageAccVec = averageAccVec + ((accVec - averageAccVec) / 500f);

        predictedPosition = showFuturePosObj.transform.position;


        if (velocityVec.sqrMagnitude > 3f * 3f)
        {
            //Predict future pos if we are moving
            PredictPosition();
        }
	}



    void PredictPosition() 
    {
        float time = 0f;

        float h = 0.06f;

        Vector3 currentPos = transform.position;
        Vector3 currentVel = averagePosVec;

        //is not working
        //currentPos *= maxTime; 
        
        //Find the the predicted time
        //maxTime = ballisticsScript.CalculateTimeToHitTarget();
        //maxTime = 0.5f;

        while (time < maxTime)
        {
            currentPos += h * currentVel;
            //currentVel += h * averageAccVec;

            time += h;
        }

        showFuturePosObj.transform.position = currentPos;
    }
}
