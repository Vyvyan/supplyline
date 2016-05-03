using UnityEngine;
using System.Collections;

public class s_AutoDestroy : MonoBehaviour {

    public float timeBeforeDestroy;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(TimedDestroy());
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    IEnumerator TimedDestroy()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
}
