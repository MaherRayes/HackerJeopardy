using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiScreenController : MonoBehaviour
{

    public Camera mod_cam;
    public Camera pub_cam;
    public Canvas mod_canvas;
    public Canvas pub_canvas;
    public Canvas playerCanvas;

    void Start()
    {
        Init_Displays();
    }

    public void Screen_Switcher()
    {

        if (mod_cam.gameObject.active)
        {
            mod_cam.gameObject.SetActive(false);
            mod_canvas.sortingOrder = 0;

            pub_cam.gameObject.SetActive(true);
            pub_canvas.sortingOrder = 1;
        }
        else
        {
            pub_cam.gameObject.SetActive(false);
            pub_canvas.sortingOrder = 0;
            
            mod_cam.gameObject.SetActive(true);
            mod_canvas.sortingOrder = 1;

        }

    }

    public void Init_Displays()
    {
        int numOfDisplayes = Display.displays.Length;

        for (int i = 0; i < numOfDisplayes; i++)
        {
            Display.displays[i].Activate();
        }
        Screen.SetResolution(3840, 2160, true);
        if (numOfDisplayes > 1)
        {
            playerCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Display.displays[0].systemWidth * 1920 / Display.displays[1].systemWidth, 0);
        }
        else
        {
            Debug.Log("Connect a second screen to play the game. For now you just have a moderator view");
        }
       

    }

}
