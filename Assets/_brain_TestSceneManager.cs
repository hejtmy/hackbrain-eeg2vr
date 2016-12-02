using UnityEngine;
using System.Collections;

public class _brain_TestSceneManager : MonoBehaviour {


    public _brain_Object Object1;

    bool _colour_done = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("k"))
        {
            Object1.Rotate(2, 10);
            if (_colour_done) Object1.SwitchColours(Color.green, Color.yellow, 2);
            else Object1.SwitchColours(Color.yellow, Color.green, 2);
            _colour_done = !_colour_done;
        }
        if (Input.GetKeyDown("o"))
        {
            Object1.PulseColour(5, 5);
        }
	}
}
