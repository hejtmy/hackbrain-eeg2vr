using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bvr_EEGVisualiserManager : MonoBehaviour
{
    private Component[] visualisers;
    public Camera Camera;
    void Start()
    {
        if (Camera == null) Camera = Camera.main;
        visualisers = gameObject.GetComponentsInChildren<bvr_EEGVisualiser>();
    }

    public void StartReadingEEGs()
    {
        Camera.GetComponent<RotateCamera>().StartRotating();
        foreach (bvr_EEGVisualiser visualiser in visualisers)
            visualiser.StartReading();
    }

    public void StopReadingEEGs()
    {
        Camera.GetComponent<RotateCamera>().StopRotating();
        foreach (bvr_EEGVisualiser visualiser in visualisers)
            visualiser.StopReading();
    }
}
