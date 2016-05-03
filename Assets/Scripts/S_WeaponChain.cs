using UnityEngine;
using System.Collections;

public class S_WeaponChain : MonoBehaviour {

    LineRenderer lineRend;
    public Transform objectToDrawALineTo;

	// Use this for initialization
	void Start ()
    {
        lineRend = gameObject.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // sets the start position
        lineRend.SetPosition(0, gameObject.transform.position);

        // sets the end position
        lineRend.SetPosition(1, objectToDrawALineTo.position);
	}
}
