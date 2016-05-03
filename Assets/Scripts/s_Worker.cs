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

    // spawn location for the "building mode" "do you want to build here" conv when it's not showing snapped to a conv
    Vector3 buildPreviewOBJSkyLocation = new Vector3(0, 500, 0);

    public enum PlayerState { playing, building };
    public PlayerState playerState;
    public GameObject convSmall, convMedium, convLarge, convXlarge, convCorner;
    // the actual conveyor we are building, that gets assigned based on what type of conv pack we pick up
    GameObject buildThisConveyor, buildThisConveyor_Preview;

    // variables for the game objects of the preview build conveyor belts, that are always spawned in
    public GameObject ConvSmallPreviewBuildOBJ, ConvMediumPreviewBuildOBJ, ConvLargePreviewBuildOBJ, ConvXlargePreviewBuildOBJ,
    ConvCornerPreviewBuildOBJ;
    float previewBuildHeightOffset;
    float previewBuildVerticalRotationOffset;

    //flags
    bool hasGottenOurBuildingReferenceObject;

    // bool for corner rotations
    public bool rotateCornerOppositeWay;

    // more dumb stuff
    GameObject tempConveyor;
    s_Conveyor tempConvScript;

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

        // scrolling the mouse makes the build object move up and down AND changes the rotation vertically. Shouldn't interfere by doing both at the same time
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            previewBuildHeightOffset += .1f;
            previewBuildVerticalRotationOffset += 1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            previewBuildHeightOffset -= .1f;
            previewBuildVerticalRotationOffset -= 1f;
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

                // does the object we are looking at have a resource script?
                if (hit.transform.gameObject.GetComponent<s_Resource>() != null)
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
                    }
                }
            }
        }

        // IF WE ARE BUILDING
        if (playerState == PlayerState.building)
        {
            // if we haven't gotten our building reference object, get it
            if (!hasGottenOurBuildingReferenceObject)
            {
                // run the choose what we are building function
                ChooseWhatWeAreBuilding();
                // assign the right conveyor to the preview object, so we can see it before placing it
                tempConveyor = buildThisConveyor_Preview;
                // get the script attached to the conveyor we want to build
                tempConvScript = buildThisConveyor_Preview.GetComponent<s_Conveyor>();
                // we've now got our building reference object
                hasGottenOurBuildingReferenceObject = true;
            }

            // raycast to see what's in front of us
            if (Physics.Raycast(ray, out hit, 6f))
            {
                // if we are hitting a trigger object for the build joint
                if (hit.transform.gameObject.tag == "BuildJoint")
                {
                    s_Conveyor ConvWeAreLookingAt = hit.transform.gameObject.GetComponentInParent<s_Conveyor>();

                    // SO IF HIT SOMETHING NAMED TRIGGER 1, WE BUILD AT BJ1's POSITION, TRIGGER 2 is at BJ2's, and so on
                    if (hit.transform.gameObject.name == "Trigger1")
                    {
                        // if we are holding a corner
                        if (tempConvScript.parentConvObj.name == "ParentCorner")
                        {
                            // rotation of corner piece 
                            if (rotateCornerOppositeWay)
                            {
                                tempConvScript.parentConvObj.transform.rotation = ConvWeAreLookingAt.BJ1_Corner.transform.rotation;

                                // move the corner piece to where we need it, offset a bit since we rotate the build joint at it's center
                                tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ1_Corner.transform.position;
                            }
                            else
                            {
                                tempConvScript.parentConvObj.transform.rotation = ConvWeAreLookingAt.BJ1_Corner_Alt.transform.rotation;

                                // move the corner piece to where we need it, offset a bit since we rotate the build joint at it's center
                                tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ1_Corner_Alt.transform.position;
                            }

                            // changes the rotation of the preview conveyor belt
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                rotateCornerOppositeWay = !rotateCornerOppositeWay;
                            }
                            if (Input.GetKeyDown(KeyCode.Q))
                            {
                                rotateCornerOppositeWay = !rotateCornerOppositeWay;
                            }
                        }
                        // if we are holding a non-corner piece
                        else
                        {
                            // then move it to where we want it
                            tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ1.transform.position;
                            // create a rotation variable because quaterion.euler is fuckshit crazy, and this is how we make it work
                            Quaternion initialRot = ConvWeAreLookingAt.BJ1.transform.rotation;
                            // rotate it how we want to
                            tempConvScript.parentConvObj.transform.rotation = initialRot * Quaternion.Euler(ConvWeAreLookingAt.BJ1.transform.rotation.x,
                                                                                             ConvWeAreLookingAt.BJ1.transform.rotation.y,
                                                                                             ConvWeAreLookingAt.BJ1.transform.rotation.z + previewBuildVerticalRotationOffset);
                        }
                    }
                    if (hit.transform.gameObject.name == "Trigger2")
                    {
                        // if we are holding a corner
                        if (tempConvScript.parentConvObj.name == "ParentCorner")
                        {
                            // rotation of corner piece 
                            if (rotateCornerOppositeWay)
                            {
                                tempConvScript.parentConvObj.transform.rotation = ConvWeAreLookingAt.BJ2_Corner.transform.rotation;
                                // move the corner piece to where we need it, offset a bit since we rotate the build joint at it's center
                                tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ2_Corner.transform.position;
                            }
                            else
                            {
                                tempConvScript.parentConvObj.transform.rotation = ConvWeAreLookingAt.BJ2_Corner_Alt.transform.rotation;
                                // move the corner piece to where we need it, offset a bit since we rotate the build joint at it's center
                                tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ2_Corner_Alt.transform.position;
                            }

                            // changes the rotation of the preview conveyor belt
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                rotateCornerOppositeWay = !rotateCornerOppositeWay;
                            }
                            if (Input.GetKeyDown(KeyCode.Q))
                            {
                                rotateCornerOppositeWay = !rotateCornerOppositeWay;
                            }
                        }
                        // if we are holding a non-corner piece
                        else
                        {
                            // then move it to where we want it
                            tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ2.transform.position;
                            // create a rotation variable because quaterion.euler is fuckshit crazy, and this is how we make it work
                            Quaternion initialRot = ConvWeAreLookingAt.BJ2.transform.rotation;
                            // rotate it how we want to
                            tempConvScript.parentConvObj.transform.rotation = initialRot * Quaternion.Euler(ConvWeAreLookingAt.BJ2.transform.rotation.x,
                                                                                             ConvWeAreLookingAt.BJ2.transform.rotation.y,
                                                                                             ConvWeAreLookingAt.BJ2.transform.rotation.z + previewBuildVerticalRotationOffset);
                        }
                    }
                    if (hit.transform.gameObject.name == "Trigger3")
                    {
                        // then move it to where we want it
                        tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ3.transform.position;
                        // rotate it how we want to
                        tempConvScript.parentConvObj.transform.rotation = ConvWeAreLookingAt.BJ3.transform.rotation;
                    }
                    if (hit.transform.gameObject.name == "Trigger4")
                    {
                        // then move it to where we want it
                        tempConvScript.parentConvObj.transform.position = ConvWeAreLookingAt.BJ4.transform.position;
                        // rotate it how we want to
                        tempConvScript.parentConvObj.transform.rotation = ConvWeAreLookingAt.BJ4.transform.rotation;
                    }

                    // IF we hit build, actually place the conveyor belt
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // if we have a conveyor left in the pack to build
                        if (conveyorPackScript.NumberOfConveyorsLeftInPack > 0)
                        {
                            // spawn the conveyor belt in our arbitrary position, then move it where we want it. (we do this because parent objects mess with placement
                            GameObject convWeJustBuilt = Instantiate(buildThisConveyor, buildPreviewOBJSkyLocation, Quaternion.identity) as GameObject;
                            // get the script from the conv we just spawned, since it has references to the objects, including the parent object we want to move
                            s_Conveyor convWeJustBuiltScript = convWeJustBuilt.GetComponent<s_Conveyor>();

                            // then move it to where we want it
                            convWeJustBuiltScript.parentConvObj.transform.position = tempConvScript.parentConvObj.transform.position;
                            // rotate it how we want to
                            convWeJustBuiltScript.parentConvObj.transform.rotation = tempConvScript.parentConvObj.transform.rotation;
                            // decrease the number of conveyors left in the pack
                            conveyorPackScript.NumberOfConveyorsLeftInPack--;

                        }
                    }
                }
                // if we raycast hit something but it's NOT a buildjoint
                else
                {
                    // if we aren't looking at a conveyor belt, enter into free build mode
                    tempConvScript.parentConvObj.transform.position = hit.point;

                    // changes the rotation of the preview conveyor belt
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        tempConvScript.parentConvObj.transform.Rotate(tempConvScript.parentConvObj.transform.rotation.x,
                                                                      tempConvScript.parentConvObj.transform.rotation.y + 45f,
                                                                      tempConvScript.parentConvObj.transform.rotation.z);
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        tempConvScript.parentConvObj.transform.Rotate(tempConvScript.parentConvObj.transform.rotation.x,
                                                                      tempConvScript.parentConvObj.transform.rotation.y - 45f,
                                                                      tempConvScript.parentConvObj.transform.rotation.z);
                    }

                    // IF we hit build, actually place the conveyor belt
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // if we have a conveyor left in the pack to build
                        if (conveyorPackScript.NumberOfConveyorsLeftInPack > 0)
                        {
                            // spawn the conveyor belt in our arbitrary position, then move it where we want it. (we do this because parent objects mess with placement
                            GameObject convWeJustBuilt = Instantiate(buildThisConveyor, buildPreviewOBJSkyLocation, Quaternion.identity) as GameObject;
                            // get the script from the conv we just spawned, since it has references to the objects, including the parent object we want to move
                            s_Conveyor convWeJustBuiltScript = convWeJustBuilt.GetComponent<s_Conveyor>();

                            // then move it to where we want it
                            convWeJustBuiltScript.parentConvObj.transform.position = tempConvScript.parentConvObj.transform.position;
                            // rotate it how we want to
                            convWeJustBuiltScript.parentConvObj.transform.rotation = tempConvScript.parentConvObj.transform.rotation;

                            // decrease the number of conveyors left in the pack
                            conveyorPackScript.NumberOfConveyorsLeftInPack--;

                        }
                    }
                }

            }
            // if we didn't hit ANYTHING with the raycast
            else
            {
                // place the temp build object in front of the player if we aren't hitting anything with a ray in front of us
                // we add the offset to the Y, so we can raise the height with the mouse wheel
                tempConvScript.parentConvObj.transform.position = new Vector3(buildObjectHoldLocation.position.x, buildObjectHoldLocation.position.y + previewBuildHeightOffset,
                                                                              buildObjectHoldLocation.position.z);

                // changes the rotation of the preview conveyor belt
                if (Input.GetKeyDown(KeyCode.E))
                {
                    tempConvScript.parentConvObj.transform.Rotate(tempConvScript.parentConvObj.transform.rotation.x,
                                                                  tempConvScript.parentConvObj.transform.rotation.y + 45f,
                                                                  tempConvScript.parentConvObj.transform.rotation.z);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    tempConvScript.parentConvObj.transform.Rotate(tempConvScript.parentConvObj.transform.rotation.x,
                                                                  tempConvScript.parentConvObj.transform.rotation.y - 45f,
                                                                  tempConvScript.parentConvObj.transform.rotation.z);
                }

                // IF we hit build, actually place the conveyor belt
                if (Input.GetKeyUp(KeyCode.F))
                {
                    // if we have a conveyor left in the pack to build
                    if (conveyorPackScript.NumberOfConveyorsLeftInPack > 0)
                    {
                        // spawn the conveyor belt in our arbitrary position, then move it where we want it. (we do this because parent objects mess with placement
                        GameObject convWeJustBuilt = Instantiate(buildThisConveyor, buildPreviewOBJSkyLocation, Quaternion.identity) as GameObject;
                        // get the script from the conv we just spawned, since it has references to the objects, including the parent object we want to move
                        s_Conveyor convWeJustBuiltScript = convWeJustBuilt.GetComponent<s_Conveyor>();

                        // then move it to where we want it
                        convWeJustBuiltScript.parentConvObj.transform.position = tempConvScript.parentConvObj.transform.position;
                        // rotate it how we want to
                        convWeJustBuiltScript.parentConvObj.transform.rotation = tempConvScript.parentConvObj.transform.rotation;

                        // decrease the number of conveyors left in the pack
                        conveyorPackScript.NumberOfConveyorsLeftInPack--;

                    }
                }
            }

            // if we are building, but we don't have an item held anymore, go back to playing. (this is for when
            //a conveyor pack is empty and destroys itself, or for a safeguard I guess, so we don't get stuck in
            // build mode if something weird happens.
            if (heldItem == null)
            {
                playerState = PlayerState.playing;
                // if we are out of building things, then we should put the preview object back up in the sky
                if (buildThisConveyor_Preview)
                {
                    buildThisConveyor_Preview.transform.position = buildPreviewOBJSkyLocation;
                }
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
                    // sets the prewview build object to go back in the sky
                    tempConvScript.parentConvObj.transform.position = buildPreviewOBJSkyLocation;
                    // reset the height offset
                    previewBuildHeightOffset = 0;
                    // reset the height rotation offset
                    previewBuildVerticalRotationOffset = 0;
                    // switch player states to playing
                    playerState = PlayerState.playing;
                }

                // drop the item
                //heldItemRB.isKinematic = false;
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

    // assigns the variables for the type of object we are trying to build
    void ChooseWhatWeAreBuilding()
    {
        s_Resource tempScript = heldItem.GetComponent<s_Resource>();

        if (tempScript.conveyorType == s_Resource.ConveyorType.small)
        {
            buildThisConveyor = convSmall;
            buildThisConveyor_Preview = ConvSmallPreviewBuildOBJ;
        }
        if (tempScript.conveyorType == s_Resource.ConveyorType.medium)
        {
            buildThisConveyor = convMedium;
            buildThisConveyor_Preview = ConvMediumPreviewBuildOBJ;
        }
        if (tempScript.conveyorType == s_Resource.ConveyorType.large)
        {
            buildThisConveyor = convLarge;
            buildThisConveyor_Preview = ConvLargePreviewBuildOBJ;
        }
        if (tempScript.conveyorType == s_Resource.ConveyorType.xlarge)
        {
            buildThisConveyor = convXlarge;
            buildThisConveyor_Preview = ConvXlargePreviewBuildOBJ;
        }
        if (tempScript.conveyorType == s_Resource.ConveyorType.corner)
        {
            buildThisConveyor = convCorner;
            buildThisConveyor_Preview = ConvCornerPreviewBuildOBJ;
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
