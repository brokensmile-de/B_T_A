using System;
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
    public int doubleMaxShield;
    public float timeTilShieldRestore;  //Zeit die man keinen damage bekommen darf bis das Schild sich wieder auflädt
    public AudioClip hitSound;
    public bool hasVampire;
    public bool hasDoubleDam;

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

    public void activateDoubleShield()
    {
        StartCoroutine(DoubleRestoreShield());
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

    // for debugging
    public void SignOfLife()
    {
        Debug.Log("I'm there!");
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

    public void VampirePowerUp()
    {
        StartCoroutine(VampireRecoverHp());
    }

    public void DoubleDamagePowerUp()
    {
        StartCoroutine(DoubleDamageEnum());
    }

    private IEnumerator DoubleRestoreShield()
    {
        PowerUpCountDown timer = GetComponent<PowerUpCountDown>();

        
        restoringShield = true;
        while (shield < doubleMaxShield && lastHitTimestamp < Time.time )
        {
            shield += 2;

            if (shield > doubleMaxShield)
                shield = doubleMaxShield;
            if (shieldText)
                shieldText.text = "" + shield;
            yield return new WaitForSeconds(0.05f);

            if (timer.hasPowerUp == false)
                break;
            
        }

        if (shield > maxShield)
            shield=maxShield;

        restoringShield = false;
       
    }

    private IEnumerator VampireRecoverHp()
    {
        //  Countdown
        PowerUpCountDown timer = GetComponent<PowerUpCountDown>();
        //bullet 
        //StealHP stealHP = GetComponent<StealHP>();
        //powerup pick up 
        VampirePowerUp powerup = GetComponent<VampirePowerUp>();
        
        //stealHP.setInstantiator(gameObject);
        
        while (timer.hasPowerUp==true)
        {
            hasVampire = true;
                          
            //if (hitpointsText)
            //    hitpointsText.text = "" + hitpoints;

            yield return new WaitForSeconds(0.05f);

        }
        hasVampire = false;
    }


    private IEnumerator DoubleDamageEnum()
    {
        PowerUpCountDown timer = GetComponent<PowerUpCountDown>();
        //StealHP stealHP = GetComponent<StealHP>();


        while (timer.hasPowerUp == true)
        {
            //if (stealHP.Damage == stealHP.doubleDamageAmount)
            //{
            //    stealHP.Damage = stealHP.doubleDamageAmount;
            //}
            hasDoubleDam = true;
            yield return new WaitForSeconds(0.05f);

        }
        hasDoubleDam = false;
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


    //edit 05.04.2017 for setting up references during runtime
    public void setHealthReference(GameObject t)
    {
        hitpointsText=t.GetComponent<Text>();         
    }

    public void setShieldReference(GameObject s)
    {
        shieldText=s.GetComponent<Text>();
    }
}
