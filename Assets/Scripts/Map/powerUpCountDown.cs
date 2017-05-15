using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCountDown : MonoBehaviour {

    public Text CountdownTimer;
    public float powerUptimeLeft = 15;
    public bool hasPowerUp;
    public bool timerAlreadyStarted;

    private IEnumerator CountDown(float timer)
    {
        do
        {
            timerAlreadyStarted = true;
            CountdownTimer.enabled = true;
            double currTime = timer;
            hasPowerUp = true;
            timer -= Time.deltaTime;

            if (CountdownTimer)
            {
                currTime = System.Math.Round(currTime, 1);
                CountdownTimer.text = "" + currTime;
            }
            yield return new WaitForSeconds(0.0005f);
        } while (timer > 0);
        timerAlreadyStarted = false;
        timer = powerUptimeLeft;
        hasPowerUp = false;
    }

    public void CountDownTimer(float timer)
    {
        StopAllCoroutines();
        StartCoroutine(CountDown(15));
        
    }

    
    
    
}
