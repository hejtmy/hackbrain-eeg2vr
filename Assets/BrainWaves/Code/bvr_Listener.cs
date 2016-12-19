#define DEBUG
using UnityEngine;
using SharpOSC;
using System;

public class bvr_Listener : Singleton<bvr_Listener>
{
    public OscReceiver Receiver;

    public delegate void AlfaHandler(double value);
    public event AlfaHandler AlfaChanged;

    // Use this for initialization
    void Awake()
    {
        Receiver = new OscReceiver(12345);
        Subscribe();
        Receiver.StartReceiving();
    }

    void Subscribe()
    {
        Receiver.AddAction("/onUp", onUp);
        Receiver.AddAction("/alfa", alfa);
    }

    void OnApplicationQuit()
    {
        Receiver.StopReceiving();
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

    private void alfa(OscBundle data)
    {
        // expecting 1 message with 1 argument of type double ~ this info has to be hardcoded. Get the info from OpenVIBE settings.
        double alfaValue = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());

        Debug.Log("Alfa:" + alfaValue);
        if (AlfaChanged != null) AlfaChanged(alfaValue);
    }

    private void onUp(OscBundle data)
    {
        Debug.Log("brain excited called:" + data);
    }

    private void GyroY(OscBundle data)
    {

    }

    private void GyroX(OscBundle data)
    {

    }

    public bool IsConnected()
    {
        return Receiver.IsReceiving();
        //else return true;
    }
}
