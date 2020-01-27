using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoAudioController : MonoBehaviour
{
    public void PlayVideo(GameObject videoObj)
    {
        VideoPlayer vid = videoObj.GetComponent<VideoPlayer>();
        if (!vid.isPlaying)
            vid.Play();
    }

    public void PauseVideo(GameObject videoObj)
    {
        videoObj.GetComponent<VideoPlayer>().Pause();
    }

    public void RewindVideo(GameObject videoObj)
    {
        videoObj.GetComponent<VideoPlayer>().time = 0;
    }

    public void PlayAudio(GameObject audioObj)
    {
        AudioSource aud = audioObj.GetComponent<AudioSource>();

        for (int i = 1; i < audioObj.transform.childCount; i++)
        {
            audioObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (!aud.isPlaying)
            aud.Play();

    }

    public void PauseAudio(GameObject audioObj)
    {
        audioObj.GetComponent<AudioSource>().Pause();
        for(int i = 1; i<audioObj.transform.childCount; i++)
        {
            audioObj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void RewindAudio(GameObject audioObj)
    {
        audioObj.GetComponent<AudioSource>().time = 0;
    }
}
