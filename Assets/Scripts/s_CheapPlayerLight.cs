using UnityEngine;
using System.Collections;

public class s_CheapPlayerLight : MonoBehaviour {


    //THIS ENTIRE SCRIPT IS JUST SO WE HAVE CHEAP AMBIENT LIGHTING WE CAN CHANGE ON THE FLY
    public Light sun;
    Light ourLight;

	// Use this for initialization
	void Start ()
    {
        ourLight = gameObject.GetComponent<Light>();
	}

    // Update is called once per frame
    void Update()
    {
        // change our intensity based on the sun
        if (ourLight.enabled)
        {
            ourLight.intensity = sun.intensity;
        }

        if (sun.intensity < .1f)
        {
            ourLight.enabled = false;
        }
        if (sun.intensity > .1f)
        {
            if (!ourLight.enabled)
            {
                ourLight.enabled = true;
            }
        }
    }
}
