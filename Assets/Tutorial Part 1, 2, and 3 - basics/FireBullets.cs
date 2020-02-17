using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fire bullets continuously from a cannon barrel
public class FireBullets : MonoBehaviour
{
    //The bullet we will fire from the cannon
    public GameObject bulletObj;
    //We will parent all bullets to this object to get a clean workspace
    public Transform bulletsParent;



    void Start()
    {
        StartCoroutine(FireBullet());
    }



    //Fire bullets continuously
    public IEnumerator FireBullet()
    {
        while (true)
        {
            //Create a new bullet at the base of the cannon barrel
            GameObject newBullet = Instantiate(bulletObj, transform.position, transform.rotation) as GameObject;

            //Parent it to get a less messy workspace
            newBullet.transform.parent = bulletsParent;

            //The start speed vector
            Vector3 startSpeedVec = GunController.bulletStartSpeed * transform.forward;

            //Add velocity to the bullet with a rigidbody
            //newBullet.GetComponent<Rigidbody>().velocity = startSpeedVec;

            //Add start values to the bullet that has no rigidbody
            newBullet.GetComponent<MoveBullet>().SetStartValues(transform.position, startSpeedVec);

            //Wait 2 seconds until we fire another bullet
            yield return new WaitForSeconds(2f);
        }
    }
}
