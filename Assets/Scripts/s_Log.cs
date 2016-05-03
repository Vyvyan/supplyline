using UnityEngine;
using System.Collections;

public class s_Log : MonoBehaviour {

    public GameObject choppedTree;

	// Use this for initialization
	void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionEnter(Collision other)
    {
        // if we get hit by an axe
        if (other.collider.gameObject.tag == "Axe")
        {
            if (other.relativeVelocity.magnitude > 10f)
            {
                Instantiate(choppedTree, gameObject.transform.parent.transform.position, gameObject.transform.parent.transform.rotation);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
