using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour
{
    public float Speed;
    private bool _shouldRotate;
    void Update()
    {
        if (_shouldRotate) gameObject.transform.Rotate(0, Speed * Time.deltaTime, 0);
    }

    public void StartRotating()
    {
        _shouldRotate = true;
    }

    public void StopRotating()
    {
        _shouldRotate = false;
    }
}
