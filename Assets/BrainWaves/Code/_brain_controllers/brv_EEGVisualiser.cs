using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class brv_EEGVisualiser : MonoBehaviour {

    public float Amplitude;
    private Vector3 _startPosition;
    public string EEGPath;
    private List<float> _eegSignal;
    private int _currentPoint;
    private int _length;
    public int sample;
    public bool ReadEEG;

    void Start()
    {
        _eegSignal = FileReader.ReadEEG(EEGPath);
        _startPosition = transform.localPosition;
        _currentPoint = 0;
        _length = _eegSignal.Count;
        ReadEEG = false;

        gameObject.GetComponent<TrailRenderer>().enabled = false;
    }

    void Update()
    {
        if (ReadEEG)
        {
            _currentPoint += 1;
            if (_currentPoint >= _length) _currentPoint = 0;
            if (_currentPoint % sample != 0) return;
            var newPoint = _eegSignal[_currentPoint];
            gameObject.transform.localPosition = new Vector3(_startPosition.x, _startPosition.y + newPoint * Amplitude, _startPosition.z);
        }

    }

    public void StartReading()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        ReadEEG = true;
    }

    public void StopReading()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        ReadEEG = false;
    }
}
