using UnityEngine;
using System.Collections;

public class s_Wisp : MonoBehaviour {

    public Transform book;
    public float yOffset;
    public float speed;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        // put the wisp above the book always
        transform.position = new Vector3(book.transform.position.x, (book.position.y + 2.8f) + yOffset * Mathf.Sin(speed * Time.time), book.transform.position.z);
	}
}
