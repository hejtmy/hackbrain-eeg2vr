using UnityEngine;
using System.Collections;

public class bvr_Particles : MonoBehaviour {

    public bvr_SceneSettings settings;
    private ParticleSystem particles;

    IEnumerator colorSwitching;
    // Use this for initialization
    void Start() {
        particles = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

    }
    #region Pulbi API
    public void ChangeColor(Color color1, Color color2, float duration)
    {
        var prevStartColor = particles.startColor[0];
        var prevEndColor = particles.startColor[1];
        if (colorSwitching != null) StopCoroutine(colorSwitching);
        //colorSwitching = SwitchingColours(prevStartColor, prevEndColor, duration);
        //StartCoroutine(colorSwitching);
    }
    #endregion
    IEnumerator SwitchingColours(Color color1, Color color2, float duration)
    {
        var startTime = Time.realtimeSinceStartup;
        var elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            var lerpedColor = Color.Lerp(color1, color2, elapsedTime / duration);
            //particles.startColor[1] = lerpedColor;
            yield return null;
        }
    }
}