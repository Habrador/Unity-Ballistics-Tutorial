using UnityEngine;
using System.Collections;

public class TutorialBullet : MonoBehaviour
{
    //Drags
    public GameObject hitMarker;
    public GameObject targetMirror;
    
    public Vector3 currentPosition;
    public Vector3 currentVelocity;

    Vector3 newPosition = Vector3.zero;
    Vector3 newVelocity = Vector3.zero;



    void Awake()
    {
        currentPosition = transform.position;
    }



    void Update()
    {
        DestroyBullet();
    }



    void FixedUpdate()
    {
        MoveBullet();
    }



    //Did we hit a target
    void CheckHit() 
    {
        Vector3 fireDirection = (newPosition - currentPosition).normalized;
        float fireDistance = Vector3.Distance(newPosition, currentPosition);

        RaycastHit hit;

        if (Physics.Raycast(currentPosition, fireDirection, out hit, fireDistance))
        {
            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("Hit target!");
                
                //Destroy the bullet
                Destroy(gameObject);

                //Add marker where we hit target
                GameObject.FindGameObjectWithTag("Player").GetComponent<SniperController>().AddMarker(hit.point);
            }
        }
    }



    void MoveBullet()
    {
        //Use Heuns method to calculate the new position of the bullet
        float h = Time.fixedDeltaTime;
        IntegrationMethods.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

        //First we need these coordinates to check if we have hit something
        CheckHit();

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        //Add the new position to the bullet
        transform.position = currentPosition;
    }



    void DestroyBullet()
    {
        if (transform.position.y < -30f)
        {
            Destroy(gameObject);
        }
    }
}
