using UnityEngine;
using System.Collections;

public class BrainWave : MonoBehaviour {  
    public Color Color;
    public float Speed = 1;
    public float Strength;

    public bool _moving;
    private Vector3 _direction = new Vector3(1, 0, 0);

    void Update()
    {
        if (_moving) transform.Translate(_direction * Time.deltaTime * Speed);
    }

    public void Move(Vector3 direction, float time)
    {
        _direction = direction;
        _moving = true;
        Destroy(this, time);
    }
}
