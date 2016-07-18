using UnityEngine;
using System.Collections;

public class s_FishDrill : MonoBehaviour {

    public GameObject fish, drill, spawnLocation;
    bool onIce;

    public float fishTimer_Current, fishTimer_Max;

    float spinAmount;

	// Use this for initialization
	void Start ()
    {
        GetNewFishTimer();
        spinAmount = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // spin drill
        drill.transform.Rotate(Vector3.down * spinAmount);

        if (spinAmount < 0) { spinAmount = 0; }

        if (onIce)
        {
            // rev up on the drill
            if (spinAmount < 30)
            {
                spinAmount += (3 * Time.deltaTime);
            }

            // if we are above zero on the counter, then count down
            if (fishTimer_Current > 0)
            {
                fishTimer_Current -= (1 * Time.deltaTime);
            }
            // else we spawn a fish and get a new timer
            else
            {
                // spawn fish
                GameObject temp = Instantiate(fish, spawnLocation.transform.position, Quaternion.identity) as GameObject;
                // add a force to the fish
                temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), 7, Random.Range(-5, 5)), ForceMode.Impulse);
                temp.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)));
                GetNewFishTimer();
            }
        }
        // not on ice
        else
        {
            // rev down on the drill
            if (spinAmount > 0)
            {
                spinAmount -= (8 * Time.deltaTime);
            }
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ice")
        {
            onIce = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ice")
        {
            onIce = false;
        }
    }

    void GetNewFishTimer()
    {
        fishTimer_Max = Random.Range(5f, 120f);
        fishTimer_Current = fishTimer_Max;
    }
}
