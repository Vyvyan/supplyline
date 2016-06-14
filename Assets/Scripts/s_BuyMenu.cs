using UnityEngine;
using System.Collections;

public class s_BuyMenu : MonoBehaviour {

    public enum MenuState {start, defense, resourceGather, misc};
    public MenuState menuState;
    public MenuState menuStateLastFrame;

    public GameObject startGroup, defenseGroup, resourceGatherGroup, miscGroup;

	// Use this for initialization
	void Start ()
    {
        menuState = MenuState.start;
        defenseGroup.active = false;
        resourceGatherGroup.active = false;
        miscGroup.active = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (menuState == MenuState.start)
        {
            if (startGroup.active == false)
            {
                startGroup.active = true;
                defenseGroup.active = false;
                resourceGatherGroup.active = false;
                miscGroup.active = false;
            }
        }
	}
}
