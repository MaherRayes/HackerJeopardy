using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalJeopardy : MonoBehaviour
{
    //final Jeopardy objects
    private Game game;
    public GameObject FinalJeopardyModPanel;
    public GameObject FinalJeopardyPlayerPanel;
    public GameObject FinalClueModPanel;
    public List<GameObject> RightWrongButtons;
    private List<int> wagers = new List<int>();
    public TextMeshProUGUI warning;
    private bool[] undo;


    public void SetGame(Game game)
    {
        this.game = game;
    }

    public void checkFinalJeopardy()
    {
      
        FinalJeopardyModPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Category: " + game.GetFinalJeopardyCat();
        FinalJeopardyPlayerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = game.GetFinalJeopardyCat();
        GameObject finalWagers = FinalJeopardyModPanel.transform.GetChild(0).gameObject;
        List<MyPlayer> removedPlayers = new List<MyPlayer>();
        //Remove bad players
        for (int index = 0; index < game.GetPlayerCount(); index++)
        {
            MyPlayer pl = game.GetPlayer(index);
            if (pl.GetPoints() <= 0)
            {
                removedPlayers.Add(pl);
                pl.banPlayer();
                continue;
            }
        }
        for (int i = 0; i < removedPlayers.Count; i++)
        {
            game.RemovePlayer(removedPlayers[i]);
        }
        for (int i = 0; i < game.GetPlayerCount(); i++)
        {
            GameObject playerField = finalWagers.transform.GetChild(i).gameObject;
            playerField.SetActive(true);
            playerField.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = game.GetPlayer(i).GetName();
            game.GetPlayer(i).SetTurn(false);
        }
        this.FinalJeopardyModPanel.SetActive(true);
        this.FinalJeopardyPlayerPanel.SetActive(true);
        undo = new bool[game.GetPlayerCount()];
    }

    public bool ConfirmFinalWagers()
    {
        GameObject finalWagers = FinalJeopardyModPanel.transform.GetChild(0).gameObject;
        wagers.Clear();
        for (int i = 0; i < game.GetPlayerCount(); i++)
        {
            int money = 0;
            if (!int.TryParse(finalWagers.transform.GetChild(i).GetChild(0).GetComponent<TMP_InputField>().text,out money))
            {
                warning.gameObject.SetActive(true);
                warning.text = "please only enter numbers";
                return false;
            }

            if (money < 0)
            {
                warning.gameObject.SetActive(true);
                warning.text = "all wagers should be non negative";
                return false;
            }

            if (money > game.GetPlayer(i).GetPoints())
            {
                warning.gameObject.SetActive(true);
                warning.text = "player " + (i+1) + " doesn't have " + money + "$";
                return false;
            }
            this.wagers.Add(money);
        }
        this.FinalJeopardyModPanel.SetActive(false);
        this.FinalJeopardyPlayerPanel.SetActive(false);
        FinalMod();
        return true;
    }

    private void FinalMod()
    {
        Clue finalClue = game.GetFinalJeopardyClue();
        //setup final clue
        FinalClueModPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Category: " + game.GetFinalJeopardyCat();
        FinalClueModPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Answer: " + finalClue.GetAnswer();
        GameObject finalWagers = FinalClueModPanel.transform.GetChild(0).gameObject;
        for (int i = 0; i < game.GetPlayerCount(); i++)
        {
            MyPlayer pl = game.GetPlayer(i);
            GameObject playerField = finalWagers.transform.GetChild(i).gameObject;
            playerField.SetActive(true);
            playerField.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.GetName();
            playerField.transform.GetChild(1).GetComponent<Text>().text = this.wagers[i].ToString() + " $";
        }
        this.FinalClueModPanel.SetActive(true);
    }

    public void FinalRate(bool isRight, int player)
    {
        RightWrongButtons[player].transform.GetChild(0).gameObject.SetActive(false);
        RightWrongButtons[player].transform.GetChild(1).gameObject.SetActive(false);
        RightWrongButtons[player].transform.GetChild(2).gameObject.SetActive(true);
        if (isRight)
            game.GetPlayer(player).AddPoints(wagers[player]);
        else
            game.GetPlayer(player).AddPoints(-wagers[player]);
    }

    public void FinalRateRight(int player)
    {
        FinalRate(true, player);
        undo[player] = true;
    }

    public void FinalRateWrong(int player)
    {
        FinalRate(false, player);
        undo[player] = false;
    }

    public void UndoRate(int player)
    {
        RightWrongButtons[player].transform.GetChild(0).gameObject.SetActive(true);
        RightWrongButtons[player].transform.GetChild(1).gameObject.SetActive(true);
        RightWrongButtons[player].transform.GetChild(2).gameObject.SetActive(false);

        if (undo[player])
            game.GetPlayer(player).AddPoints(-wagers[player]);
        else
            game.GetPlayer(player).AddPoints(wagers[player]);
    }

    public bool CanConfirmRate()
    {
        foreach(GameObject g in RightWrongButtons)
        {
            if (g.transform.GetChild(0).gameObject.activeSelf && g.transform.parent.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}