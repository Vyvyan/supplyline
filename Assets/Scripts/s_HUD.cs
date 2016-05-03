using UnityEngine;
using System.Collections;

public class s_HUD : MonoBehaviour {

	public Texture2D texture_Crosshair;
	public float crosshairSize;

    // we hold some global variables here to access from all scripts since reasons
    public GameObject fire_Particles, smoke_Particles;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		 
	}

	// GUI
	void OnGUI()
	{
		GUI.DrawTexture(new Rect((Screen.width / 2) - (crosshairSize / 2), (Screen.height / 2) - (crosshairSize / 2), 
		                         crosshairSize,crosshairSize), texture_Crosshair);
	}
}
