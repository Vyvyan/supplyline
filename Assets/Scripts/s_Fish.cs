using UnityEngine;
using System.Collections;

public class s_Fish : MonoBehaviour {

    //this script controls the fish flop fish behavior

    float fishLifeTime;
    Rigidbody rb;
    bool canFlop;
    float fishFlopTimer_Current, fishFlopTimer_Max;

	// Use this for initialization
	void Start ()
    {
        canFlop = false;
        rb = gameObject.GetComponent<Rigidbody>();
        fishLifeTime = 20;
        NewFishFlopTimer();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (fishLifeTime > 0)
        {
            // countdown
            fishLifeTime -= Time.deltaTime;

            if (fishFlopTimer_Current > 0)
            {
                fishFlopTimer_Current -= Time.deltaTime;
            }
            else
            {
                if (canFlop)
                {
                    rb.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 3, Random.Range(-2, 2)), ForceMode.Impulse);
                    rb.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)));
                }
                NewFishFlopTimer();
            }
        }
	}

    void NewFishFlopTimer()
    {
        fishFlopTimer_Max = Random.Range(.2f, 1f);
        fishFlopTimer_Current = fishFlopTimer_Max;
    }

    void OnCollisionEnter(Collision other)
    {
        canFlop = true;
    }
}
