using UnityEngine;
using System.Collections;

public class bvr_Core : MonoBehaviour {

    IEnumerator rotater;
    IEnumerator mover;

    public float Speed;

    #region Public API
    public void Rotate(Vector3 angle, float speed)
    {
        StopRotating();
        rotater = Rotating(angle, speed);
        StartCoroutine(rotater);
    }

    public void StopRotating()
    {
        if(rotater != null) StopCoroutine(rotater);
    }

    public void MoveTo(Vector3 where, float duration)
    {
        if (mover != null) StopCoroutine(mover);
        mover = Moving(where, duration);
        StartCoroutine(mover);
    }
    #endregion

    #region numerators
    protected IEnumerator Rotating(Vector3 angle, float speed)
    {
        while (true)
        {
            gameObject.transform.Rotate(angle * Time.deltaTime * speed, Space.World);
            yield return null;
        }
    }

    protected IEnumerator Moving(Vector3 where, float duration)
    {
        var startPos = gameObject.transform.position;
        var elapsedTime = 0f;
        var startTime = Time.realtimeSinceStartup;
        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            // TODO - needs to finish actually
             
            gameObject.transform.position = Vector3.Lerp(startPos, where, elapsedTime / duration);
            yield return null;
        }
    }
    #endregion
}
