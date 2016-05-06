using UnityEngine;
using System.Collections;

public class s_Rock : MonoBehaviour {

    public GameObject rockToSpawn, metalToSpawn;
    public int rockHealth;
    public Transform[] spawnLocations;
    public ParticleSystem bonk;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if our rock healt is zero, due to being hit by a pick, then spawn the rocks
	    if (rockHealth <= 0)
        {
            foreach(Transform spawn in spawnLocations)
            {
                int rnd = Random.Range(1, 11);
                // if we randomed 9 or lower, then spawn a rock
                if (rnd <= 9)
                {
                    Instantiate(rockToSpawn, spawn.transform.position, Quaternion.identity);
                }
                // if we randomed a 10, then we spawn 3 sets of metal
                else
                {
                    Instantiate(metalToSpawn, spawn.transform.position, Quaternion.identity);
                    Instantiate(metalToSpawn, spawn.transform.position, Quaternion.identity);
                    Instantiate(metalToSpawn, spawn.transform.position, Quaternion.identity);
                }

            }
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        // if we get hit by a pickaxe
        if (other.collider.gameObject.tag == "Pickaxe")

        {
            if (other.relativeVelocity.magnitude > 8f)
            {
                rockHealth--;
                Instantiate(bonk, other.contacts[0].point, Quaternion.identity);
            }
        }
    }
}
