using UnityEngine;
using System.Collections;
using System;

public class _brain_Object : MonoBehaviour {

    IEnumerator rotation;
    IEnumerator movement;

    float Y_axis;

    Material _mat;
	// Use this for initialization
	void Start ()
    {
        _mat = gameObject.GetComponentInChildren<Renderer>().material;
        Y_axis = gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #region Public API
    #region Moving

    public void MoveTo(Vector3 where, float duration)
    {
        if (movement != null) StopCoroutine(movement);
        movement = Moving(where, duration);
        StartCoroutine(movement);
    }
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

    public void Rotate(Vector3 angle, float speed)
    {
        rotation = Rotating(angle, speed);
        StartCoroutine(rotation);
    }

    public void StopRotating()
    {
        StopCoroutine(rotation);
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

    IEnumerator Rotating(Vector3 angle, float speed)
    {
        while (true) {
            gameObject.transform.Rotate(angle * Time.deltaTime * speed);
            yield return null;
        }
    }

    IEnumerator Moving(Vector3 where, float duration)
    {
        var startPos = gameObject.transform.position;
        var elapsedTime = 0f;
        var startTime = Time.realtimeSinceStartup;
        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            // TODO - needs to finish actually
            gameObject.transform.position = Vector3.Lerp(startPos, where, elapsedTime/duration);
            yield return null;
        }
    }

    #endregion
}
