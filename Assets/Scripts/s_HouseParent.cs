using UnityEngine;
using System.Collections;

public class s_HouseParent : MonoBehaviour {

    
    public GameObject brokenHousePrefab;
    // this variable here is so we don't accidentally spawn 2 broken houses on the same frame, by hitting two child objects at the same time
    bool hasBroken;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void BreakHouse()
    {
        // spawn in the broken house and delete the normal house
        if (!hasBroken)
        {
            Instantiate(brokenHousePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            hasBroken = true;
        }
    }
}
