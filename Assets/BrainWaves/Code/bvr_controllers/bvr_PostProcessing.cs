using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;

public class bvr_PostProcessing : MonoBehaviour {

    public PostProcessingProfile PostProcessing;

    IEnumerator vignetter;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LerpVignette(float target, float duration)
    {
        if (vignetter != null) StopCoroutine(vignetter);
        var startvignette = PostProcessing.vignette.settings.intensity;
        vignetter = LerpingVignette(startvignette, target, duration);
        StartCoroutine(vignetter);
    }
     
    IEnumerator LerpingVignette(float start, float target, float duration)
    {
        var elapsedTime = 0f;
        var startTime = Time.realtimeSinceStartup;
        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            // TODO - needs to finish actually
            var settings = PostProcessing.vignette.settings;
            settings.intensity = Mathf.Lerp(start, target, elapsedTime / duration);
            PostProcessing.vignette.settings = settings;
            yield return null;
        }
    }

}
