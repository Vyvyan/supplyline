using UnityEngine;
using System.Collections;

public class s_BuyMenu : MonoBehaviour {

    public enum MenuState {start, defense, resourceGather, misc};
    public MenuState menuState;

    public GameObject beacon;

    public string shipmentQueue;

    Rigidbody rb;

    bool canChangeDisplay;

    public GameObject startGroup, defenseGroup, resourceGatherGroup, miscGroup;

	// Use this for initialization
	void Start ()
    {
        menuState = MenuState.start;
        rb = gameObject.GetComponentInParent<Rigidbody>();
        canChangeDisplay = true;
        beacon = GameObject.FindGameObjectWithTag("Beacon Target");
	}
	
	// Update is called once per frame
	void Update ()
    {
        beacon.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 15, gameObject.transform.position.z);
	    if (menuState == MenuState.start)
        {
            if (canChangeDisplay)
            {
                startGroup.active = true;
                defenseGroup.active = false;
                resourceGatherGroup.active = false;
                miscGroup.active = false;
            }
        }
        else if (menuState == MenuState.resourceGather)
        {
            if (canChangeDisplay)
            {
                startGroup.active = false;
                defenseGroup.active = false;
                resourceGatherGroup.active = true;
                miscGroup.active = false;
            }
        }
        else if (menuState == MenuState.defense)
        {
            if (canChangeDisplay)
            {
                startGroup.active = false;
                defenseGroup.active = true;
                resourceGatherGroup.active = false;
                miscGroup.active = false;
            }
        }
        else if (menuState == MenuState.misc)
        {
            if (canChangeDisplay)
            {
                startGroup.active = false;
                defenseGroup.active = false;
                resourceGatherGroup.active = false;
                miscGroup.active = true;
            }
        }

        // if we are holding the lever, we need to freeze the rest of the buy menu, so we can actually pull the lever
        if (s_Worker.ObjectWeAreHolding)
        {
            if (s_Worker.ObjectWeAreHolding.tag == "FreezeBuyScreenWhenHeld")
            {
                if (!rb.isKinematic)
                {
                    rb.isKinematic = true;
                }
            }
        }
        if (s_Worker.ObjectWeAreHolding == null)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }
        }
    }

    public void ClickButton(string buttonName)
    {
        if (buttonName == "Butt_Back")
        {
            menuState = MenuState.start;
            canChangeDisplay = true;
        }

        if (buttonName == "Butt_Resource")
        {
            menuState = MenuState.resourceGather;
            canChangeDisplay = true;
        }

        if (buttonName == "Butt_Misc")
        {
            menuState = MenuState.misc;
            canChangeDisplay = true;
        }

        if (buttonName == "Butt_Defense")
        {
            menuState = MenuState.defense;
            canChangeDisplay = true;
        }

        if (buttonName == "Butt_Axe")
        {
            shipmentQueue += 1;
            menuState = MenuState.start;
            canChangeDisplay = true;
        }
    }

    /*
    Here is the legend for the crates, which numbers they relate to:
    1: axe crate

    */
}
