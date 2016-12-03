using UnityEngine;
using System.Collections;
using System;

public class _brain_Object : _brain_Core {

    float Y_axis;

    Material _mat;
	// Use this for initialization
	void Start ()
    {
        _mat = gameObject.GetComponentInChildren<Renderer>().material;
        Y_axis = gameObject.transform.position.y;
	}

    #region Public API
    #region Moving


    public void Disappear(float duration)
    {
        var curPos = gameObject.transform.position;
        var where = new Vector3(curPos.x, -1, curPos.z);
        MoveTo(where, duration);
    }

    public void Appear(float duration)
    {
        var curPos = gameObject.transform.position;
        var where = new Vector3(curPos.x, Y_axis, curPos.z);
        MoveTo(where, duration);
    }

    #endregion
    #region transformations
    public void Resize(float newScale = 1)
    {

    }
    #endregion
    #region Colour Changes

    #endregion
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

    public void ChangeColour(Color color, float speed = 1)
    {

    }

    #endregion

    #region IEnumerators
    IEnumerator SwitchingColours(Color color1, Color color2, float duration)
    {
        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            var lerpedColor = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1));
            _mat.color = lerpedColor;
            yield return null;
        }
    }

    IEnumerator PulsingEmission(float speed, float duration)
    {
        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            float emission = Mathf.PingPong(Time.time, 1);
            var color = _mat.color * Mathf.LinearToGammaSpace(emission);
            _mat.SetColor("_EmissionColor", color);
            yield return null;
        }
    }

    #endregion
}
