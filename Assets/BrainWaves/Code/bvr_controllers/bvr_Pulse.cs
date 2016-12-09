using UnityEngine;
using System.Collections;

public class bvr_Pulse : bvr_Core {  
    public Color Color;
    public float Speed = 1;
    public float Strength;

    public IEnumerator pulse;
    private Vector3 _direction = new Vector3(1, 0, 0);

    public void StartPulsing(float targetScale, float duration)
    {
        StopPulsing();
        pulse = Pulsing(targetScale, duration);
        StartCoroutine(pulse);
    }
    public void StopPulsing()
    {
        if (pulse != null) StopCoroutine(pulse);
    }

    protected IEnumerator Pulsing(float targetScale, float duration)
    {
        var currentScaleVec = gameObject.transform.localScale;
        var targetScaleVec = currentScaleVec * targetScale;
        while (true)
        {
            gameObject.transform.localScale = Vector3.Lerp(currentScaleVec, targetScaleVec, Mathf.PingPong(Time.time, 1));
            yield return null;
        }
    }

}
