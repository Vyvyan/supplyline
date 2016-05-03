using UnityEngine;
using System.Collections;

public class s_ConveyorBelt : MonoBehaviour {

	public Vector3 beltDirectionVector3;
	public bool BeltReverse;
	public float beltSpeed;
    public bool isCorner;
    public s_ConveyorBelt side1belt,side2belt;
    public bool isMiddleCorner;

	// Use this for initialization
	void Start () 
	{
		BeltReverse = false;
	}

    // Update is called once per frame
    void Update()
    {
        // if we aren't a corner belt, then nbd, just reverse the belt
        if (!isCorner)
        { 
            if (!BeltReverse)
            {
                beltDirectionVector3 = transform.right * -1;
            }
            else
            {
                beltDirectionVector3 = transform.right;
            }
        }

        // if we ARE a corner belt
        if (isCorner)
        {
            // if we are the belt in the middle of the corner
            if (isMiddleCorner)
            {
                if (!BeltReverse)
                {
                    // change the belt orientation of the side belts
                    side1belt.beltDirectionVector3 = side1belt.gameObject.transform.right;
                    side2belt.beltDirectionVector3 = side2belt.gameObject.transform.right * -1;
                    // face whatever way the side belt is facing
                    beltDirectionVector3 = side2belt.beltDirectionVector3;
                }
                if (BeltReverse)
                {
                    // change the belt orientation of the side belts
                    side1belt.beltDirectionVector3 = side1belt.gameObject.transform.right * -1;
                    side2belt.beltDirectionVector3 = side2belt.gameObject.transform.right;
                    // face whatever way the other side belt is facing
                    beltDirectionVector3 = side1belt.beltDirectionVector3;
                }
            }
        }
	}
}
