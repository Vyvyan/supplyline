using UnityEngine;
using System.Collections;

public class s_WispProjectile : MonoBehaviour {

    public GameObject target;

    Rigidbody rb;

    public float power;

    public int damage;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.AddForce((target.transform.position - transform.position) * power, ForceMode.VelocityChange);

        StartCoroutine(AutoDestruct());
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    IEnumerator AutoDestruct()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
