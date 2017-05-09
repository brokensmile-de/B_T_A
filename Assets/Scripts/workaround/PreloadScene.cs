using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(1,LoadSceneMode.Additive);
	}
	public void changeScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        SceneManager.I
    }
	// Update is called once per frame
	void Update () {
		
	}
}
