using UnityEngine;
using System.Collections;

public class s_BuyMenu : MonoBehaviour {

    public enum MenuState {start, defense, resourceGather, misc};
    public MenuState menuState;

    public GameObject beacon;

    float shippingPower;

    public Transform shippingCatapultLocation;

    public GameObject itemToShip;

    // our collection of power bar indicators
    public GameObject[] powerBars;

    s_StretchToPoint stretchScript;

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
        stretchScript = gameObject.GetComponent<s_StretchToPoint>();
        // references where the catapult is, basically our location to fire from
        shippingCatapultLocation = GameObject.FindGameObjectWithTag("ShippingCatapult").transform;

        // start off with lowest power
        //DisplayPowerBars(0);
        shippingPower = .5f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        beacon.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 30, gameObject.transform.position.z);
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

    public void ClickButton(string buttonName, GameObject clickedGameObject)
    {
        if (buttonName == "Butt_Back")
        {
            menuState = MenuState.start;
            canChangeDisplay = true;
        }

        else if (buttonName == "Butt_Resource")
        {
            menuState = MenuState.resourceGather;
            canChangeDisplay = true;
        }

        else if (buttonName == "Butt_Misc")
        {
            menuState = MenuState.misc;
            canChangeDisplay = true;
        }

        else if (buttonName == "Butt_Defense")
        {
            menuState = MenuState.defense;
            canChangeDisplay = true;
        }

        else if (buttonName == "Butt_Axe")
        {
            StartCoroutine(ShipObject(1));
            menuState = MenuState.start;
            canChangeDisplay = true;
        }


        /* WE MAY NOT NEED THIS CODE FOR THE POWER. SEEMS LIKE IT'S HITTING PRETTY WELL AT DEFAULT POWER
        // for the power meter on the side of the menu, we use one lower than our actual power on the button because ARRAYS ARE DUMB
        else if (buttonName == "1")
        {
            DisplayPowerBars(0);
            shippingPower = .5f;
        }

        else if (buttonName == "2")
        {
            DisplayPowerBars(1);
            shippingPower = 25;
        }

        else if (buttonName == "3")
        {
            DisplayPowerBars(2);
            shippingPower = 50;
        }

        else if (buttonName == "4")
        {
            DisplayPowerBars(3);
        }

        else if (buttonName == "5")
        {
            DisplayPowerBars(4);
        }

        else if (buttonName == "6")
        {
            DisplayPowerBars(5);
        }

        else if (buttonName == "7")
        {
            DisplayPowerBars(6);
        }

        else if (buttonName == "8")
        {
            DisplayPowerBars(7);
        }

        else if (buttonName == "9")
        {
            DisplayPowerBars(8);
        }

        else if (buttonName == "10")
        {
            DisplayPowerBars(9);
        }
        */
    }

    void DisplayPowerBars(int buttonNumber)
    {
        int i = buttonNumber;

        // make all of the power bars invisible
        foreach (GameObject ob in powerBars)
        {
            ob.SetActive(false);
        }

        // we count down using this int 
        while (i >= 0)
        {
            powerBars[i].SetActive(true);
            i--;
        }
    }

    IEnumerator ShipObject(int itemCode)
    {
        // we wait a short time, and then launch the thing
        yield return new WaitForSeconds(3f);

        // instantiate the standard crate
        GameObject temp = Instantiate(itemToShip, shippingCatapultLocation.position, Quaternion.identity) as GameObject;
        // get it's rigid body to launch it
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        // get it's crate script so we can tell it what type of crate it is
        s_Crate tempCrateScript = temp.GetComponent<s_Crate>();
        // assign the item code to the newly created crate
        tempCrateScript.itemCode = itemCode;

        Vector3 direction = beacon.transform.position - shippingCatapultLocation.transform.position;

        rb.velocity = new Vector3(direction.x / 2, 45, direction.z / 2) * shippingPower;
        rb.AddRelativeTorque(new Vector3(Random.Range(0,100), Random.Range(0, 100), Random.Range(0, 100)));

        Debug.Log(shippingPower.ToString());
    }

    /*

    Item Code Legend:
    Axe Crate - 1


    */
}
