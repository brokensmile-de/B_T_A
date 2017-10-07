using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolumeSetter : MonoBehaviour {
    Slider sl;
    public AudioSource[] sourcesToSet;
    public SliderType type;
    public enum SliderType { MUSIC, FX};
	// Use this for initialization
	void Start ()
    {
        sl = GetComponent<Slider>();
        sl.onValueChanged.AddListener(delegate { ChangeVolume(); });
        if (type == SliderType.MUSIC)
        {
           sl.value = PersistenceManager.instance.musicVolume ;

        }
        else
        {
            sl.value = PersistenceManager.instance.effectVolume;
        }
    }

	void ChangeVolume()
    {
        foreach(AudioSource source in sourcesToSet)
        {

            if(type == SliderType.MUSIC)
            {
                PersistenceManager.instance.musicVolume = sl.value;
                PersistenceManager.instance.music.volume = sl.value;

            }
            else
            {
                source.volume = sl.value;
                PersistenceManager.instance.effectVolume = sl.value;
            }

        }
    }
}
