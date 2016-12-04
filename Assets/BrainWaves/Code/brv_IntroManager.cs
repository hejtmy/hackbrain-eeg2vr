using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class brv_IntroManager : MonoBehaviour {

    public GameObject StartExperience;
    public GameObject Connect;

	// Use this for initialization
	
    public void ConnectEEG()
    {
        //do sth
        Connect.SetActive(false);
        StartExperience.SetActive(true);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("FogThing");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
