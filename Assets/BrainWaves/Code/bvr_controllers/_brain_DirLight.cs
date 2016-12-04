using UnityEngine;
using System.Collections;

public class _brain_DirLight : _brain_Core {

    Light thisLight;
    IEnumerator colorSwitching;


	// Use this for initialization
	void Start () {
        thisLight = gameObject.GetComponent<Light>();   
	}

    #region Public API
    public void ChangeColor(Color color, float duration)
    {
        var prevColor = thisLight.color;
        if (colorSwitching != null) StopCoroutine(colorSwitching);
        colorSwitching = SwitchingColours(prevColor, color, duration);
        StartCoroutine(colorSwitching);
    }
    
    #endregion
    #region Enumerators
    IEnumerator SwitchingColours(Color color1, Color color2, float duration)
    {
        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            var lerpedColor = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1));
            thisLight.color = lerpedColor;
            yield return null;
        }
    }

    #endregion
}
