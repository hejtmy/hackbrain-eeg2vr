using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class bvr_IntroManager : Singleton<bvr_EEGVisualiser> {

    public GameObject StartExperience;
    public GameObject Connect;

	// Use this for initialization
	
    public void ConnectEEG()
    {
        //do sth
        StartExperience.SetActive(true);
    }

    public void StartLevel()
    {
        DontDestroyOnLoad(bvr_Listener.Get.gameObject);
        SceneManager.LoadScene("FogThing");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
