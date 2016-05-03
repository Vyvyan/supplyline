using UnityEngine;
using System.Collections;

public class s_Rock : MonoBehaviour {

    public GameObject rockToSpawn;
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
                Instantiate(rockToSpawn, spawn.transform.position, Quaternion.identity);
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
