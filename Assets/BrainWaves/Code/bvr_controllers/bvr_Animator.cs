using UnityEngine;
using System.Collections;

public class bvr_Animator : MonoBehaviour {

    Animator anim;

    IEnumerator Speeding;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame

    public void PlayAnimation()
    {
        anim.SetBool("doDope", true);
    }

    public void SpeedUp(float targetSpeed, float duration)
    {
        if (Speeding != null) StopCoroutine(Speeding);
        Speeding = InterpolatingSpeed(targetSpeed, duration);
        StartCoroutine(Speeding);
    }

    public void SlowDown(float duration)
    {
        if (Speeding != null) StopCoroutine(Speeding);
        Speeding = InterpolatingSpeed(1, duration);
        StartCoroutine(Speeding);
    }

    protected IEnumerator InterpolatingSpeed(float targetSpeed, float duration)
    {
        var startSpeed = anim.speed;
        var elapsedTime = 0f;
        var startTime = Time.realtimeSinceStartup;
        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            // TODO - needs to finish actually

            anim.speed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / duration);
            yield return null;
        }
    }
}
