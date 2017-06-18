using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitedStamina : PowerUps
{
    
    public override void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            InitilizePowerUp();
            
            PlayerMovement playerMov = other.GetComponent<PlayerMovement>();
            PowerUpCountDown countdown = other.GetComponent<PowerUpCountDown>();
            countdown.RestartTimer();
            if (countdown.hasPowerUp)
            {
                playerMov.IncreaseDashPowerUp();
            }
            
            Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }

    }
    
}