using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {
    public GameObject Mesh;
    private bool onCooldown;
    public float cooldown;
    private AudioSource audioSrc;


    public virtual void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            onCooldown = true; //Set Cooldown
            Mesh.gameObject.SetActive(false); //Deactivate mesh
            audioSrc.volume = PersistenceManager.instance.effectVolume;
            audioSrc.Play();
            Use(other);
            Invoke("ReActivate", cooldown); //Re-enable after Cooldown
        }
    }

    protected abstract void Use(Collider other);

    private void ReActivate()
    {
        onCooldown = false;
        Mesh.gameObject.SetActive(true);
    }
}
