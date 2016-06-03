using UnityEngine;
using System.Collections;

public class s_StretchToPoint : MonoBehaviour {

    public Transform EndPoint;
    Transform trans;
    public GameObject stretchObject;

	// Use this for initialization
	void Start ()
    {
        // might as well reference it now, so we don't do it every frame below
        trans = gameObject.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (EndPoint)
        {
            // if we aren't activated, then activate us
            if (!stretchObject.activeSelf)
            {
                stretchObject.SetActive(true);
            }
            // stretch to the end point
            stretchObject.transform.position = trans.position + (EndPoint.position - trans.position) / 2;
            stretchObject.transform.LookAt(EndPoint.position);
            stretchObject.transform.localScale = new Vector3(1f, 1f, (EndPoint.position - trans.position).magnitude);
        }
        else
        {
            // if we don't have an end goal, then just disable this object
            stretchObject.gameObject.SetActive(false);
        }
	}
}
