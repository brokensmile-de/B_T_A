using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Author: Adrian Zimmer
//Description: Hitpoints + Shield script. Zur benutzung: auf dem Script Applydamage(amount,schadensQuelle als gameobjekt) aufrufen
//Date Created: 03.04.2017
//Last edited:16.06.2017
//Edited by:Valentin Kircher, alle powerups fangen durch die bestätigung vom CountDown an und leiten von PowerUps ab

public class Hitpoints : NetworkBehaviour
{
    [Header("Parameter")]
    public const int maxHitpoints = 100;
    public const int maxShield = 100;
    public const int doubleMaxShield = 200;
    public bool hasVampire;
    public bool hasDoubleDamage;
    public float timeTilShieldRestore;  //Zeit die man keinen damage bekommen darf bis das Schild sich wieder auflädt
    public AudioClip hitSound;
    public bool alwaysTrue = true;

    [Header("Refferenzen")]
    public Text hitpointsText;          //Hud Text Hp
    public Text shieldText;             //Hud Text Shield
    //public Text CountdownTimerText;


    private NetworkStartPosition[] spawnPoints;
    [SyncVar(hook = "OnChangeHealth")]
    public int hitpoints = maxHitpoints;
    [SyncVar(hook = "OnChangeShield")]
    public int shield = maxShield;

    private AudioSource audioSource;
    private float lastHitTimestamp;      //Zeitpunkt zu dem man das letzte mal schaden bekommen + cooldown
    private bool restoringShield;        //gibt an ob das shield gerade am aufladen ist



    [SyncVar]
    public int deaths = 0;
    [SyncVar]
    public int kills = 0;
    [SyncVar]
    public int score = 0;

    [SyncVar]
    public string playerName;

    void Start()
    {

        hitpoints = maxHitpoints;
        shield = maxShield;
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            GameObject hudCanvas = GameObject.Find("HudCanvas");
            hitpointsText = hudCanvas.transform.Find("HealthUI/Hitpoints").GetComponent<Text>();
            shieldText = hudCanvas.transform.Find("HealthUI/Shield").GetComponent<Text>();
           // PowerUpCountDown countdownTimer = GetComponent<PowerUpCountDown>();

           // CountdownTimerText = hudCanvas.transform.Find("Countdown/bg/CountdownTimer").GetComponent<Text>();
        }
        hitpoints = maxHitpoints;
        shield = maxShield;
    }

    public void Update()
    {
        if (!isServer)
            return;

        if (lastHitTimestamp < Time.time && shield < maxShield && !restoringShield)
        {
            StartCoroutine(RestoreShield());
        }
    }

    public void Heal(int amount)
    {
        if (!isServer)
            return;

        if (hitpoints + amount <= maxHitpoints)
        {
            hitpoints += amount;
        }
        else
        {
            hitpoints = maxHitpoints;
        }

    }
    public void TakeDamage(int amount, GameObject inflicter)
    {
        if (!isServer)
            return;

        lastHitTimestamp = Time.time + timeTilShieldRestore;

        //Schild+damage abzugsberechnungen
        int differenz = shield - amount;
        if (differenz < 0)
        {
            hitpoints += differenz;

            if (hitpoints < 0)
                hitpoints = 0;

            shield = 0;
        }

        else
        {
            shield -= amount;
        }


        if (audioSource != null)
            audioSource.PlayOneShot(hitSound);

        if (hitpoints <= 0)
        {

            hitpoints = maxHitpoints;
            shield = maxShield;
            deaths++;



            // called on the Server, invoked on the Clients
            RpcRespawn();

        }
    }

    [Server]
    private void AddScore()
    {
        kills++;
        score += 10;
    }


    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            ScoreboardManager.s_Singleton.GenerateScoreboard();
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
            }
            GetComponent<Combat.GunController>().PickGun(0);

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;

        }
    }


    private void OnChangeHealth(int currentHealth)
    {
        //GUI Text setzen
        if (hitpointsText)
            hitpointsText.text = "" + currentHealth;

    }

    private void OnChangeShield(int currentShield)
    {
        //GUI Text setzen
        if (shieldText)
            shieldText.text = "" + currentShield;
    }
    //
    public void DoubleShieldPowerUp()
    {

        PowerUpCountDown timer = GetComponent<PowerUpCountDown>();

        restoringShield = true;


        while (shield < doubleMaxShield && timer.hasPowerUp)
        {

            //shield += 2;
            HealShield(2);

            //shield = doubleMaxShield;
            //if (shield > doubleMaxShield)
            //    shield = doubleMaxShield;
            OnChangeShield(shield);

        }

        restoringShield = false;
    }

    public void VampirePowerUp()
    {
        //PowerUpCountDown timer = GetComponent<PowerUpCountDown>();
        //VampirePowerUp vampirePowerup = GetComponent<VampirePowerUp>();
        //while(shield<=maxShield&&timer.hasPowerUp)
        //{
        //    hasVampire = true;
        //    OnChangeHealth(hitpoints);
        //}
        //hasVampire = false;
        StartCoroutine(Vampir());
    }
    //valentin, Ienumerator damit es bei der while schleife es nicht abstürzt
    private IEnumerator Vampir()
    {
        PowerUpCountDown timer = GetComponent<PowerUpCountDown>();
        //VampirePowerUp vampirePowerup = GetComponent<VampirePowerUp>();
        while (timer.hasPowerUp)
        {
            hasVampire = true;
            //OnChangeHealth(hitpoints);
            yield return new WaitForSeconds(0.5f);
        }
        hasVampire = false;
    }

    public void DoubleDamagePowerUp()
    {
        //PowerUpCountDown timer = GetComponent<PowerUpCountDown>();
        //while (timer.hasPowerUp)
        //{
        //    hasDoubleDamage = true;

        //}
        //hasDoubleDamage = false;
        StartCoroutine(DoubleDamage());
    }

    public IEnumerator DoubleDamage()
    {
        PowerUpCountDown timer = GetComponent<PowerUpCountDown>();
        while (timer.hasPowerUp)
        {
            hasDoubleDamage = true;
            yield return new WaitForSeconds(0.5f);
        }
        hasDoubleDamage = false;

    }

    private IEnumerator RestoreShield()
    {
        restoringShield = true;
        while (shield < maxShield && lastHitTimestamp < Time.time)
        {
            shield += 2;

            if (shield > maxShield)
                shield = maxShield;
            if (shieldText)
                shieldText.text = "" + shield;
            yield return new WaitForSeconds(0.05f);
        }
        restoringShield = false;
    }
    public void HealShield(int amount)
    {
        if (doubleMaxShield + amount <= doubleMaxShield){
            shield += amount;
        }
        else
        {
            shield = doubleMaxShield;
        }
        
    }
}
