using UnityEngine;
using System.Collections;
using SimpleFileBrowser;
using UnityEngine.UI;
using SmartDLL;
using TMPro;
using System;

public class FileExplorer : MonoBehaviour
{
    // Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
    // Warning: FileBrowser can only show 1 dialog at a time
    public TMP_InputField path;
    SmartFileExplorer fileExplorer = new SmartFileExplorer();
    bool readText = false;
    public void OpenFile()
    {

   
        // Set filters (optional)
        // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
        // if all the dialogs will be using the same filters
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png",".jpeg",".gif"), new FileBrowser.Filter("Audio", ".mp3", ".wav"), new FileBrowser.Filter("Video", ".mp4", ".mkv"));

        // Set default filter that is selected when the dialog is shown (optional)
        // Returns true if the default filter is set successfully
        // In this case, set Images filter as the default filter
        FileBrowser.SetDefaultFilter(".jpg");

        // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        // Note that when you use this function, .lnk and .tmp extensions will no longer be
        // excluded unless you explicitly add them as parameters to the function
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        // It is sufficient to add a quick link just once
        // Name: Users
        // Path: C:\Users
        // Icon: default (folder icon)
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        // Show a save file dialog 
        // onSuccess event: not registered (which means this dialog is pretty useless)
        // onCancel event: not registered
        // Save file/folder: file, Initial path: "C:\", Title: "Save As", submit button text: "Save"
        // FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save As", "Save" );

        // Show a select folder dialog 
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
        // FileBrowser.ShowLoadDialog( (path) => { Debug.Log( "Selected: " + path ); }, 
        //                                () => { Debug.Log( "Canceled" ); }, 
        //                                true, null, "Select Folder", "Select" );

        // Coroutine example
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, "", "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)

    
  
        if (FileBrowser.Success)
        {
            string file = FileBrowser.Result;
            
            if (file.EndsWith(".mp3") || file.EndsWith(".wav") || file.EndsWith(".mp4") || file.EndsWith(".mkv") || file.EndsWith(".png") || file.EndsWith(".gif") || file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
                path.text = file;             // Absoulut path
            Debug.Log(Application.dataPath);
        }
       
    }
    public static string MakeRelative(string filePath, string referencePath)        // make a relativ path  
    {
        var fileUri = new Uri(filePath);
        var referenceUri = new Uri(referencePath);
        return referenceUri.MakeRelativeUri(fileUri).ToString();
    }
    /* private void Update()
     {
         while (readText)
         {
             this.path.text = fileExplorer.fileName;
             readText = false; 
         }
     }
     public void ShowExplorer()
     {
         string initialDir = @"C:\";
         bool restoreDir = true;
         string title = "Open a File";
         string defExt = "txt";
         string filter = "txt files (*.txt)|*.txt";

         fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
         readText = true;
     }
     */

}