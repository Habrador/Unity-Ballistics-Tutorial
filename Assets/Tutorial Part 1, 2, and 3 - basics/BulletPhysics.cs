using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A collection of methods for bullet physics
public static class BulletPhysics
{
    //Bullet data belong to the bullet we simulate
    //So you might have to change it depending on the bullet you have
    //Mass [kg]
    private static float m = 0.2f;
    //Radius [m]
    private static float r = 0.05f;

    //Calculate the bullet's drag acceleration
    public static Vector3 CalculateBulletDragAcc(Vector3 bulletVel, Vector3 windSpeed)
    {
        //If you have a wind speed in your game, you can take that into account here:
        //https://www.youtube.com/watch?v=lGg7wNf1w-k
        Vector3 bulletVelRelativeToWindVel = bulletVel - windSpeed;
    
        //Step 1. Calculate the bullet's drag force
        //https://en.wikipedia.org/wiki/Drag_equation
        //F_drag = 0.5 * rho * v^2 * C_d * A 

        //The density of the fluid is travelling in, which in this case is air at 15 degrees [kg/m^3]
        float rho = 1.225f;
        //The velocity of the bullet [m/s]
        float v = bulletVelRelativeToWindVel.magnitude;
        //Drag coefficient, which is a value you can't calculate - you have to simulate it in a wind tunnel
        //and it also depends on the speed
        //A Tesla Model S has the drag coefficient 0.24
        float C_d = 0.5f; //Assume this value
        //The bullet's cross section area [m^2]
        float A = Mathf.PI * r * r;
        
        float F_drag = 0.5f * rho * v * v * C_d * A;


        //Step 2. We need to add an acceleration, not a force, in the integration method
        //Drag acceleration F = m * a -> a = F / m
        float a_drag = F_drag / m;

        //Has to be in a direction opposite of the bullet's velocity vector
        Vector3 dragVec = a_drag * bulletVelRelativeToWindVel.normalized * -1f;

        return dragVec;
    }



    //Calculate the bullet's lift acceleration
    public static Vector3 CalculateBulletLiftAcc(Vector3 bulletVel, Vector3 windSpeed, Vector3 upDir)
    {
        //If you have a wind speed in your game, you can take that into account here:
        //https://www.youtube.com/watch?v=lGg7wNf1w-k
        Vector3 bulletVelRelativeToWindVel = bulletVel - windSpeed;

        //Step 1. Calculate the bullet's lift force
        //https://en.wikipedia.org/wiki/Lift_(force)
        //F_lift = 0.5 * rho * v^2 * S * C_l 

        //The density of the fluid is travelling in, which in this case is air at 15 degrees [kg/m^3]
        float rho = 1.225f;
        //The velocity of the bullet [m/s]
        float v = bulletVelRelativeToWindVel.magnitude;
        //Lift coefficient
        float C_l = 0.5f;
        //Planform (projected) wing area, which is assumed to be the same as the cross section area [m^2]
        float S = Mathf.PI * r * r;

        float F_lift = 0.5f * rho * v * v * S * C_l;

        //Step 2. We need to add an acceleration, not a force, in the integration method
        //Drag acceleration F = m * a -> a = F / m
        float a_lift = F_lift / m;

        //The lift force acts in the up-direction = perpendicular to the velocity direction it travels in
        Vector3 liftVec = a_lift * upDir;

        return liftVec;
    }
}
