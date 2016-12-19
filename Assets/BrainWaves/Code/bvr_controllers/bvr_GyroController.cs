using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using SharpOSC;
using System;

public class bvr_GyroController : MonoBehaviour {

    public RigidbodyFirstPersonController rgcontrol;
    bvr_Listener Listener;

    float Y_axis;
    float X_axis;

    float X_diff;
    float Y_diff;

    public float Y_sensitivity = 0.01f;
    public float X_sensitivity = 0.01f;

    Vector3 rotation;

	// Use this for initialization
	void Start () {
        Listener = bvr_Listener.Get;
        if (rgcontrol == null) rgcontrol = gameObject.GetComponent<RigidbodyFirstPersonController>();

        if (Listener.IsConnected()) rgcontrol.enabled = false;
        Subscribe();
        var oldRotation = gameObject.transform.rotation;
    }
	
    void Subscribe()
    {
        Listener.Receiver.AddAction("/GyroscopeX", GyroscopeX);
        Listener.Receiver.AddAction("/GyroscopeY", GyroscopeY);
    }

    private double parseDoubleFromString(string s)
    {
        double outputDouble;

        if (!Double.TryParse(s, out outputDouble))
        {
            throw new InvalidCastException("Not able to parse double from input string.");
        }
        return outputDouble;
    }

    private void GyroscopeY(OscBundle data)
    {
        double gyroValue = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());
        //Movign camera
        Debug.Log("Receiving Y" + gyroValue);
        Y_diff = (Y_axis - (float)gyroValue) * Y_sensitivity;
        Y_axis = (float)gyroValue;
        rotation = new Vector3(rotation.x, rotation.y + Y_diff, rotation.z);
    }

    private void GyroscopeX(OscBundle data)
    {
        double gyroValue = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());
        //Movign camera
        Debug.Log("Receiving X" + gyroValue);
        X_diff = (X_axis - (float)gyroValue) * X_sensitivity;
        X_axis = (float)gyroValue;
        rotation = new Vector3(rotation.x + X_diff, rotation.y, rotation.z);
    }

    // Update is called once per frame
    void LateUpdate () {
        gameObject.transform.Rotate(Vector3.up, X_diff);
        gameObject.transform.Rotate(Vector3.forward, Y_diff);

    }
}
