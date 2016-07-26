using UnityEngine;
using System.Collections;

public class s_Flyer : MonoBehaviour {

    TextMesh tm_name, tm_job, tm_likes1, tm_likes2, tm_dislikes;

    // we assign these variables because when we try to assign the text when the flyer is instantiated, the textmesh variables are too slow to assign and we get errors
    public string name_string, job_string, likes1_string, likes2_string, dislikes_string;

	// Use this for initialization
	void Start ()
    {
        tm_name = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        tm_job = gameObject.transform.GetChild(1).GetComponent<TextMesh>();
        tm_likes1 = gameObject.transform.GetChild(3).GetComponent<TextMesh>();
        tm_likes2 = gameObject.transform.GetChild(4).GetComponent<TextMesh>();
        tm_dislikes = gameObject.transform.GetChild(6).GetComponent<TextMesh>();

        tm_name.text = name_string;
        tm_job.text = job_string;
        tm_likes1.text = likes1_string;
        tm_likes2.text = likes2_string;
        tm_dislikes.text = dislikes_string;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
