using UnityEngine;
using System.Collections;

public class s_TestLauncher : MonoBehaviour {

    public GameObject launchTarget;
    public float force;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.I))
        {
            launchTarget.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.VelocityChange);
        }
	}
}
