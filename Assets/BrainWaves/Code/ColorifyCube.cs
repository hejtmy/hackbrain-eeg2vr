using UnityEngine;
using System.Collections;

public class ColorifyCube : MonoBehaviour {

    private Material _mat;
    private Color _originalColour;
    private Color _color;
    public BoxSettings Settings;

	// Use this for initialization
	void Start () {
        _mat = gameObject.GetComponent<Renderer>().material;
        _originalColour = _mat.color;
	}
	
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Brain") return;
        BrainWave brainWave = col.gameObject.GetComponent<BrainWave>();
        ChangeColour(brainWave.Color);
    }

	// Update is called once per frame
	void Update () {
        float emission = Mathf.PingPong(Time.time, 1.0f);
        _color = _mat.color * Mathf.LinearToGammaSpace(emission);
        _mat.SetColor("_EmissionColor", _color);
    }

    #region Public API
    public void ChangeColour(Color color)
    {
        _mat.color = color;
        StartCoroutine(ChangeColourBack(1));
    }

    public void Pulse()
    {
        
    }
    #endregion

    IEnumerator ChangeColourBack(float time)
    {
        yield return new WaitForSeconds(time);
        _mat.color = _originalColour;
    }
}
