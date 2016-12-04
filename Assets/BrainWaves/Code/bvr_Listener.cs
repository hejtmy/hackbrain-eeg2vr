#define DEBUG

using oscReceiver;
using UnityEngine;

public class bvr_Listener : Singleton<bvr_Listener> {
    public EegOscReceiver Receiver;

    public delegate void AlfaHandler(double value);
    public event AlfaHandler AlfaChanged;

    // Use this for initialization
    void Awake () {
        Receiver = new EegOscReceiver(55056);
        Receiver.StartReceiving();
        Subscribe();
    }
    void Subscribe()
    {
        EegOscReceiver.ActiveFocusUpEvent += onUp;
        EegOscReceiver.BrainExcitementLevelEvent += alfa;
    }

    void OnApplicationQuit()
    {
        Receiver.StopReceiving();
    }

    private void alfa(double eventData)
    {
        Debug.Log("Alfa:" + eventData);
        if (AlfaChanged != null) AlfaChanged(eventData);
    }

    private void onUp(double eventData)
    {
        Debug.Log("brain excited called:" + eventData);
    }

    private void GyroY(double diff)
    {

    }

    private void GyroX(double diff)
    {

    }

    public bool IsConnected()
    {
        return Receiver.IsConnected();
        //else return true;
    }
}
