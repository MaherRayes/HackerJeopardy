using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueStep : Step
{

    GameObject button;
    GameObject activeClue;
    MyPlayer player;

    public ClueStep( GameObject button, GameObject activeClue, MyPlayer player)
    {
        this.button = button;
        this.activeClue = activeClue;
        this.player = player;
    }

    public override void Undo()
    {
        button.GetComponent<MoneyButton>().activate();
        for (int i = 0; i < activeClue.transform.childCount; i++)
            activeClue.transform.GetChild(i).gameObject.SetActive(false);
        activeClue.SetActive(false);
        player.SetTurn(true);
    }

    public override bool BoardMode()
    {
        return true;
    }

    public MyPlayer GetOldPlayer()
    {
        return player;
    }
}
