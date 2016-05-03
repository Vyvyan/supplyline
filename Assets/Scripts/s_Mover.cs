using UnityEngine;
using System.Collections;

public class s_Mover : MonoBehaviour {

    NavMeshAgent agent;
    GameObject player;

	// Use this for initialization
	void Start ()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        agent.destination = player.transform.position;
    }
}
