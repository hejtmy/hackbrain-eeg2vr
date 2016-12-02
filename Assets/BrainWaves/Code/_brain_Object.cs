using UnityEngine;
using System.Collections;
using System;

public class _brain_Object : MonoBehaviour {

    IEnumerator rotation;
    Material _mat;
	// Use this for initialization
	void Start ()
    {
        _mat = gameObject.GetComponentInChildren<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #region Public API
    public void Resize(float newScale = 1)
    {

    }

    public void SwitchColours(Color color1, Color color2, float duration)
    {
        var switchCol = SwitchingColours(color1, color2, duration);
        StartCoroutine(switchCol);
    }

    internal void PulseColour(float speed, float duration)
    {
        var pulse = PulsingEmission(speed, duration);
        StartCoroutine(pulse);
    }

    public void Rotate(float speed = 1, float time = 0)
    {
        rotation = Rotating(speed, time);
        StartCoroutine(rotation);
    }

    public void StopRotating()
    {
        StopCoroutine(rotation);
    }

    public void ChangeColour(Color color, float speed = 1)
    {

    }

    public void MoveTo(Vector3 where, float speed = 1)
    {

    }
    #endregion

    #region IEnumerators
    IEnumerator SwitchingColours(Color color1, Color color2, float duration)
    {
        var startTime = Time.realtimeSinceStartup;
        while (true)
        {
            var lerpedColor = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1));
            _mat.color = lerpedColor;
            if (Time.realtimeSinceStartup - startTime > duration) break;
            yield return null;
        }
    }

    IEnumerator PulsingEmission(float speed, float duration)
    {
        var startTime = Time.realtimeSinceStartup;
        while (true)
        {
            float emission = Mathf.PingPong(Time.time, 1);
            var color = _mat.color * Mathf.LinearToGammaSpace(emission);
            _mat.SetColor("_EmissionColor", color);
            if (Time.realtimeSinceStartup - startTime > duration) break;
            yield return null;
        }
    }

    IEnumerator ColorChanging(Color color, float speed)
    {
        while (true) {
            yield return null;
        }
    }

    IEnumerator Rotating(float speed, float time){
        while (true) {

            yield return null;
        }
    }
    #endregion
}
