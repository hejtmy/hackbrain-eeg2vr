using UnityEngine;
using System.Collections;

public class brv_IntroManager : MonoBehaviour {

    public GameObject StartExperience;
    public GameObject Connect;

	// Use this for initialization
	void Start () {
	
	}
	
    public void ConnectEEG()
    {
        //do sth
        Connect.SetActive(false);
        StartExperience.SetActive(true);
    }

    public void StartLevel()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
