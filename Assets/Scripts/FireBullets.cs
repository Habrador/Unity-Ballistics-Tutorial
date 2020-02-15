using UnityEngine;
using System.Collections;

public class FireBullets : MonoBehaviour 
{
    public GameObject bulletObj;
    public Transform bulletParent;
	
    public bool shouldFireBullets = true;



	void Start() 
	{
        StartCoroutine(FireBullet());
	}
	
	

	void Update() 
	{
	
	}



    public IEnumerator FireBullet() 
    {        
        while (true)
        {
            if (shouldFireBullets)
            {
                GameObject newBullet = Instantiate(bulletObj, transform.position, transform.rotation) as GameObject;

                newBullet.transform.parent = bulletParent;

                //Add velocity
                //newBullet.GetComponent<Bullet>().currentVelocity = Ballistics.bulletSpeed * transform.forward;
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
