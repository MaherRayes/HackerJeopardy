using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualClue : Clue
{
    string firstClue;
    string secoundClue; 

    public DualClue(string clue1,string clue2, string answer, int difficulty) :base (answer ,difficulty)
    {
        this.firstClue = clue1;
        this.secoundClue = clue2; 
    }
    public override bool IsDual()
    {
        return true;
    }

    public string GetClue1()
    {
     
        return this.firstClue;
    }
    public string GetClue2()
    {
        return this.secoundClue; 
    }
    public Clue_Type GetType1()
    {
        return GetType(firstClue);
    }
    public Clue_Type GetType2()
    {
        return GetType(secoundClue);
    }



    private Clue_Type GetType(string clue)
    {
        if (System.IO.File.Exists(clue))
        {
            if (clue.EndsWith(".mp4") || clue.EndsWith(".mkv")) return Clue_Type.VIDEO;
            else if (clue.EndsWith(".png") || clue.EndsWith(".jpg") || clue.EndsWith(".jpeg")) return Clue_Type.IMAGE;
            else if (clue.EndsWith(".mp3") || clue.EndsWith(".wav")) return Clue_Type.AUDIO;
            else if (clue.EndsWith(".gif")) return Clue_Type.GIF;
            else return Clue_Type.NONE;

        }
        else
        {
            return Clue_Type.TEXT;
        }
    }
}
 