using UnityEngine;
using System.Collections;

public class s_ConstraintsEnabler : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if our rigid body is at a stand still, then freeze it in place
	   if (rb.IsSleeping())
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
       else
        {
            if (rb.constraints != RigidbodyConstraints.FreezeRotationZ)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            }
        }
	}

}
