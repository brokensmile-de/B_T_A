using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitedStamina : MonoBehaviour
{

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
            PlayerMovement playerMov = other.GetComponent<PlayerMovement>();
            powerUpCountDown countdown = other.GetComponent<powerUpCountDown>();
            countdown.CountDownTimer(15);
            playerMov.IncreaseDashPowerUp();
            Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }

    }

    private void ReActivate()
    {
        onCooldown = false;
        packMesh.SetActive(true);
    }
}