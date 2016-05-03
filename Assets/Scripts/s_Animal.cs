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
        if (other.relativeVelocity.magnitude > 3)
        {
            Instantiate(deadAnimal, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }   
}
