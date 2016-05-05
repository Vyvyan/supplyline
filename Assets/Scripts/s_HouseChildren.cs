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
            if (other.collider.gameObject.tag == "Axe" || other.collider.gameObject.tag == "Weapon")
            {
                if (other.relativeVelocity.magnitude > 10f)
                {
                    GetComponentInParent<s_HouseParent>().BreakHouse();
                    hasBrokenHouse = true;
                }
            }
        }
    }
}
