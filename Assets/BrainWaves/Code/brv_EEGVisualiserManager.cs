using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class brv_EEGVisualiserManager : MonoBehaviour
{
    private Component[] visualisers;
    public Camera Camera;
    void Start()
    {
        if (Camera == null) Camera = Camera.main;
        visualisers = gameObject.GetComponentsInChildren<brv_EEGVisualiser>();
    }

    public void StartReadingEEGs()
    {
        Camera.GetComponent<RotateCamera>().StartRotating();
        foreach (brv_EEGVisualiser visualiser in visualisers)
            visualiser.StartReading();
    }

    public void StopReadingEEGs()
    {
        Camera.GetComponent<RotateCamera>().StopRotating();
        foreach (brv_EEGVisualiser visualiser in visualisers)
            visualiser.StopReading();
    }
}
