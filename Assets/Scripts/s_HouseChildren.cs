using UnityEngine;
using System.Collections;

public class s_HouseChildren : MonoBehaviour {

    bool hasBrokenHouse;

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
        //Debug.Log(other.relativeVelocity.magnitude.ToString());

        // we check if we've broken the house yet, cause we get errors occasionally
        if (!hasBrokenHouse)
        {
            // if we get hit by an axe
            if (other.collider.gameObject.tag == "Axe")
            {
                if (other.relativeVelocity.magnitude > 10f)
                {
                    GetComponentInParent<s_HouseParent>().BreakHouse();
                    hasBrokenHouse = true;
                }
            }
            // if something with a high mass hits the house hard enough, break the house
            else if (other.rigidbody.mass >= 5 && other.relativeVelocity.magnitude > 10f)
            {
                GetComponentInParent<s_HouseParent>().BreakHouse();
                hasBrokenHouse = true;
            }
        }
    }
}
