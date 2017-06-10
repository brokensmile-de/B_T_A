using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour {
    static public ScoreboardManager s_Singleton;

    public GameObject scoreEntryPrefab;
    public GameObject scoreboard;       //Refferenz auf Scoreboard
    private GameObject playerlist;      //Refferenz auf scoreEntrys

    public GameObject[] players;


    private bool showing;
    // Use this for initialization

    void Start () {
        s_Singleton = this;
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

    public void GenerateScoreboard()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (Transform child in playerlist.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        foreach (GameObject o in players)
        {

            var entry = Instantiate(scoreEntryPrefab) as GameObject;
            entry.transform.SetParent(playerlist.transform);
            entry.transform.localScale = new Vector3(1, 1, 1);

            EntryManager man = entry.GetComponent<EntryManager>();

            PlayerMovement player = o.GetComponent<PlayerMovement>();
            Hitpoints playerHP = o.GetComponent<Hitpoints>();
            //man.playerName.text = e.Value.playerName;
            man.color.color = player.color;
            man.deaths.text = playerHP.deaths + "";
            man.kills.text = playerHP.kills + "";
            man.score.text = playerHP.score + "";
        }

        //Prototype.NetworkLobby.LobbyManager.s_Singleton.gameObject
    }
}

