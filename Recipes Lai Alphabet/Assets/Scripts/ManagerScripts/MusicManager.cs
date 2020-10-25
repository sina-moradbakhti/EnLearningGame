using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManager;
    public AudioClip music;

    private AudioSource audioSource;

    void Start()
    {
        SetInitialReferences();
        
        if(!musicManager){
            musicManager = this;
        }else{
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        PlayMusic();
    }

    public void PlayMusic()
    {
        if(!music) return;

        audioSource.clip = music;
        audioSource.Play();
        audioSource.loop = true;
    }

    private bool changeVolume = false;
    private float newVolume;
    private float changeDelay;

    public void ChangeMusicVolume(float volume , float changeDelay)
    {

        newVolume = volume;
        this.changeDelay = changeDelay;
        changeVolume = true;
    }

    private void Update()
    {
        if (changeVolume)
        {
            if(newVolume > audioSource.volume)
            {
                audioSource.volume += (changeDelay * Time.deltaTime);
                if (audioSource.volume >= newVolume)
                    changeVolume = false;
            }
            else if(newVolume < audioSource.volume)
            {
                audioSource.volume -= (changeDelay * Time.deltaTime);
                if (audioSource.volume <= newVolume)
                    changeVolume = false;
            }
        }
    }

    private void SetInitialReferences()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

}
