using UnityEngine;
using System.Collections;

public class s_ConveyorBelt : MonoBehaviour {

	public Vector3 beltDirectionVector3;
	public bool BeltReverse;
	public float beltSpeed;
    public s_ConveyorBelt side1belt,side2belt;

	// Use this for initialization
	void Start () 
	{
		BeltReverse = false;
	}

    // Update is called once per frame
    void Update()
    {
        // if we aren't a corner belt, then nbd, just reverse the belt
        if (!BeltReverse)
        {
            beltDirectionVector3 = gameObject.transform.forward * -1;
        }
        else
        {
            beltDirectionVector3 = gameObject.transform.forward;
        }
        
	}
}
