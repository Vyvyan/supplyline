using UnityEngine;
using System.Collections;

public class s_Worker : MonoBehaviour {

    public Camera playersCamera;
    public Transform objectHoldLocation, buildObjectHoldLocation;
    public bool isHoldingAnObject;
    public GameObject heldItem;
    public Rigidbody heldItemRB;
    s_Resource conveyorPackScript;
    public float objectMoveToHoldPositionSpeed;
    public SpringJoint holdJoint;

    public float playerWarmth;
    bool isInHeatVolume;
    public float maxPlayerWarmth;


    public enum PlayerState { playing, building, connectingTowers };
    public PlayerState playerState;
    public GameObject conveyorBeltObject;


    //flags
    bool hasGottenOurBuildingReferenceObject;

    // bool for corner rotations
    public bool rotateCornerOppositeWay;

    // more dumb stuff
    GameObject tempConveyor;
    s_Conveyor tempConvScript;

    // variables for connecting transport poles
    s_StretchToPoint startingPole;

    // Use this for initialization
    void Start()
    {
        playersCamera = Camera.main;
        isHoldingAnObject = false;
        playerState = PlayerState.playing;
        hasGottenOurBuildingReferenceObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if we are in a heat volume, heat up, if not, cool down
        if (isInHeatVolume)
        {
            if (playerWarmth <= maxPlayerWarmth)
            {
                playerWarmth += Time.deltaTime * 2;
            }
        }
        else
        {
            playerWarmth -= Time.deltaTime * .25f;
        }

        ///////////
        // RAYCASTING FROM CENTER OF CAMERA
        ///////////
        Ray ray = playersCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        // IF WE ARE PLAYING
        if (playerState == PlayerState.playing)
        {
            // reset our has gotten our preview object flag
            hasGottenOurBuildingReferenceObject = false;

            if (Physics.Raycast(ray, out hit, 3f))
            {
                // are we looking at a button on the conveyor belt?
                if (hit.transform.gameObject.tag == "Belt Direction Button")
                {
                    // if we are clicking
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        // change the belt direction
                        hit.transform.gameObject.GetComponentInParent<s_Conveyor>().ChangeBeltDirection();
                    }
                }

                // if we aim at a store button
                else if (hit.collider.tag == "StoreButton")
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        // send the name of the button we clicked to the store script 
                        GameObject.FindGameObjectWithTag("BuyScreen").GetComponent<s_BuyMenu>().ClickButton(hit.collider.gameObject.name.ToString());
                    }
                }

                // does the object we are looking at have a resource script?
                else if (hit.transform.gameObject.GetComponent<s_Resource>() != null)
                {
                    // can we even grab it? THIS IS HERE so we can't grab trees while they are still part of the tree
                    if (hit.transform.gameObject.GetComponent<s_Resource>().isGrabable)
                    {
                        if (!isHoldingAnObject)
                        {
                            // are we holding down left click?
                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                heldItem = hit.transform.gameObject;
                                heldItemRB = heldItem.GetComponent<Rigidbody>();
                                //heldItemRB.isKinematic = true;

                                // CREATE A NEW SPRING WHENEVER WE PICK UP AN OBJECT
                                // get the position of the object we are picking up to revert back to after making the spring
                                Vector3 storedPOSofObjectWePickedUp = heldItem.transform.position;
                                // move held item to the hold position
                                heldItem.transform.position = objectHoldLocation.transform.position;
                                // make the spring
                                holdJoint = objectHoldLocation.gameObject.AddComponent<SpringJoint>();
                                holdJoint.connectedBody = heldItemRB;
                                holdJoint.autoConfigureConnectedAnchor = true;
                                holdJoint.anchor = Vector3.zero;
                                holdJoint.connectedAnchor = Vector3.zero;
                                holdJoint.spring = 500;
                                holdJoint.damper = 50f;
                                holdJoint.minDistance = 0;
                                holdJoint.maxDistance = 0;
                                // put the object back where it was
                                heldItem.transform.position = storedPOSofObjectWePickedUp;

                                // if what we just picked up is a conveyor pack
                                if (heldItem.GetComponent<s_Resource>().isAConveyorPack)
                                {
                                    // turn off the collision and renderer for the pack we are holding
                                    heldItem.GetComponent<Renderer>().enabled = false;
                                    heldItem.GetComponent<Collider>().enabled = false;
                                    conveyorPackScript = heldItem.GetComponent<s_Resource>();
                                    // switch player states to building
                                    playerState = PlayerState.building;
                                }
                            }
                        }

                        // if we are trying to connect a tower
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (hit.transform.gameObject.tag == "TransportPole")
                            {
                                // get the transportPole script
                                startingPole = hit.transform.gameObject.GetComponent<s_StretchToPoint>();

                                playerState = PlayerState.connectingTowers;
                            }
                        }
                    }
                }
            }
        }

        // IF WE ARE CONNECTING TOWERS
        if (playerState == PlayerState.connectingTowers)
        {
            if (Physics.Raycast(ray, out hit, 6f))
            {
                // if we raycast a transport pole that IS NOT the pole we started connection mode with (we can't connect to the same pole)
                if (hit.collider.tag == "TransportPole" && hit.collider.gameObject != startingPole.gameObject)
                {
                    // if we press E
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // then connect the two towers
                        startingPole.EndPoint = hit.collider.gameObject.transform.GetChild(0);

                        // now go back to playing
                        playerState = PlayerState.playing;
                    }
                }
            }

            // cancelling connecting towers
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                playerState = PlayerState.playing;
            }
         }


        // IF WE ARE BUILDING
        if (playerState == PlayerState.building)
        {
            // raycast to see what's in front of us
            if (Physics.Raycast(ray, out hit, 6f))
            {
                if (hit.collider.tag == "BuildJoint")
                {
                    Debug.Log("yup");
                }     
            }

            // if we are building, but we don't have an item held anymore, go back to playing. (this is for when
            //a conveyor pack is empty and destroys itself, or for a safeguard I guess, so we don't get stuck in
            // build mode if something weird happens.
            if (heldItem == null)
            {
                playerState = PlayerState.playing;
            }
        }

        // DROPPING objects
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // if we have an item to drop
            if (heldItem)
            {
                // are we holding a conveyor pack?
                if (heldItem.GetComponent<s_Resource>().isAConveyorPack)
                {
                    // turn on the collision and renderer for the pack we are holding
                    heldItem.GetComponent<Renderer>().enabled = true;
                    heldItem.GetComponent<Collider>().enabled = true;
                    // switch player states to playing
                    playerState = PlayerState.playing;
                }

                // drop the item
               
                // destroy the spring
                Destroy(holdJoint);
                heldItem = null;
            }
        }

        // if we are holding an object, then isholdinganobject is true
        isHoldingAnObject = heldItem != null;
    }

    void FixedUpdate()
    {
        // reset the heat volume to false every frame, before the normal updates run
        isInHeatVolume = false;

        // if we are holding an object
        if (heldItem)
        {
            // if we still even have an RB, this is just precautionary, had it throw an error once
            if (heldItemRB)
            {
                // move it with us
                //heldItemRB.MovePosition(objectHoldLocation.position);

                // SLOW THE VELOCITY DOWN BY 20% since the spring joint is a fucking psycho
                heldItemRB.velocity = heldItemRB.velocity * .8f;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // if we are in a heat trigger, then baboosh make us warm. This shuts off in fixed update every frame
        if (other.gameObject.tag == "Warmer")
        {
            isInHeatVolume = true;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, 0, 500, 500), playerWarmth.ToString());
    }
}
