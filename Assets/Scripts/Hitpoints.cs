﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Adrian Zimmer
//Description: Hitpoints + Shield script. Zur benutzung: auf dem Script Applydamage(amount,schadensQuelle als gameobjekt) aufrufen
//Date Created: 03.04.2017
//Last edited:
//Edited by:

public class Hitpoints : MonoBehaviour {

    [Header("Parameter")]
    public int maxHitpoints;
    public int maxShield;
    public float timeTilShieldRestore;  //Zeit die man keinen damage bekommen darf bis das Schild sich wieder auflädt
    public AudioClip hitSound;

    [Header("Refferenzen")]
    public Text hitpointsText;          //Hud Text Hp
    public Text shieldText;             //Hud Text Shield

    private int hitpoints;
    private int shield;
    private AudioSource audioSource;
    private float lastHitTimestamp;      //Zeitpunkt zu dem man das letzte mal schaden bekommen + cooldown
    private bool restoringShield;        //gibt an ob das shield gerade am aufladen ist

	void Start ()
    {
        hitpoints = maxHitpoints;
        shield = maxShield;
        audioSource = GetComponent<AudioSource>();
	}

    public void Update()
    {
        //Wenn man länger nicht gehittet wurde und das schild nicht schon voll oder bereits am aufladen ist
        if(lastHitTimestamp < Time.time && shield < maxShield && !restoringShield)
        {
            StartCoroutine(RestoreShield());
        }
    }

    public event EventHandler<HitpointsEventArgs> HitEvent;
    protected virtual void OnHitEvent(int amount, GameObject damageSource)
    {
        lastHitTimestamp = Time.time + timeTilShieldRestore;

        //Schild+damage abzugsberechnungen
        int differenz = shield - amount;
        if (differenz < 0)
        {
            hitpoints += differenz;

            if (hitpoints < 0)
                hitpoints = 0;

            shield = 0;
        }else
        {
            shield -= amount;
        }

        //GUI Text setzen
        if (hitpointsText)
            hitpointsText.text = ""+hitpoints;
        if(shieldText)
            shieldText.text = "" + shield;

        //Hit sound abspielen
        if (audioSource != null)
            audioSource.PlayOneShot(hitSound);

        //HitEvent message senden
        if (HitEvent != null)
            HitEvent(this, new HitpointsEventArgs(hitpoints,shield, damageSource));
    }

    public event EventHandler<HitpointsEventArgs> HealEvent;
    protected virtual void OnHealEvent(int amount, GameObject healSource)
    {
        if (hitpoints + amount <= maxHitpoints)
        {
            hitpoints += amount;
        }
        else
        {
            hitpoints = maxHitpoints;
        }

        //GUI Text setzen
        if(hitpointsText)
            hitpointsText.text = "" + hitpoints;

        if (HealEvent != null)
            HealEvent(this, new HitpointsEventArgs(hitpoints, shield, healSource));
    }

    public void ApplyDamage(int amount,GameObject damageSource)
    {
        OnHitEvent(amount, damageSource);
    }
	
    public void ApplyHeal(int amount, GameObject healSource)
    {
        OnHealEvent(amount, healSource);
    }

    public void DamageTest()
    {
        ApplyDamage(5, null);
    }

    public void HealTest()
    {
        ApplyHeal(5, this.gameObject);
    }
    //Shield restore coroutine
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

    //Parameter werden bei hit event übergeben
    public class HitpointsEventArgs : EventArgs
    {
        public GameObject source;
        public int currentHitpoints,currentShield;

        public HitpointsEventArgs(int currentHitpoints,int currentShield, GameObject damageSource)
        {
            this.source = damageSource;
            this.currentHitpoints = currentHitpoints;
        }
    }
}
