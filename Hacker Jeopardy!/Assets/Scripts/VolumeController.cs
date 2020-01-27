using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider1;
    public Slider volumeSlider2;
    public GameObject optionsPanel1;
    public GameObject optionsPanel2;
    public GameObject soloVideoObject;
    public GameObject dualVideoObject1;
    public GameObject dualVideoObject2;
    public GameObject intro;
    public GameObject cam1;

    public Image muteButton1;
    public Image muteButton2;
    public Sprite volumeOn;
    public Sprite volumeOff;

    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        volumeSlider1.value = AudioListener.volume;
        volumeSlider2.value = AudioListener.volume;

        if(PlayerPrefs.GetInt("mute") == 1)
        {
            muteButton1.sprite = volumeOff;
            muteButton1.color = Color.red;
            muteButton2.sprite = volumeOff;
            muteButton2.color = Color.red;

            AudioListener.volume = 0;
        }
        else
        {
            muteButton1.sprite = volumeOn;
            muteButton1.color = Color.green;
            muteButton2.sprite = volumeOn;
            muteButton2.color = Color.green;
        }

        SetVideoVolume();

    }

    public void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!optionsPanel1.activeSelf && !optionsPanel2.activeSelf)
            {
                if (cam1.activeSelf)
                    optionsPanel1.SetActive(true);
                else
                    optionsPanel2.SetActive(true);
            }
            else
            {
                optionsPanel1.SetActive(false);
                optionsPanel2.SetActive(false);
            }
        }
    }

    public void setVolume1()
    {
        AudioListener.volume = volumeSlider1.value;
        PlayerPrefs.SetFloat("volume", volumeSlider1.value);
        volumeSlider2.value = AudioListener.volume;
        SetVideoVolume();
    }

    public void setVolume2()
    {
        AudioListener.volume = volumeSlider2.value;
        PlayerPrefs.SetFloat("volume", volumeSlider2.value);
        volumeSlider1.value = AudioListener.volume;
        SetVideoVolume();
    }


    public void Exit()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        if(cam1.activeSelf)
            optionsPanel1.SetActive(false);
        else
            optionsPanel2.SetActive(false);
    }

    private void SetVideoVolume()
    {
        soloVideoObject.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, AudioListener.volume);
        dualVideoObject1.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, AudioListener.volume);
        dualVideoObject2.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, AudioListener.volume);
        intro.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, AudioListener.volume);
    }

    public void Mute()
    {
        if(muteButton1.sprite == volumeOn)
        {
            muteButton1.sprite = volumeOff;
            muteButton1.color = Color.red;
            muteButton2.sprite = volumeOff;
            muteButton2.color = Color.red;

            AudioListener.volume = 0;
            SetVideoVolume();
            PlayerPrefs.SetInt("mute", 1);
        }
        else
        {
            muteButton1.sprite = volumeOn;
            muteButton1.color = Color.green;
            muteButton2.sprite = volumeOn;
            muteButton2.color = Color.green;

            AudioListener.volume = volumeSlider1.value;
            SetVideoVolume();
            PlayerPrefs.SetInt("mute", 0);
        }

        
    }

}
