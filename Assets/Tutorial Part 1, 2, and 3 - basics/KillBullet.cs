using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We fire bullets continuously, so we need to remove them 
public class KillBullet : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        //Remove the bullet if it has fallen off the plane
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
