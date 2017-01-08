using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bvr_SceneSetup : MonoBehaviour {

	// Needs to be in Start, as most of things are instantiating in Awake - this is last resort
	void Start ()
	{
	    bvr_Listener.GetOrCreate(gameObject);
	}
	
}
