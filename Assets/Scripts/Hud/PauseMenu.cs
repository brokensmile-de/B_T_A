using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenu;
    public AudioSource closeSound;
    public GameObject blackscreen;
	// Use this for initialization
	void Start () {
		
	}

    private bool showing;
    public void openPauseMenu()
    {
        pauseMenu.SetActive(true);

        showing = true;
    }

    public void closePauseMenu()
    {
        pauseMenu.SetActive(false);
        showing = false;
        closeSound.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("escape") && !showing)
        {
            
            openPauseMenu();
        }
        else if (showing && Input.GetKeyDown("escape"))
        {
                
            closePauseMenu();
        }

        }


    public void Disconnect()
    {
        blackscreen.SetActive(true);
        LobbyManager.s_Singleton.GoBackButton();
    }
}



