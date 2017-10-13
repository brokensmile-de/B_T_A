using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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
    [HideInInspector]
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
        string seconds = "";
        string minutes = "";

        if (actualTime % 60 < 10)
            seconds = "0" + actualTime % 60;
        else
            seconds = actualTime % 60 + "";

        if (actualTime / 60 < 10 && actualTime / 60  > 0)
            minutes = "0" + actualTime / 60;
        else if(actualTime / 60 <= 0)
            minutes = "00";
        else
            minutes = actualTime / 60 + "";

        timerText.text = minutes + " : " + seconds;
        if (actualTime <= 0)
        {
            isGameOver = true;
            scoreBoard.SetActive(true);
            ScoreboardManager.s_Singleton.ammoHud.SetActive(false);
            ScoreboardManager.s_Singleton.GenerateScoreboard();

            HudObject.SetActive(false);
            TimerObject.SetActive(false);
            Invoke("Disconnect", 5);
        }
    }

    public void Disconnect()
    {
        if(this.isServer)
            Prototype.NetworkLobby.LobbyManager.s_Singleton.SendReturnToLobby();
    }
}
