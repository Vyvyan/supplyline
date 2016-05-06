using UnityEngine;
using System.Collections;

public class s_Chipper : MonoBehaviour {

    public GameObject spawnLocation;
    public GameObject wood, stone, gem, gold;
    string spawnString;
    float spawnTimer;
    public float spawnTimerLimit;

	// Use this for initialization
	void Start ()
    {
        spawnTimer = 0;
        // we need this leading zero in the string to avoid errors, we reference the first index for spawning
        spawnString += 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void FixedUpdate()
    {
        //Debug.Log(spawnString);

        // what we spawn is numbered, so wood = 1, stone = 2, A Small Stone = 3, we need to specify small stones, so we can't get random objects from a single small stone

        // if we have something in the spawn string
        if (spawnString.Length > 1)
        {
            if (spawnTimer <= 0)
            {
                if (spawnString[1] == '1')
                {
                    // spawn wood
                    GameObject temp = Instantiate(wood, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
                    temp.GetComponent<Rigidbody>().AddForce(spawnLocation.transform.up * 500);
                }
                if (spawnString[1] == '2')
                {
                    // spawn stone, but first, random between 1-10, 10% chance to spawn a gem
                    int rand = Random.Range(1, 12);
                    // assign this to stone, but will change it later. Compiler doesn't like this being unnassigned
                    GameObject objectToSpawn = stone;
                    // if random is 9 and under, spawn a stone
                    if (rand <= 9)
                    {
                        objectToSpawn = stone;
                    }
                    // if we randomed 10, spawn a gem
                    if (rand == 10)
                    {
                        objectToSpawn = gem;
                    }
                    // if we randomed 11, spawn gold
                    if (rand == 11)
                    {
                        objectToSpawn = gold;
                    }
                    GameObject temp = Instantiate(objectToSpawn, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
                    temp.GetComponent<Rigidbody>().AddForce(spawnLocation.transform.up * 500);
                }
                if (spawnString[1] == '3')
                {
                    // spawn stone
                    GameObject temp = Instantiate(stone, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
                    temp.GetComponent<Rigidbody>().AddForce(spawnLocation.transform.up * 500);
                }



                // remove the letter from the first index, since we spawned the object associated with it
                spawnString = spawnString.Remove(1, 1);

                // resets the timer
                spawnTimer = spawnTimerLimit;
            }
        }

        // spawn timer
        if (spawnTimer > 0)
        {
            // count the timer down
            spawnTimer -= Time.deltaTime;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // is what touches us a resource?
        if (other.gameObject.GetComponent<s_Resource>())
        {
            // get the object
            s_Resource otherScript = other.gameObject.GetComponent<s_Resource>();

            if (otherScript.isWood)
            {
                while (otherScript.woodPower > 0)
                {
                    // minus 5, add 1 to the spawn string
                    otherScript.woodPower -= 5;
                    spawnString += '1';
                }
            }
            if (otherScript.isStone)
            {
                // if we are a SMALL stone, then we don't want a chance to spawn a gem, so it is it's own number
                if (otherScript.stonePower == 5)
                {
                    spawnString += '3';
                }
                else
                {
                    while (otherScript.stonePower > 0)
                    {
                        // minus 5, add 1 to the spawn string
                        otherScript.stonePower -= 5;
                        spawnString += '2';
                    }
                }
            }

            Destroy(otherScript.gameObject);
        }
    }
}
