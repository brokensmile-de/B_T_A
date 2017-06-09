using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour {

    public GameObject scoreboard;
    public GameObject scoreEntryPrefab;
    private GameObject playerlist;
    private bool showing;
    public Hashtable playerList = new Hashtable();

	// Use this for initialization
	void Start () {

        playerlist = scoreboard.transform.Find("PlayerList").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("tab") && !showing)
        {
            scoreboard.SetActive(true);
            showing = true;
            GenerateScoreboard();
        }

        else if (showing && !Input.GetKey("tab"))
        {
            if(scoreboard.activeInHierarchy)
            {
                scoreboard.SetActive(false);
                showing = false;
            }

        }
    }

    private void GenerateScoreboard()
    {
        foreach (Transform child in playerlist.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < 5; i++)
        {
            var entry = Instantiate(scoreEntryPrefab) as GameObject;
            entry.transform.parent = playerlist.transform;
            entry.transform.localScale = new Vector3(1, 1, 1);
            entry.GetComponent<EntryManager>().naeme.text = i+ "";
        }

        //Prototype.NetworkLobby.LobbyManager.s_Singleton.gameObject
    }
}

