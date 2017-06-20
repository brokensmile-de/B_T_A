using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PowerUpCountDown : NetworkBehaviour
{
    
    //public Text CountdownTimer;
    
    [SyncVar]
    public float powerUptimeLeft = 15;
    public bool hasPowerUp;
    public bool timerAlreadyStarted;
    public double currTime;
    //public double time = 5;
    //public float countdownSpeed=0.1f;

    private IEnumerator CountDown(float timer)
    {
        //Hitpoints cText = GetComponent<Hitpoints>();
        
        while (timer > 0)
        {
            //cText.CountdownTimerText.enabled = true;
            currTime = timer;
            //if (cText.CountdownTimerText)
           // {
                currTime = System.Math.Round(currTime, 1);
           //     cText.CountdownTimerText.text = "" + currTime;
           // }
            
           

            timerAlreadyStarted = true;          
            hasPowerUp = true;
            timer -= Time.deltaTime;
            
            
            yield return new WaitForSeconds(0.005f);
        }
        timerAlreadyStarted = false;
        timer = powerUptimeLeft;
        hasPowerUp = false;
        //cText.CountdownTimerText.enabled = false;
    }


    public void CountDownTimer()
    {
        RestartTimer();
        CountDown(30);
        

    }



    public void RestartTimer()
    {
        if (timerAlreadyStarted)
        {
            StopAllCoroutines();
            StartCoroutine(CountDown(15));
           // CountDown(30);
        }
        else
            StartCoroutine(CountDown(15));
        //CountDown(30);
    }
}

//public void DoCountdown()
//{
//    while (time > 0)
//    {


//        timerAlreadyStarted = true;
//        CountdownTimerText.enabled = true;
//        hasPowerUp = true;

//        time -= Time.deltaTime;
//        //double currTime = time;

//        if (CountdownTimerText)
//        {
//            time = System.Math.Round(time, 1);
//            CountdownTimerText.text = "Time:" + time*countdownSpeed;
//            //  Debug.Log("" + CountdownTimerText.text);
//        }

//    }
//    timerAlreadyStarted = false;
//    time = powerUptimeLeft;
//    hasPowerUp = false;
//    //CountdownTimerText.enabled = false;
//}

//private void Update()
//{

//}
