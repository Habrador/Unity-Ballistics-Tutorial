using UnityEngine;
using System.Collections;

public class BulletPhysics : MonoBehaviour 
{
    //Calculate the bullet's drag's acceleration
    public static Vector3 CalculateDrag(Vector3 velocityVec)
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
}
