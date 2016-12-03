using UnityEngine;
using System.Collections;

public class _brain_TestSceneManager : MonoBehaviour {


    public _brain_Object Object1;

    public _brain_DirLight Light1;

    bool _colour_done = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("r")) Object1.Rotate(new Vector3(0, 1, 0), 100);

	    if (Input.GetKeyDown("k"))
        {
            
            if (_colour_done) Object1.SwitchColours(Color.green, Color.yellow, 2);
            else Object1.SwitchColours(Color.yellow, Color.green, 2);
            _colour_done = !_colour_done;
        }

        if (Input.GetKeyDown("o")) Object1.PulseColour(5, 5);

        if (Input.GetKeyDown("m")) Object1.Disappear(3);
        
        if (Input.GetKeyDown("j")) Object1.Appear(3);

        if (Input.GetKeyDown("l")) Light1.ChangeColor(Color.green, 3);

        if (Input.GetKeyDown("p")) Light1.Rotate(Vector3.right, 50);
    }
}
