using UnityEngine;
using System.Collections;

public class s_Conveyor : MonoBehaviour {

	public GameObject BJ1, BJ2, BJ3, BJ4, side1, side2, belt, parentConvObj;
	public bool hasBeenPlaced;
	public bool startWithCollision;
	// variables holding the buttons that change the belt direction
	public GameObject ButtonDirectionPlus1, ButtonDirectionPlus2, ButtonDirectionNeg1, ButtonDirectionNeg2;
	// variable holding the belt script
	s_ConveyorBelt beltScript;
	// material variables
	public Material mat_ButtonOn, mat_ButtonOff;
    public GameObject BJ1_Corner, BJ2_Corner, BJ1_Corner_Alt, BJ2_Corner_Alt;

    public GameObject[] conveyorParts;

	// Use this for initialization
	void Start () 
	{
		hasBeenPlaced = false;
		// this is just used mainly for testing, since most of the conveyors that spawn have collision off by default
		if (!startWithCollision)
		{
			TurnOffCollision();
		}

		beltScript = belt.GetComponent<s_ConveyorBelt>();
		// sets the button materials based on the direction the belt is set at
		if (beltScript.BeltReverse)
		{
			ButtonDirectionNeg1.GetComponent<Renderer>().material = mat_ButtonOff;
			ButtonDirectionNeg2.GetComponent<Renderer>().material = mat_ButtonOff;
			ButtonDirectionPlus1.GetComponent<Renderer>().material = mat_ButtonOn;
			ButtonDirectionPlus2.GetComponent<Renderer>().material = mat_ButtonOn;
		}
		// sets the button materials based on the direction the belt is set at
		if (!beltScript.BeltReverse)
		{
			ButtonDirectionNeg1.GetComponent<Renderer>().material = mat_ButtonOn;
			ButtonDirectionNeg2.GetComponent<Renderer>().material = mat_ButtonOn;
			ButtonDirectionPlus1.GetComponent<Renderer>().material = mat_ButtonOff;
			ButtonDirectionPlus2.GetComponent<Renderer>().material = mat_ButtonOff;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
       
	}

	void TurnOffCollision()
	{
		foreach(GameObject convPart in conveyorParts)
        {
            convPart.GetComponent<Collider>().enabled = false;
        }
	}

	void TurnOnCollision()
	{
        foreach (GameObject convPart in conveyorParts)
        {
            convPart.GetComponent<Collider>().enabled = true;
        }
    }

	public void ChangeBeltDirection()
	{
		// change the belt direction
		beltScript.BeltReverse = !beltScript.BeltReverse;

		// change the materials on the buttons so we know where its going
		if (beltScript.BeltReverse)
		{
			ButtonDirectionNeg1.GetComponent<Renderer>().material = mat_ButtonOff;
			ButtonDirectionNeg2.GetComponent<Renderer>().material = mat_ButtonOff;
			ButtonDirectionPlus1.GetComponent<Renderer>().material = mat_ButtonOn;
			ButtonDirectionPlus2.GetComponent<Renderer>().material = mat_ButtonOn;
		}
		// sets the button materials based on the direction the belt is set at
		if (!beltScript.BeltReverse)
		{
			ButtonDirectionNeg1.GetComponent<Renderer>().material = mat_ButtonOn;
			ButtonDirectionNeg2.GetComponent<Renderer>().material = mat_ButtonOn;
			ButtonDirectionPlus1.GetComponent<Renderer>().material = mat_ButtonOff;
			ButtonDirectionPlus2.GetComponent<Renderer>().material = mat_ButtonOff;
		}
	}
}
