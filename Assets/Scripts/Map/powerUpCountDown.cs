using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerUpCountDown : MonoBehaviour {

    public Text CountdownTimer;
    public float powerUptimeLeft = 15;
    public bool hasPowerUp;

    private IEnumerator CountDown(float timer)
    {
        do
        {
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
        hasPowerUp = false;
        timer = powerUptimeLeft;
        CountdownTimer.enabled = false;
    }

    public void CountDownTimer(float timer)
    {
        StartCoroutine(CountDown(15));
    }
}
