﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Slider musicSlider;


    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach(AudioClip clip in clips)
        {
            audioClips.Add(clip.name,clip);
        }
        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); }) ;
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); }) ;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaySFX("Die2");
        }
    }

    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);
        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}