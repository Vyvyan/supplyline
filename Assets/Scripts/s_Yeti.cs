using UnityEngine;
using System.Collections;

public class s_Yeti : MonoBehaviour {

    public GameObject deadYeti;
    public int health;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // is our yeti dead?
        if (health <= 0)
        {
            //kill that foool, son.
            KillTheYeti();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // if we are a weapon, then we don't have to hit it as hard to kill it
        if (other.gameObject.tag == "Weapon")
        {
            if (other.relativeVelocity.magnitude > 3)
            {
                Debug.Log("hit with a weapon");
                health--;
            }
        }
        // if we get hit by a wisp projectile
        else if (other.gameObject.tag == "Wisp Projectile")
        {
            Debug.Log("YAY WISP HIT US! WEEE MILA IS A SEXY MAN");
            s_WispProjectile tempScript = other.gameObject.GetComponent<s_WispProjectile>();
            health -= tempScript.damage;
            Debug.Log("This Yeti has " + health.ToString() + " health left.");
        }
        // but if we aren't a weapon, then it can die to anything as long as it hits it hard enough
        else if (other.relativeVelocity.magnitude > 6)
        {
            Debug.Log("hit hard enough");
            health--;
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
