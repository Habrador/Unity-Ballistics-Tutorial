using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    //Drags
    public ParticleSystem groundHit;

    public bool shouldFireDust = false;
    
    public Vector3 currentPosition;
    public Vector3 currentVelocity;

    Vector3 newPosition = Vector3.zero;
    Vector3 newVelocity = Vector3.zero;



    void Awake()
    {
        currentPosition = transform.position;
    }



    void Start()
    {

    }



    void Update()
    {
        //For safety if we missed the ground for some reason
        if (transform.position.y < -30f)
        {
            Debug.Log("Destryed bullet because below ground with no hit");
        
            Destroy(gameObject);
        }
    }



    void FixedUpdate()
    {
        MoveBullet();
    }



    //Did we hit a target
    void CheckHit() 
    {
        Vector3 fireDirection = (newPosition - currentPosition).normalized;
        //The distance between the bullet positions
        float fireDistance = Vector3.Distance(newPosition, currentPosition);

        RaycastHit hit;

        if (Physics.Raycast(currentPosition, fireDirection, out hit, fireDistance))
        {
            //We hit the target
            if (hit.collider.CompareTag("Target"))
            {
                //Debug.Log("Hit target!");

                Destroy(gameObject);
            }
            //We missed the target and hit the groud
            else if (hit.collider.CompareTag("Ground") && shouldFireDust)
            {
                //ParticleSystem newDust = Instantiate(groundHit, transform.position, Quaternion.identity) as ParticleSystem;

                //newDust.transform.parent = artilleryParent.transform;

                Destroy(gameObject);
            }
        }
    }



    void MoveBullet()
    {
        //Use Heuns method to calculate the new position of the bullet
        float h = Time.fixedDeltaTime;

        //Ballistics.HeunsMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        IntegrationMethods.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

        //First we need these coordinates to check if we have hit something
        CheckHit();

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        //Add the new position to the bullet
        transform.position = currentPosition;
    }
}
