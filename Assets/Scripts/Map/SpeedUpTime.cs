using UnityEngine;
using System.Collections;

public class SpeedUpTime : MonoBehaviour
{
    public float SpeedUpFactor = 2.0f; // Ratio ralentizado / normal
    public float duration = 30.0f; // Duración de la ralentización
    public float deactivationPeriodDuration = 1.0f; // Duración del periodo en el que se reestablece la velocidad normal.

    private float deactivationElapsedTime; // Tiempo que ha transcurrido durante el reestablecimiento de la velocidad normal.
    private float endEffect; // Tiempo en el que acaba el efecto de ralentización

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            endEffect = Time.time + duration;
            StartCoroutine(SpeedUp());
        }
    }

    IEnumerator SpeedUp()
    {
        Time.timeScale = SpeedUpFactor;
        while (Time.time < endEffect)
        {
            yield return null;
        }

        deactivationElapsedTime = 0;

        while (deactivationElapsedTime < deactivationPeriodDuration)
        {
            Time.timeScale = Mathf.Lerp(SpeedUpFactor, 1, (deactivationElapsedTime / deactivationPeriodDuration));
            deactivationElapsedTime += Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 1;
    }
}