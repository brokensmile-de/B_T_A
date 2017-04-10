using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMasterVol : MonoBehaviour {

    public Slider masterVolumeSlider;
    public AudioSource myMusic;

	public void VolumeController() {
        masterVolumeSlider.value = myMusic.volume;
    }
}
