using UnityEngine;
using System.Collections;

public class s_CrateParts : MonoBehaviour {

    FixedJoint joint;
    public GameObject crateSide;
    public GameObject payLoad;

	// Use this for initialization
	void Start ()
    {
        joint = gameObject.GetComponent<FixedJoint>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Axe")
        {
            Instantiate(crateSide, gameObject.transform.position, gameObject.transform.rotation);
            // if any of the crate sides break, we need to make the payload a physics object 
            if (payLoad)
            {
                payLoad.GetComponent<Rigidbody>().isKinematic = false;
                payLoad.transform.parent = null;
            }
            Destroy(gameObject);
        }
        else if (other.relativeVelocity.magnitude >= 30)
        {
            Instantiate(crateSide, gameObject.transform.position, gameObject.transform.rotation);
            // if any of the crate sides break, we need to make the payload a physics object
            if (payLoad)
            {
                payLoad.GetComponent<Rigidbody>().isKinematic = false;
                payLoad.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }
}
