using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fire bullets continuously from a cannon barrel
public class FireBullets : MonoBehaviour
{
    //Drags
    //The bullet we will fire from the cannon
    public GameObject bulletObj;
    //We will parent all bullets to this object to get a clean workspace
    public Transform bulletsParent;
    //The end of the cannon barrel where we will create the bullets
    //If we create them at the bottom they will cut through the barrel because we are not simulating
    //the bullets first travelling straight along the barrel
    public Transform endOfBarrelTrans;

    //Bullet data
    private BulletData bulletData;



    void Start()
    {
        bulletData = bulletObj.GetComponent<BulletData>();

        StartCoroutine(FireBullet());
    }



    //Fire bullets continuously
    public IEnumerator FireBullet()
    {
        while (true)
        {
            //Create a new bullet at the end of the cannon barrel
            GameObject newBullet = Instantiate(bulletObj) as GameObject;

            //Parent it to get a less messy workspace
            newBullet.transform.parent = bulletsParent;

            //The bullets start position and start direction
            Vector3 startPos = endOfBarrelTrans.position;
            Vector3 startDir = transform.forward;


            //If the bullet has a rigidbody
            //newBullet.transform.position = startPos;

            //The start speed vector
            //Vector3 startSpeedVec = bulletData.muzzleVelocity * startDir;

            //Add velocity to the bullet with a rigidbody
            //newBullet.GetComponent<Rigidbody>().velocity = startSpeedVec;


            //If the bullet has no rigidbody
            newBullet.GetComponent<MoveBullet>().SetStartValues(startPos, startDir);

            //Wait 2 seconds until we fire another bullet
            yield return new WaitForSeconds(2f);
        }
    }
}
