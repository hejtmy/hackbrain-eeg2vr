using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("delete"))
        {
            Destroy(bvr_Listener.Get.gameObject);
            SceneManager.LoadScene(0);
        }
	}
}
