using UnityEngine;
using System.Collections;

public class s_ShippingCatapult : MonoBehaviour {

    public GameObject axeCrate;
    public GameObject shippingObject;

    public GameObject target;
    public float power;

    // Use this for initialization
    void Start ()
    {

        target = GameObject.FindGameObjectWithTag("BuyScreen");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.I))
        {

            shippingObject = Instantiate(axeCrate, gameObject.transform.position, Quaternion.identity) as GameObject;
            Rigidbody rb = shippingObject.transform.GetChild(0).GetComponent<Rigidbody>();
            Vector3 direction = target.transform.position - gameObject.transform.position;

            // fire at the buy menu, with a power based on the distance between the catapult and the tower
            rb.AddForce(new Vector3(direction.x / 2, 100, direction.z / 2) * (power * (direction.magnitude)), ForceMode.VelocityChange);

        }
	}

}
