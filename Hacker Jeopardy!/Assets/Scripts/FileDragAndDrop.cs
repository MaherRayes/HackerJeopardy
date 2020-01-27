using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class FileDragAndDrop : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    private string file;
    public TMP_InputField path;

    // important to keep the instance alive while the hook is active.
    UnityDragAndDropHook hook;

    void OnFiles(List<string> aFiles, POINT aPos)
    {
            // do something with the dropped file names. aPos will contain the 
            // mouse position within the window where the files has been dropped.
            Debug.Log("Dropped " + aFiles.Count + " files at: " + aPos + "\n" +
                aFiles.Aggregate((a, b) => a + "\n" + b));
            file = aFiles.ElementAt(0);
        if (file.EndsWith(".mp3")|| file.EndsWith(".wav")|| file.EndsWith(".mp4")|| file.EndsWith(".mkv")|| file.EndsWith(".png")|| file.EndsWith(".gif")|| file.EndsWith(".jpg")|| file.EndsWith(".jpeg"))
            path.text =  file;

        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        hook = new UnityDragAndDropHook();
        hook.InstallHook();

       hook.OnDroppedFiles += OnFiles;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hook.UninstallHook();
    }

}
