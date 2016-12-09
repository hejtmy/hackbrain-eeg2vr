using UnityEngine;
using System.Collections;

public class bvr_DirLight : bvr_Core {

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
        var elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            var lerpedColor = Color.Lerp(color1, color2, elapsedTime / duration);
            thisLight.color = lerpedColor;
            yield return null;
        }
    }

    #endregion
}
