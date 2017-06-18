
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VampirePowerUp : PowerUps
{
    
    public override void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            InitilizePowerUp();
            PowerUpCountDown countdown = other.GetComponent<PowerUpCountDown>();
            Hitpoints hpScript = other.GetComponent<Hitpoints>();                    
            countdown.RestartTimer();
            if (countdown.hasPowerUp)
            {
                hpScript.VampirePowerUp();//Activate VampirePowerUp Coroutine
            }
            
            Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }

    }

   
}



//public GameObject packMesh;
//public float cooldown;

//private bool onCooldown;
//private AudioSource audioSrc;

//public void Start()
//{
//    audioSrc = GetComponent<AudioSource>();
//}

//private void ReActivate()
//{
//    onCooldown = false;
//    packMesh.SetActive(true);
//}