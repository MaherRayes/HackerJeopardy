using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuPanelController : MonoBehaviour
{
    public List<int> gameIDs;
    public int currentGame;
    public Slider volumeSlider;
    public Image muteButton;
    public Sprite volumeOn;
    public Sprite volumeOff;


    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        volumeSlider.value = AudioListener.volume;

        if (PlayerPrefs.GetInt("mute") == 1)
        {
            muteButton.sprite = volumeOff;
            muteButton.color = Color.red;

            AudioListener.volume = 0;
        }
        else
        {
            muteButton.sprite = volumeOn;
            muteButton.color = Color.green;
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();

   }

    public void On_Panel(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void Off_Panel(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void setVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    public void Mute()
    {
        if (muteButton.sprite == volumeOn)
        {
            muteButton.sprite = volumeOff;
            muteButton.color = Color.red;

            AudioListener.volume = 0;
            PlayerPrefs.SetInt("mute", 1);
        }
        else
        {
            muteButton.sprite = volumeOn;
            muteButton.color = Color.green;

            AudioListener.volume = volumeSlider.value;
            PlayerPrefs.SetInt("mute", 0);
        }
    }

    
}
