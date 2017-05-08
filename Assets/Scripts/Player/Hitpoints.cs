using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Author: Adrian Zimmer
//Description: Hitpoints + Shield script. Zur benutzung: auf dem Script Applydamage(amount,schadensQuelle als gameobjekt) aufrufen
//Date Created: 03.04.2017
//Last edited:
//Edited by:

public class Hitpoints : NetworkBehaviour
{
    [Header("Parameter")]
    public const int maxHitpoints = 100;
    public const int maxShield = 100;
    public float timeTilShieldRestore;  //Zeit die man keinen damage bekommen darf bis das Schild sich wieder auflädt
    public AudioClip hitSound;
    [Header("Refferenzen")]
    public Text hitpointsText;          //Hud Text Hp
    public Text shieldText;             //Hud Text Shield

    private NetworkStartPosition[] spawnPoints;
    [SyncVar(hook = "OnChangeHealth")]
    public int hitpoints = maxHitpoints;
    [SyncVar(hook = "OnChangeShield")]
    public int shield = maxShield;
    private AudioSource audioSource;
    private float lastHitTimestamp;      //Zeitpunkt zu dem man das letzte mal schaden bekommen + cooldown
    private bool restoringShield;        //gibt an ob das shield gerade am aufladen ist

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
    public void TakeDamage(int amount)
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

            // called on the Server, invoked on the Clients
            RpcRespawn();
        }
    }


    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
            }

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

}
