using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour {

    public static PersistenceManager instance;

    public float musicVolume = 1;
    public float effectVolume = 1;
    public AudioSource music;
    public ParticleSystem particles;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if(this == instance)
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
            effectVolume = PlayerPrefs.GetFloat("effectVolume", 1);
        }

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("effectVolume", effectVolume);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0 && scene.buildIndex != 1)
        {
            music.Stop();
            particles.Stop();
        }else if(!music.isPlaying)
        {
            music.Play();
            particles.Play();
        }
    }
}
