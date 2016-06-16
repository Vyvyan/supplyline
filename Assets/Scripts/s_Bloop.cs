using UnityEngine;
using System.Collections;

public class s_Bloop : MonoBehaviour {

    public static string deliveryQueue;

    public GameObject axeCrate;

    public GameObject holdLocation;

    public GameObject heldItem;

    public float speed;

	// Use this for initialization
	void Start ()
    {
        holdLocation = gameObject.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}
