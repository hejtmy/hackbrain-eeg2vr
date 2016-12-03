using oscReceiver;
using UnityEngine;

public class _brain_Listener : MonoBehaviour {
    EegOscReceiver receiver;

    public delegate void AlfaHandler(double value);
    public event AlfaHandler AlfaChanged;

    // Use this for initialization
    void Start () {
        receiver = new EegOscReceiver(55056);
        EegOscReceiver.ActiveFocusUpEvent += onUp;
        EegOscReceiver.BrainExcitementLevelEvent += alfa;
        receiver.StartReceiving();
    }

    void OnApplicationQuit()
    {
        receiver.StopReceiving();
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

    // Update is called once per frame
    void Update () {
	
	}
}
