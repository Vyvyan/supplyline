using UnityEngine;
using System.Collections;
using System.IO;

public class s_HomeBase : MonoBehaviour {

    // FOR REFERENCE, Our Class Types are: cook, carpenter, outfitter, blacksmith, mage, miner

    string[] firstNames, lastNames, likesAndDislikes;

    public static int townWood, townStone, townGems, townGold, townFood, townMetal, townLeather;
    public static int cooks, carpenters, outfitters, blacksmiths, mages, miners;

    public GameObject citizenFlyer, flyerSpawnLocation;

    // Use this for initialization
    void Start ()
    {
        firstNames = File.ReadAllLines("Assets/Text Files/First Names.txt");
        lastNames = File.ReadAllLines("Assets/Text Files/Last Names.txt");
        likesAndDislikes = File.ReadAllLines("Assets/Text Files/Likes and Dislikes.txt");

        Debug.Log(likesAndDislikes.Length);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.M))
        {
            GetRandomName();
            GetPersonalityTraits();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        // if the object touching us is a resource
        if (other.gameObject.GetComponent<s_Resource>())
        {
            // get the resource script
            s_Resource tempResScript = other.gameObject.GetComponent<s_Resource>();

            // if we are wood, add our wood power to the towns
            if(tempResScript.isWood)
            {
                townWood += Mathf.RoundToInt(tempResScript.woodPower);
            }
            // if we are stone, add our stone power to the towns
            if (tempResScript.isStone)
            {
                townStone += Mathf.RoundToInt(tempResScript.stonePower);
            }
            // if we are gem, add our gem power to the towns
            if (tempResScript.isGem)
            {
                townGems += Mathf.RoundToInt(tempResScript.gemPower);
            }
            // if we are gold, add our gold power to the towns
            if (tempResScript.isGold)
            {
                townGold += Mathf.RoundToInt(tempResScript.goldPower);
            }
            // if we are food, add our food power to the towns
            if (tempResScript.isFood)
            {
                townFood += Mathf.RoundToInt(tempResScript.foodPower);
            }
            // if we are metal, add our metal power to the towns
            if (tempResScript.isMetal)
            {
                townMetal += Mathf.RoundToInt(tempResScript.metalPower);
            }
            // if we are leather, add our leather power to the towns
            if (tempResScript.isLeather)
            {
                townLeather += Mathf.RoundToInt(tempResScript.leatherPower);
            }

            // destroy the object we are returning, unless it can't be destroyed at town
            if (!tempResScript.isTool)
            {
                Destroy(other.gameObject);
            }

            if (tempResScript.isPerson)
            {
                if (tempResScript.personType == s_Resource.PersonType.blacksmith)
                {
                    SpawnFlyer("Blacksmith");
                    blacksmiths++;
                }
                if (tempResScript.personType == s_Resource.PersonType.carpenter)
                {
                    SpawnFlyer("Carpenter");
                    carpenters++;
                }
                if (tempResScript.personType == s_Resource.PersonType.cook)
                {
                    SpawnFlyer("Cook");
                    cooks++;
                }
                if (tempResScript.personType == s_Resource.PersonType.mage)
                {
                    SpawnFlyer("Mage");
                    mages++;
                }
                if (tempResScript.personType == s_Resource.PersonType.miner)
                {
                    SpawnFlyer("Miner");
                    miners++;
                }
                if (tempResScript.personType == s_Resource.PersonType.outfitter)
                {
                    SpawnFlyer("Outfitter");
                    outfitters++;
                }

                Destroy(other.gameObject);
            }

            Debug.Log("Wood = " + townWood.ToString() + ", Stone = " + townStone.ToString() + ", Gems = " + townGems.ToString()
                + ", Gold = " + townGold.ToString() + ", Food = " + townFood.ToString() + ", Metal = " + townMetal.ToString() + 
                ", Leather = " + townLeather.ToString());

        }
    }

    public string GetRandomName()
    {
        string first = firstNames[Random.Range(0, firstNames.Length)];
        string last = lastNames[Random.Range(0, lastNames.Length)];
        Debug.Log(first + " " + last);
        return first + " " + last;
    }

    public string GetPersonalityTraits()
    {
        string trait = likesAndDislikes[Random.Range(0, likesAndDislikes.Length)];
        Debug.Log(trait);
        return trait;
    }

    public void SpawnFlyer(string jobTitle)
    {
        GameObject flyer = Instantiate(citizenFlyer, flyerSpawnLocation.transform.position, Quaternion.identity) as GameObject;
        s_Flyer flyerScript = flyer.GetComponent<s_Flyer>();

        flyerScript.name_string = GetRandomName();
        flyerScript.job_string = jobTitle;
        flyerScript.likes1_string = GetPersonalityTraits();
        flyerScript.likes2_string = GetPersonalityTraits();
        flyerScript.dislikes_string = GetPersonalityTraits();
    }
}
