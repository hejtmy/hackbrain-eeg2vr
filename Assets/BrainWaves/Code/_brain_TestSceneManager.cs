using UnityEngine;
using System.Collections;
using System;

public class _brain_TestSceneManager : MonoBehaviour {


    public _brain_Object Object1;

    public _brain_DirLight Light1;

    public Animator Anim1;

    bool _colour_done = false;

    public _brain_Object Alchemy;
    public int AlchemyRotSpeed;

    public GameObject player;

    public bvr_Listener listener;

    Vector2 centerPosition;

    bool _rotating = false;

    // Use this for initialization
    void Start () {
        centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        listener.AlfaChanged += AlfaChanged;
    }

    private void AlfaChanged(double value)
    {
        if (value > 10) _rotating = true;
        else _rotating = false;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("r")) Object1.Rotate(new Vector3(0, 1, 0), 100);
        if (Input.GetKeyDown("t"))
        {
            if (_colour_done) Object1.SwitchColours(Color.green, Color.yellow, 2);
            else Object1.SwitchColours(Color.yellow, Color.green, 2);
            _colour_done = !_colour_done;
        }

        if (Input.GetKeyDown("o")) Object1.PulseColour(5, 5);
        if (Input.GetKeyDown("m")) Object1.Disappear(3);
        if (Input.GetKeyDown("j")) Object1.Appear(3);
        if (Input.GetKeyDown("l"))
        {
            AlchemyRotSpeed += 10;
            Alchemy.Rotate(new Vector3(0, 1, 0), AlchemyRotSpeed);
        }
        if (Input.GetKeyDown("k"))
        {
            AlchemyRotSpeed -= 10;
            Alchemy.Rotate(new Vector3(0, 1, 0), AlchemyRotSpeed);

        }
        if (Input.GetKeyDown("p")) Light1.Rotate(new Vector3(0, 1, 0), 50);
        if (Input.GetKeyDown("t")) Anim1.SetBool("doDope", true);
        Raycasing();

        if (_rotating) Object1.Rotate(new Vector3(0, 1, 0), 10);
        else Object1.StopRotating();
    }

    void Raycasing()
    {
        Ray ray = player.GetComponentInChildren<Camera>().ScreenPointToRay(centerPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "BrainObject")
            {
                Object1 = hit.collider.gameObject.GetComponent<_brain_Object>();
            }
            // Do something with the object that was hit by the raycast.
        }
    }

    static void DoSth(double sth)
    {

    }
}
