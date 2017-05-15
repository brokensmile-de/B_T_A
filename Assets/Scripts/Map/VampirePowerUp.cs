﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirePowerUp : MonoBehaviour
{
    public int StealHpAmount;
    public GameObject packMesh;
    public float cooldown;

    private bool onCooldown;
    private AudioSource audioSrc;

    public void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            onCooldown = true; //Set Cooldown
            packMesh.SetActive(false); //Deactivate mesh
            audioSrc.Play();
            PowerUpCountDown countdown = other.GetComponent<PowerUpCountDown>();
            Hitpoints hpScript = other.GetComponent<Hitpoints>();                    
            countdown.CountDownTimer(30);
            hpScript.VampirePowerUp();//Activate VampirePowerUp Coroutine
            Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }

    }

    private void ReActivate()
    {
        onCooldown = false;
        packMesh.SetActive(true);
    }
}

