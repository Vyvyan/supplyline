using UnityEngine;
using System.Collections;

public class s_Resource : MonoBehaviour {

	Rigidbody rb;
	public bool isAConveyorPack;
	public int NumberOfConveyorsLeftInPack;
	public enum ConveyorType{small, medium, large, xlarge, corner};
	public ConveyorType conveyorType;
    public bool isGrabable;
    public bool isWood;
    public bool isTool;
    public float woodPower;
    public bool isBranches;
    public float branchesPower;
    public bool isStone;
    public float stonePower;
    public bool isGem;
    public float gemPower;
    public bool isFood;
    public float foodPower;
    public float resourceHealth;
    public bool isOnFire;
    float fireResistance = 3;
    public bool fireProof;

    // non public variables for fire
    GameObject ourFireParticles, ourSmokeParticles;
    Light ourLightWhenOnFire;

	// Use this for initialization
	void Start () 
	{
		rb = gameObject.GetComponent<Rigidbody>();
		NumberOfConveyorsLeftInPack = 5;

        // if we are wood or branches, de-parent the object. (helps with having skewed visuals on rotations when scaling the physics objects)
        if (isWood || isBranches)
        {
            gameObject.transform.parent = null;
        }

        // sets the healt of resources based on what they are, so we don't have to deal with finding the prefabs and stuff
        if(isWood)
        {
            resourceHealth = woodPower * 3;
        }
        else if (isBranches)
        {
            resourceHealth = branchesPower * 2;
        }
        else if (isTool)
        {
            resourceHealth = 2;
        }
        else if (isStone || isGem)
        {
            resourceHealth = 100;
            // makes stone automatically fireproof
            fireProof = true;
        }
        else
        {
            resourceHealth = 1;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if this resouce is a conveyor pack, and it's depleted, destroy it
		if (NumberOfConveyorsLeftInPack <= 0)
		{
			Destroy(gameObject);
		}

        // OH SNAP ARE WE ON FIRE? NOBODY PANIC
        if (isOnFire)
        {
            // if we haven't spawned our particles yet
            if (!ourFireParticles)
            {
                // spawn our fire WHICH IS LOCATED IN THE GAME MANAGER GAME OBJECT IN THE HUD SCRIPT, SO WE CAN FIND IT WITHOUT ASSIGNING IT TO EVERY RESOURCE, ONLY WHEN WE NEED
                ourFireParticles = Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<s_HUD>().fire_Particles, gameObject.transform.position, Quaternion.identity)
                    as GameObject;
                // change the scale of the fire based on what is on fire
                ourFireParticles.transform.localScale = gameObject.transform.localScale * 1.5f;
                // parent the fire to the game object so it follows it
                ourFireParticles.transform.parent = gameObject.transform;
                // change our tag so we can light other things on fire
                gameObject.tag = "On Fire";

                // spawns a light so the fire gives off, well, light.
                ourLightWhenOnFire = gameObject.AddComponent<Light>();
                ourLightWhenOnFire.type = LightType.Point;
                ourLightWhenOnFire.range = 50;
                ourLightWhenOnFire.color = Color.yellow;
                ourLightWhenOnFire.intensity = 1;
                ourLightWhenOnFire.renderMode = LightRenderMode.ForcePixel;
                StartCoroutine(FireFlicker());

                // spawns the heat volume to warm the player, we're making it a new child object to not mess with colliders/tags of the main resource object
                GameObject childObject = new GameObject();
                // move it to where we need it
                childObject.transform.position = gameObject.transform.position;
                // parent it to us
                childObject.transform.parent = gameObject.transform;
                // add the trigger collider to the child
                SphereCollider heatCollider = childObject.AddComponent<SphereCollider>();
                heatCollider.isTrigger = true;
                heatCollider.gameObject.tag = "Warmer";
                heatCollider.radius = 8;
            }
            if (!ourSmokeParticles)
            {

            }

            // lower the durability of us, so we can burn up
            resourceHealth -= .1f * Time.deltaTime;
        }

        // if we aren't on fire, but our fire resistance is 0, it means we need to light ourselves on fire
        if (!isOnFire)
        {
            // if we aren't fire proof
            if (!fireProof)
            {
                if (fireResistance <= 0)
                {
                    isOnFire = true;
                }
            }
        }

        // if we have no health, destroy us
        if (resourceHealth <= 0)
        {
            Destroy(gameObject);
        }
	}

	void OnCollisionStay(Collision other)
	{
		if (other.gameObject.tag == "Belt")
		{
			s_ConveyorBelt tempBeltScript = other.gameObject.GetComponent<s_ConveyorBelt>();
			rb.velocity = Vector3.zero;
			rb.AddForce(tempBeltScript.beltDirectionVector3 * (tempBeltScript.beltSpeed * Time.deltaTime), ForceMode.Impulse);
		}

        // WE WILL NEED TO HAVE A WARM TRIGGER VOLUME AROUND THE OBJECT ON FIRE, SO MIGHT AS WELL DO AN EVEN CLOSE COMPARISON FOR CATCHING FIRE, SINCE THIS SEEMS TO BE BUGGY
        // if we touch something on fire
        if (other.gameObject.tag == "On Fire")
        {
            // tick up our On Fire thing, so we catch fire eventually if we stay in the fire
            if (fireResistance > 0)
            {
                fireResistance -= Time.deltaTime;
            }
        }
	}

    void OnCollisionEnter(Collision other)
    {
        // did we touch the object that lights things on fire?
        if (other.gameObject.tag == "Lighter")
        {
            // if we aren't fireproof
            if (!fireProof)
            {
                // light us on fire
                isOnFire = true;
            }
        }
    }

    IEnumerator FireFlicker()
    {
        ourLightWhenOnFire.intensity = Random.Range(.7f, 1.3f);
        yield return new WaitForSeconds(.2f);
        StartCoroutine(FireFlicker());
    }
}
