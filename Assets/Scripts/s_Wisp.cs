using UnityEngine;
using System.Collections;

public class s_Wisp : MonoBehaviour {

    public Transform book;
    public float yOffset;
    public float speed;

    public GameObject projectile;
    public float power;
    public int damage;
    public GameObject enemyWeAreFighting;

    Quaternion startingRotation;

    public float firingRate;
    float firingTimer;

	// Use this for initialization
	void Start ()
    {
        startingRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // put the wisp above the book always
        transform.position = new Vector3(book.transform.position.x, (book.position.y + 2.8f) + yOffset * Mathf.Sin(speed * Time.time), book.transform.position.z);

        // if we have an enemy we are targetting
        if (enemyWeAreFighting)
        {
            // look at our target
            transform.LookAt(enemyWeAreFighting.transform.position);
            // shoot at the target
            if(firingTimer >= firingRate)
            {
                // instantiate the projectile and assign values
                GameObject temp = Instantiate(projectile, gameObject.transform.position, Quaternion.identity) as GameObject;
                s_WispProjectile tempScript = temp.GetComponent<s_WispProjectile>();
                // assigns this wisps target to the newly spawned projectile
                tempScript.target = enemyWeAreFighting;
                // assigns this wisps power to the newly spawned projectile
                tempScript.power = power;
                tempScript.damage = damage;

                // reset our firing timer
                firingTimer = 0;
            }
        }
        else
        {
            transform.rotation = startingRotation;
        }

        // if we fired a shot, we need to wait for our firing rate to fire again
        if (firingTimer < firingRate)
        {
            firingTimer += Time.deltaTime;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyWeAreFighting = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // if the enemy leaving the trigger volume is the enemy we were locked on to, 
        if (other.gameObject == enemyWeAreFighting)
        {
            // the stop aiming at it
            enemyWeAreFighting = null;
        }
    }
}
