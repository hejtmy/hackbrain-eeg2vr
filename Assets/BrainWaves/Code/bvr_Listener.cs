#define DEBUG

using oscReceiver;
using UnityEngine;

public class bvr_Listener : Singleton<bvr_Listener> {
    EegOscReceiver _receiver;

    public delegate void AlfaHandler(double value);
    public event AlfaHandler AlfaChanged;

    // Use this for initialization
    void Start () {
        _receiver = new EegOscReceiver(55056);
        EegOscReceiver.ActiveFocusUpEvent += onUp;
        EegOscReceiver.BrainExcitementLevelEvent += alfa;
        _receiver.StartReceiving();
    }

    void OnApplicationQuit()
    {
        _receiver.StopReceiving();
    }

    private void alfa(double eventData)
    {
        Debug.Log("Alfa:" + eventData);
        AlfaChanged(eventData);
    }

    private void onUp(double eventData)
    {
        Debug.Log("brain excited called:" + eventData);
    }

    public bool IsListening()
    {
        return false;
        //else return true;
    }
}
