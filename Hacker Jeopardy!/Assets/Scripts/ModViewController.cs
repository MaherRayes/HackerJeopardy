using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModViewController : MonoBehaviour
{

    public Text mod_Category;
    public Text mod_Value;
    public Text mod_Answer;
    public GameObject answer;

    public GameObject wager;
    public GameObject wager_Error;
    public int money_wagered;
    private string enter_wager;

    public GameObject soloVideoController;
    public GameObject dualVideoController1;
    public GameObject dualVideoController2;
    public GameObject soloAudioController;
    public GameObject dualAudioController1;
    public GameObject dualAudioController2;
    

    public void Prep_Clue(Clue clue, string cat, bool dailyJeo, bool doubleJeo)
    {
        mod_Category.text = cat;
        mod_Answer.text = clue.GetAnswer();    

        if (dailyJeo)
        {
            mod_Value.text = "$???";
            wager.SetActive(true);
            answer.SetActive(false);
        }
        else
        {
            mod_Value.text = "$" + clue.GetAward(doubleJeo).ToString();
            wager.SetActive(false);
            answer.SetActive(true);
            money_wagered = 0;
        }


        //Depending on the clue: Activate the fitting Audio/Video controller
        HideControllers();
        if (!clue.IsDual())
        {
            SingleClue c = (SingleClue)clue;
            if (c.GetClueType() == Clue_Type.VIDEO)
            {
                soloVideoController.SetActive(true);
            }
            else if (c.GetClueType() == Clue_Type.AUDIO)
            {
                soloAudioController.SetActive(true);
            }
            
        }
        else
        {
            DualClue c = (DualClue)clue;
            if (c.GetType1() == Clue_Type.VIDEO)
            {
                dualVideoController1.SetActive(true);
            }
            else if (c.GetType1() == Clue_Type.AUDIO)
            {
                dualAudioController1.SetActive(true);
            }

            if (c.GetType2() == Clue_Type.VIDEO)
            {
                dualVideoController2.SetActive(true);
            }
            else if (c.GetType2() == Clue_Type.AUDIO)
            {
                dualAudioController2.SetActive(true);
            }
            
        }
    }

    public void enter_Wager(string i)
    {
        enter_wager = i;
    }

    public bool finished_Wagering()
    {

        if (int.TryParse(enter_wager, out money_wagered) && money_wagered != 0)
        {
            mod_Value.text = "$" + enter_wager;
            wager.transform.GetChild(0).GetComponent<InputField>().text = "";
        }
        else
        {
            wager_Error.SetActive(true);
            wager.transform.GetChild(0).GetComponent<InputField>().text = "";
            return false;
        }

        
        if (money_wagered != 0)
        {
            wager.SetActive(false);
            wager_Error.SetActive(false);
            enter_wager = "";
            answer.SetActive(true);
            return true;
        }

        return false;
    }

    public int GetMoneyWagered()
    {
        return money_wagered;
    }

    private void HideControllers()
    {
        dualVideoController2.SetActive(false);
        dualAudioController2.SetActive(false);
        dualVideoController1.SetActive(false);
        dualAudioController1.SetActive(false);
        soloVideoController.SetActive(false);
        soloAudioController.SetActive(false);
    }

    
}
