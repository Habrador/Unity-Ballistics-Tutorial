using UnityEngine;
using System.Collections;

public class IntegrationMethods : MonoBehaviour 
{
    //Easier to change integration method once in this method
    public static void CurrentIntegrationMethod(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity)
    {
        //IntegrationMethods.EulerForward(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        IntegrationMethods.Heuns(h, currentPosition, currentVelocity, Vector3.zero, out newPosition, out newVelocity);
        //IntegrationMethods.RungeKutta(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        //IntegrationMethods.BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    }



    //Euler's method - one iteration
    //Will not match Unity's physics engine
    public static void EulerForward(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity)
    {
        //Init acceleration
        //Gravity
        Vector3 acceleartionFactor = Physics.gravity;
        //acceleartionFactor += CalculateDrag(currentVelocity);


        //Init velocity
        //Current velocity
        Vector3 velocityFactor = currentVelocity;
        //Wind velocity
        //velocityFactor += new Vector3(2f, 0f, 3f);


        //
        //Main algorithm
        //
        newPosition = currentPosition + h * velocityFactor;

        newVelocity = currentVelocity + h * acceleartionFactor;
    }
    
    

    //Heun's method - one iteration
    //Will give a better result than Euler forward, but will not match Unity's physics engine
    //so the bullets also have to use Heuns method
    public static void Heuns(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        Vector3 windSpeed,
        out Vector3 newPosition,
        out Vector3 newVelocity)
    {
        //Init acceleration
        //Gravity
        Vector3 acceleartionFactorEuler = Physics.gravity;
        Vector3 acceleartionFactorHeun = Physics.gravity;


        //Init velocity
        //Current velocity
        Vector3 velocityFactor = currentVelocity;
        //Wind velocity
        velocityFactor += windSpeed;


        //
        //Main algorithm
        //
        //Euler forward
        Vector3 pos_E = currentPosition + h * velocityFactor;

        //acceleartionFactorEuler += BulletPhysics.CalculateDrag(currentVelocity);

        Vector3 vel_E = currentVelocity + h * acceleartionFactorEuler;


        //Heuns method
        Vector3 pos_H = currentPosition + h * 0.5f * (velocityFactor + vel_E);

        //acceleartionFactorHeun += BulletPhysics.CalculateDrag(vel_E);

        Vector3 vel_H = currentVelocity + h * 0.5f * (acceleartionFactorEuler + acceleartionFactorHeun);


        newPosition = pos_H;
        newVelocity = vel_H;
    }



    //RungeKuttas method - one iteration
    public static void RungeKutta(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity)
    {
        //Init acceleration
        //Gravity
        Vector3 acceleartionFactor = Physics.gravity;
        //acceleartionFactor += CalculateDrag(currentVelocity);


        //Init velocity
        //Current velocity
        Vector3 velocityFactor = currentVelocity;
        //Wind velocity
        //velocityFactor += new Vector3(2f, 0f, 3f);


        //
        //Main algorithm
        //
        //Position
        Vector3 k1_vel = velocityFactor;

        Vector3 k2_vel = currentVelocity + (h / 2f) * acceleartionFactor;

        Vector3 k3_vel = currentVelocity + (h / 2f) * acceleartionFactor;

        Vector3 k4_vel = currentVelocity + h * acceleartionFactor;

        newPosition = currentPosition + ((h / 6f) * (k1_vel + 2f * k2_vel + 2f * k3_vel + k4_vel));


        //Velocity
        Vector3 k1_acc = acceleartionFactor;

        Vector3 k2_acc = acceleartionFactor;

        Vector3 k3_acc = acceleartionFactor;

        Vector3 k4_acc = acceleartionFactor;

        newVelocity = currentVelocity + ((h / 6f) * (k1_acc + 2f * k2_acc + 2f * k3_acc + k4_acc));
    }



    //Euler's method - one iteration
    //Will give a better result than Euler forward, but will not match Unity's physics engine
    //so the bullets also have to use Heuns method
    public static void BackwardEuler(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity)
    {
        //Init acceleration
        //Gravity
        Vector3 acceleartionFactor = Physics.gravity;
        //acceleartionFactor += CalculateDrag(currentVelocity);


        //Init velocity
        //Current velocity
        //Vector3 velocityFactor = currentVelocity;
        //Wind velocity
        //velocityFactor += new Vector3(2f, 0f, 3f);


        //
        //Main algorithm
        //
        newVelocity = currentVelocity + h * acceleartionFactor;

        newPosition = currentPosition + h * newVelocity;
    }
}
