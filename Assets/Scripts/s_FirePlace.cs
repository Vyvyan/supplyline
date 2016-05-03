using UnityEngine;
using System.Collections;

public class s_FirePlace : MonoBehaviour {

    public ParticleSystem smokeEffect;
    public bool hasAFire;

	// Use this for initialization
	void Start ()
    {
        smokeEffect.Stop();
        hasAFire = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // turn it off every frame, since we'll change it back in a later update if we have a fire in us
        hasAFire = false;
	}

    void Update()
    {
        if (hasAFire)
        {
            if (smokeEffect.isStopped)
            {
                smokeEffect.Play();
            }
        }
        else
        {
            smokeEffect.Stop();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "On Fire")
        {
            hasAFire = true;
        }
    }
}
