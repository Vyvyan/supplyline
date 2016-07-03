using UnityEngine;
using System.Collections;

public class s_Crate : MonoBehaviour {

    public GameObject payload;
    Rigidbody rb;
    public int itemCode;
    public GameObject axeCrate, pickCrate;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > 30f)
        {
            // item codes
            if (itemCode == 1)
            {
                payload = axeCrate;
            }


            // spawn in game object, then get the child objects (aka, the crate parts) and match velocities with this object, so crates don't STOP when broken
            GameObject temp = Instantiate(payload, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            Rigidbody[] tempChildren = temp.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rig in tempChildren)
            {
                rig.velocity = rb.velocity;
            }
            Destroy(gameObject);
        }
    }
}
