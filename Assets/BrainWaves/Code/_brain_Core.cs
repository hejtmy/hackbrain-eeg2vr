using UnityEngine;
using System.Collections;

public class _brain_Core : MonoBehaviour {

    IEnumerator rotation;
    IEnumerator movement;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Public API
    public void Rotate(Vector3 angle, float speed)
    {
        rotation = Rotating(angle, speed);
        StartCoroutine(rotation);
    }

    public void StopRotating()
    {
        StopCoroutine(rotation);
    }

    public void MoveTo(Vector3 where, float duration)
    {
        if (movement != null) StopCoroutine(movement);
        movement = Moving(where, duration);
        StartCoroutine(movement);
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

    protected IEnumerator Resizing(float targetScale, float duration)
    {
        yield return null;
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
