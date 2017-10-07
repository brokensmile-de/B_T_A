using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{

    static public ScoreboardManager s_Singleton;

    public GameObject scoreEntryPrefab;
    public GameObject scoreboard;       //Refferenz auf Scoreboard
    public GameObject ammoHud;
    private GameObject playerlist;      //Refferenz auf scoreEntrys

    public GameObject[] players;


    private bool showing;
    // Use this for initialization

    void Start()
    {
        s_Singleton = this;
        playerlist = scoreboard.transform.Find("PlayerList").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("tab") && !showing)
        {
            scoreboard.SetActive(true);
            showing = true;
            GenerateScoreboard();
            StartCoroutine(updateScoreboard());
        }

        else if (showing && !Input.GetKey("tab"))
        {
            if (scoreboard.activeInHierarchy && !Timer.singleton.isGameOver)
            {

                scoreboard.SetActive(false);
                showing = false;
            }

        }
    }
    private IEnumerator updateScoreboard()
    {
        while (showing)
        {
            GenerateScoreboard();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GenerateScoreboard()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (Transform child in playerlist.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<NewEntry> playerList = new List<NewEntry>();

        foreach (GameObject o in players)
        {
            PlayerMovement player = o.GetComponent<PlayerMovement>();
            Hitpoints playerHP = o.GetComponent<Hitpoints>();
            playerList.Add(new NewEntry(playerHP.playerName,player.color, playerHP.deaths, playerHP.kills, playerHP.score));
        }

        playerList = playerList.OrderByDescending(o => o.score).ToList();

        foreach (NewEntry e in playerList)
        {
            var entry = Instantiate(scoreEntryPrefab) as GameObject;
            entry.transform.SetParent(playerlist.transform);
            entry.transform.localScale = new Vector3(1, 1, 1);

            EntryManager man = entry.GetComponent<EntryManager>();

            man.playerName.text = e.playerName;
            man.color.color = e.color;
            man.deaths.text = e.deaths + "";
            man.kills.text = e.kills + "";
            man.score.text = e.score + "";
        }


        //Prototype.NetworkLobby.LobbyManager.s_Singleton.gameObject
    }
}

class NewEntry
{
    public Color color;
    public string playerName;
    public int deaths;
    public int kills;
    public int score;

    public NewEntry(string playerName, Color color, int deaths, int kills, int score)
    {
        this.playerName = playerName;
        this.color = color;
        this.deaths = deaths;
        this.kills = kills;
        this.score = score;
    }
}

