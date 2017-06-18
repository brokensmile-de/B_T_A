using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamageItem : PowerUps
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
                hpScript.DoubleDamagePowerUp();//Activate VampirePowerUp Coroutine

            }
        Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }

    }
    
}
