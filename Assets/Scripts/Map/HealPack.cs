using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    public int healAmount;
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
        if(!onCooldown)
        {
            onCooldown = true; //Set Cooldown
            packMesh.SetActive(false); //Deactivate mesh
            audioSrc.Play();
            Hitpoints hpScript = other.GetComponent<Hitpoints>();
            hpScript.ApplyHeal(healAmount, this.gameObject);//Apply heal
            Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }

    }

    private void ReActivate()
    {
        onCooldown = false;
        packMesh.SetActive(true);
    }
}
