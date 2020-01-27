using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShowCluesOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int catIndex ;
    private int clueIndex;
    private List<int> selectedCats;
    private Dictionary<int, Category> categories;
    public TMP_Text clue;
    private void Start()
    {
        clue = GameObject.FindGameObjectWithTag("Clue").GetComponent<TMP_Text>();
    }

    public void SetUpButton(int catIndex, int clueIndex, List<int> selectedCats, Dictionary<int,Category> categories)
    {
        this.catIndex = catIndex;
        this.clueIndex = clueIndex;
        this.selectedCats = selectedCats;
        this.categories = categories;
    }
    private void ShowClue()
    {
        int id = selectedCats[catIndex];
        Clue c = categories[id].getClues()[clueIndex];
        if (c.IsDual())
        {
            DualClue dual = (DualClue)c;
            clue.text = dual.GetClue1();
        }
        else
        {
            SingleClue singel = (SingleClue)c;
            clue.text = singel.GetClue();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowClue();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        clue.text = "";
    }
}
