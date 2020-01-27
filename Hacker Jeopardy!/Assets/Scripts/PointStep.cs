using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointStep : Step
{

    List<MyPlayer> players;
    List<int> points;
   (int, int) clue;
    bool isDailyDouble;

    public PointStep(List<MyPlayer> players, List<int> points, (int,int) clue, bool isDailyDouble)
    {
        this.players = new List<MyPlayer>(players);
        this.points = new List<int>(points);
        this.clue = clue;
        this.isDailyDouble = isDailyDouble;
    }

    public override void Undo()
    {
        
        for (int i = 0; i < players.Count; i++)
        {
            players[i].AddPoints(-points[i]);
            if (!isDailyDouble)
                players[i].SetTurn(false);
        }
    }

    public override bool BoardMode()
    {
        return false;
    }

    public (int, int) getOldClue()
    {
        return clue;
    }


}
