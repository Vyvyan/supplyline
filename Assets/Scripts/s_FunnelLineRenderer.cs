using UnityEngine;
using System.Collections;

public class s_FunnelLineRenderer : MonoBehaviour {

    s_StretchToPoint funnelPostScript;
    LineRenderer line;

	// Use this for initialization
	void Start ()
    {
        funnelPostScript = GetComponentInParent<s_StretchToPoint>();
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        line.SetPosition(0, funnelPostScript.gameObject.transform.position);
        line.SetPosition(1, funnelPostScript.EndPoint.position);
	}
}
