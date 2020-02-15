using UnityEngine;
using System.Collections;

public class Realism : MonoBehaviour {

    float timer = 0f;

    Vector3 newPos = Vector3.zero;

    Vector3 initPos;

    float lastY = 1f;
    float lastAngle = 0f;

    Vector3 newRotation;


    void Start() 
	{
        initPos = transform.localPosition;

        newRotation = Vector3.zero;
    }



    void LateUpdate()
    {
        //PositionBreath();

        timer += Time.deltaTime;

        if (timer > 2f)
        {
            timer = 0f;

            //float angle = Random.Range(-5f, 5f);

            float angle = 5f;

            if ((lastAngle > 0f && angle > 0f) || (lastAngle < 0f && angle < 0f))
            {
                angle *= -1f;
            }

            lastAngle = angle;

            //Debug.Log(angle);

            Vector3 currentRotVec = transform.localEulerAngles;

            newRotation = new Vector3(currentRotVec.x + angle, currentRotVec.y, currentRotVec.z);
        }

        //transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, Time.deltaTime * 1.0f);

        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, newRotation, Time.deltaTime * 2f);
    }



    void PositionBreath()
    {
        timer += Time.deltaTime;

        if (timer > 2f)
        {
            timer = 0f;

            //Automatically sets z to 0
            //newPos = Random.insideUnitCircle * 0.05f;

            float r = 0.05f;

            float a = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            float x = 0f + r * Mathf.Cos(a);
            float y = 0f + r * Mathf.Sin(a);

            newPos = new Vector3(x, y, 0f);


            //Make sure we breathe up and down
            if ((lastY > 0f && newPos.y > 0f) || (lastY < 0f && newPos.y < 0f))
            {
                newPos *= -1f;
            }

            lastY = newPos.y;

            //Add the initial pos so we get the correct height
            newPos += initPos;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime);
    }
}
