using UnityEngine;
using System.Collections;

public class s_Yeti : MonoBehaviour {

    public GameObject deadYeti;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        // if we are a weapon, then we don't have to hit it as hard to kill it
        if (other.gameObject.tag == "Weapon")
        {
            if (other.relativeVelocity.magnitude > 3)
            {
                Debug.Log("hit with a weapon");
                KillTheYeti();
            }
        }
        // but if we aren't a weapon, then it can die to anything as long as it hits it hard enough
        else if (other.relativeVelocity.magnitude > 6)
        {
            Debug.Log("hit hard enough");
            KillTheYeti();
        }
    }

    void KillTheYeti()
    {
        // disables the object before destroying it, since Mila is a smart guy and destroy is slow
        gameObject.SetActive(false);
        Instantiate(deadYeti, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - .72f, gameObject.transform.position.z - .13f),
                    gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
