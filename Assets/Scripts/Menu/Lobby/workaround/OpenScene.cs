using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour {

    public float delay;
    private int id;
	public void ChangeScene(int id)
    {
        this.id = id;
        if (delay > 0)
            Invoke("Open", delay);
        else
            SceneManager.LoadScene(id);
    }

    private void Open()
    {
        SceneManager.LoadScene(id);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
