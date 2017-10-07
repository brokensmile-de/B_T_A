using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetter : MonoBehaviour {

    //Wird benötigt um beim laden von scenes die richtige lautstärke einzustellen
    public AudioSource close;
    public AudioSource menuLoad;
    public AudioSource highlight;
    public AudioSource backSound;

    public void Start()
    {
        close.volume = PersistenceManager.instance.effectVolume;
        menuLoad.volume = PersistenceManager.instance.effectVolume;
        highlight.volume = PersistenceManager.instance.effectVolume;
        backSound.volume = PersistenceManager.instance.effectVolume;
    }
}
