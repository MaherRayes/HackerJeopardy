using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClue : Clue
{
    string clue;

    public SingleClue(string clue, string answer, int difficulty) : base(answer,difficulty)
    {
        this.clue = clue;
    }

    public string GetClue()
    {
        return this.clue;
    }
    public override bool IsDual()
    {
        return false; 
    }
    public Clue_Type GetClueType()
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
