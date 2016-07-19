using UnityEngine;
using System.Collections;
using System.IO;

public class s_HomeBase : MonoBehaviour {

    string[] firstNames, lastNames;

	// Use this for initialization
	void Start ()
    {
        firstNames = File.ReadAllLines("Assets/Names/First Names.txt");
        lastNames = File.ReadAllLines("Assets/Names/Last Names.txt");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Your new Citizen is: " + firstNames[Random.Range(0, firstNames.Length + 1)] + " " + lastNames[Random.Range(0, lastNames.Length + 1)]);
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
                s_GameManager.townWood += Mathf.RoundToInt(tempResScript.woodPower);
            }
            // if we are stone, add our stone power to the towns
            if (tempResScript.isStone)
            {
                s_GameManager.townStone += Mathf.RoundToInt(tempResScript.stonePower);
            }
            // if we are gem, add our gem power to the towns
            if (tempResScript.isGem)
            {
                s_GameManager.townGems += Mathf.RoundToInt(tempResScript.gemPower);
            }
            // if we are gold, add our gold power to the towns
            if (tempResScript.isGold)
            {
                s_GameManager.townGold += Mathf.RoundToInt(tempResScript.goldPower);
            }
            // if we are food, add our food power to the towns
            if (tempResScript.isFood)
            {
                s_GameManager.townFood += Mathf.RoundToInt(tempResScript.foodPower);
            }
            // if we are metal, add our metal power to the towns
            if (tempResScript.isMetal)
            {
                s_GameManager.townMetal += Mathf.RoundToInt(tempResScript.metalPower);
            }
            // if we are leather, add our leather power to the towns
            if (tempResScript.isLeather)
            {
                s_GameManager.townLeather += Mathf.RoundToInt(tempResScript.leatherPower);
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
                    // stuff
                }
            }

            Debug.Log("Wood = " + s_GameManager.townWood.ToString() + ", Stone = " + s_GameManager.townStone.ToString() + ", Gems = " + s_GameManager.townGems.ToString()
                + ", Gold = " + s_GameManager.townGold.ToString() + ", Food = " + s_GameManager.townFood.ToString() + ", Metal = " + s_GameManager.townMetal.ToString() + 
                ", Leather = " + s_GameManager.townLeather.ToString());

        }
    }
}
