using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
    public int roundTime;
    private int actualTime;
    public Text timerText;
    public GameObject HudObject;
    public GameObject TimerObject;
    public GameObject scoreBoard;
    public static Timer singleton;
    public bool isGameOver = false;

    private void Awake()
    {
        singleton = this;

    }
    // Use this for initialization
    void Start()
    {


        if (!isServer)
            return;
        actualTime = roundTime*60;
        StartCoroutine(TimerRoutine());

    }

    private IEnumerator TimerRoutine()
    {
        while (actualTime > 0)
        {
            actualTime--;
            RpcUpdateTimer(actualTime);
            yield return new WaitForSeconds(1);
        }
    }

    [ClientRpc]
    void RpcUpdateTimer(int actualTime)
    {
        timerText.text = actualTime / 60 + " : " + actualTime % 60;
        if (actualTime <= 0)
        {
            isGameOver = true;
            scoreBoard.SetActive(true);
            ScoreboardManager.s_Singleton.GenerateScoreboard();

            HudObject.SetActive(false);
            TimerObject.SetActive(false);
            Invoke("Disconnect", 5);
        }


    }

    public void Disconnect()
    {
        Prototype.NetworkLobby.LobbyManager.s_Singleton.GoBackButton();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
