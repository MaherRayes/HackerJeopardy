using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoneyButton : MonoBehaviour, IPointerClickHandler
{
    private GameObject doublegaenger;
    public int category;
    public int index;
    public Manager manager;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();
        
    }

    private void Start()
    {
        if(doublegaenger == null)
            doublegaenger = GameObject.Find(gameObject.name + "S");
    }

    private void Update()
    {
        if (!manager.game.IsDouble())
            this.GetComponent<Text>().text = "$ " + (100 * (index + 1));
        else
            this.GetComponent<Text>().text = "$ " + (200 * (index + 1));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
            manager.DisplayClue(category, index, this.gameObject, false);
            gameObject.SetActive(false);
        if (doublegaenger != null)
            doublegaenger.SetActive(false);
    }

    public void activate()
    {
        gameObject.SetActive(true);
        if(doublegaenger != null)
            doublegaenger.SetActive(true);
    }
}
