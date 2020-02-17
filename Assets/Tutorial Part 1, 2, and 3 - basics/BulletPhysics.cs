using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A collection of methods for bullet physics
public static class BulletPhysics
{
    //Calculate the bullet's drag acceleration
    public static Vector3 CalculateBulletDragAcc(Vector3 bulletVel, BulletData bulletData)
    {
        //If you have a wind speed in your game, you can take that into account here:
        //https://www.youtube.com/watch?v=lGg7wNf1w-k
        Vector3 bulletVelRelativeToWindVel = bulletVel - bulletData.windSpeedVector;
    
        //Step 1. Calculate the bullet's drag force
        //https://en.wikipedia.org/wiki/Drag_equation
        //F_drag = 0.5 * rho * v^2 * C_d * A 

        //The velocity of the bullet [m/s]
        float v = bulletVelRelativeToWindVel.magnitude;
        //The bullet's cross section area [m^2]
        float A = Mathf.PI * bulletData.r * bulletData.r;
        
        float F_drag = 0.5f * bulletData.rho * v * v * bulletData.C_d * A;


        //Step 2. We need to add an acceleration, not a force, in the integration method
        //Drag acceleration F = m * a -> a = F / m
        float a_drag = F_drag / bulletData.m;

        //Has to be in a direction opposite of the bullet's velocity vector
        Vector3 dragVec = a_drag * bulletVelRelativeToWindVel.normalized * -1f;

        return dragVec;
    }



    //Calculate the bullet's lift acceleration
    public static Vector3 CalculateBulletLiftAcc(Vector3 bulletVel, BulletData bulletData, Vector3 upDir)
    {
        //If you have a wind speed in your game, you can take that into account here:
        //https://www.youtube.com/watch?v=lGg7wNf1w-k
        Vector3 bulletVelRelativeToWindVel = bulletVel - bulletData.windSpeedVector;

        //Step 1. Calculate the bullet's lift force
        //https://en.wikipedia.org/wiki/Lift_(force)
        //F_lift = 0.5 * rho * v^2 * S * C_l 

        //The velocity of the bullet [m/s]
        float v = bulletVelRelativeToWindVel.magnitude;
        //Planform (projected) wing area, which is assumed to be the same as the cross section area [m^2]
        float S = Mathf.PI * bulletData.r * bulletData.r;

        float F_lift = 0.5f * bulletData.rho * v * v * S * bulletData.C_l;

        //Step 2. We need to add an acceleration, not a force, in the integration method
        //Drag acceleration F = m * a -> a = F / m
        float a_lift = F_lift / bulletData.m;

        //The lift force acts in the up-direction = perpendicular to the velocity direction it travels in
        //Vector3 liftVec = a_lift * upDir;
        Vector3 liftVec = a_lift * Vector3.up;

        return liftVec;
    }
}
