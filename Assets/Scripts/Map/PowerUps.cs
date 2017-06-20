using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    
    public GameObject packMesh;
    public float cooldown;
    public int StealHpAmount;
    public bool onCooldown;
    public AudioSource audioSrc;

    public void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            
            onCooldown = true; //Set Cooldown
            packMesh.SetActive(false); //Deactivate mesh
            audioSrc.Play();
            PowerUpCountDown countdown = other.GetComponent<PowerUpCountDown>();
          //  Hitpoints hpScript = other.GetComponent<Hitpoints>();
            countdown.RestartTimer();
            if (countdown.hasPowerUp)
            {
                DoPowerUp();
                //hpScript.DoubleShieldPowerUp();//Activate Double Shield
                ////hpScript.shield = hpScript.doubleMaxShield;
                //Debug.Log("In DoubleShieldTrigger hasPowerUp");
            }


             //Re-enable after Cooldown
        }
        
    }

    public virtual void DoPowerUp()
    {

        Debug.Log("powerup Parent");


    }

    public void InitilizePowerUp()
    {
        
        
            onCooldown = true; //Set Cooldown
            packMesh.SetActive(false); //Deactivate mesh
            audioSrc.Play();

        
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (!onCooldown)
    //    {

    //        PowerUpCountDown countdown = other.GetComponent<PowerUpCountDown>();
    //        HitpointsNew hpScript = other.GetComponent<HitpointsNew>();
    //        countdown.CountDownTimer(30);
    //        if (countdown.hasPowerUp)
    //            hpScript.DoubleShieldPowerUp();

    //        Invoke("ReActivate", cooldown); //Re-enable after Cooldown
    //    }

    //}





    public void ReActivate()
    {
        onCooldown = false;
        packMesh.SetActive(true);
    }
}
