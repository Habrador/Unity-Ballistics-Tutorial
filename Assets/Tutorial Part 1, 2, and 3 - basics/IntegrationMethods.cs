using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A collection of integration methods for ballistics physics
public static class IntegrationMethods
{
    private static Vector3 gravityVec = new Vector3(0f, -9.81f, 0f);



    //Integration method 1
    public static void BackwardEuler(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
    {    
        //Add all factors that affects the acceleration
        //Gravity
        Vector3 accFactor = gravityVec;


        //Calculate the new velocity and position
        //y_k+1 = y_k + timeStep * f(t_k+1, y_k+1)

        //This assumes the acceleration is the same next time step
        newVel = currentVel + timeStep * accFactor;

        newPos = currentPos + timeStep * newVel;
    }



    //Integration method 2
    public static void ForwardEuler(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
    {
        //Add all factors that affects the acceleration
        //Gravity
        Vector3 accFactor = gravityVec;


        //Calculate the new velocity and position
        //y_k+1 = y_k + timeStep * f(t_k, y_k)

        newVel = currentVel + timeStep * accFactor;

        newPos = currentPos + timeStep * currentVel;
    }



    //Integration method 3
    //upVec is a vector perpendicular (in the upwards direction) to the direction the bullet is travelling in
    //is only needed if we calculate the lift force
    public static void Heuns(float timeStep, Vector3 currentPos, Vector3 currentVel, Vector3 upVec, BulletData bulletData, out Vector3 newPos, out Vector3 newVel)
    {
        //Add all factors that affects the acceleration
        //Gravity
        Vector3 accFactorEuler = gravityVec;
        //Drag
        accFactorEuler += BulletPhysics.CalculateBulletDragAcc(currentVel, bulletData);
        //Lift 
        accFactorEuler += BulletPhysics.CalculateBulletLiftAcc(currentVel, bulletData, upVec);


        //Calculate the new velocity and position
        //y_k+1 = y_k + timeStep * 0.5 * (f(t_k, y_k) + f(t_k+1, y_k+1))
        //Where f(t_k+1, y_k+1) is calculated with Forward Euler: y_k+1 = y_k + timeStep * f(t_k, y_k)

        //Step 1. Find new pos and new vel with Forward Euler
        Vector3 newVelEuler = currentVel + timeStep * accFactorEuler;

        //New position with Forward Euler (is not needed here)
        //Vector3 newPosEuler = currentPos + timeStep * currentVel;


        //Step 2. Heuns method's final step
        //If we take drag into account, then acceleration is not constant - it also depends on the velocity
        //So we have to calculate another acceleration factor
        //Gravity
        Vector3 accFactorHeuns = gravityVec;
        //Drag
        //This assumes that windspeed is constant between the steps, which it should be because wind doesnt change that often
        accFactorHeuns += BulletPhysics.CalculateBulletDragAcc(newVelEuler, bulletData);
        //Lift 
        accFactorHeuns += BulletPhysics.CalculateBulletLiftAcc(newVelEuler, bulletData, upVec);

        newVel = currentVel + timeStep * 0.5f * (accFactorEuler + accFactorHeuns);

        newPos = currentPos + timeStep * 0.5f * (currentVel + newVelEuler);
    }



    //Integration method 3.1
    //No external bullet forces except gravity
    //Makes it easier to see if the external forces are working if we display this trajectory
    public static void HeunsNoExternalForces(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
    {
        //Add all factors that affects the acceleration
        //Gravity
        Vector3 accFactor = gravityVec;


        //Calculate the new velocity and position
        //y_k+1 = y_k + timeStep * 0.5 * (f(t_k, y_k) + f(t_k+1, y_k+1))
        //Where f(t_k+1, y_k+1) is calculated with Forward Euler: y_k+1 = y_k + timeStep * f(t_k, y_k)

        //Step 1. Find new pos and new vel with Forward Euler
        Vector3 newVelEuler = currentVel + timeStep * accFactor;

        //New position with Forward Euler (is not needed)
        //Vector3 newPosEuler = currentPos + timeStep * currentVel;

        //Step 2. Heuns method's final step if acceleration is constant
        newVel = currentVel + timeStep * 0.5f * (accFactor + accFactor);

        newPos = currentPos + timeStep * 0.5f * (currentVel + newVelEuler);
    }
}
