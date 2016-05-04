using UnityEngine;
using System.Collections;

public class s_Animal : MonoBehaviour {

    public GameObject deadAnimal;


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
        // if we are a weapon, then we don't have to hit it as hard to kill it
        if (other.gameObject.tag == "Weapon")
        {
            if (other.relativeVelocity.magnitude > 3)
            {
                Debug.Log("hit with a weapon");
                Instantiate(deadAnimal, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
        }
        // but if we aren't a weapon, then it can die to anything as long as it hits it hard enough
        else if (other.relativeVelocity.magnitude > 6)
        {
            Debug.Log("hit hard enough");
            Instantiate(deadAnimal, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }   
}
