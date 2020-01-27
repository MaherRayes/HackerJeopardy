using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class GameSetup : MonoBehaviour
{
    public List<GameObject> categoryTemplates;
    //boards
    public GameObject playersBoard;
    public GameObject playersBoard2;
    public GameObject CategoryIntro;
    public GameObject CategoryInro2;
    //players' names Input Fields Toggles
    public GameObject playersNames;
    public Toggle[] championPlayers;
    //Category name Text
    public TextMeshProUGUI currentCategoryNumber;
    public TextMeshProUGUI currentCategoryName;
    public TextMeshProUGUI currentCategoryName2;
    //Category Description
    public TextMeshProUGUI currentCategoryDescription;
    //number of Players
    public TMP_Dropdown playersNumber;
    //player prefab related objects
    public List<GameObject> score_Locations;
    public GameObject player_Prefab;
    //warning text
    public GameObject warningObject;
    //intro
    public GameObject intro;
    //FadeIn FadeOut animation object
    public Animator animator;
    public Animator animator2;


    private int playerCount;
    private Game game;
    private int currentCat = 0;

    private void Start()
    {
        intro.GetComponent<VideoPlayer>().loopPointReached += CheckIntroEnd;
    }

    void CheckIntroEnd(UnityEngine.Video.VideoPlayer vp)
    {
        intro.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (intro.activeSelf)
                intro.SetActive(false);
            
            else
            {
                if (CategoryIntro.activeSelf)
                    StartCoroutine(FadeOut());
            }
        }
                    
    }

    IEnumerator FadeIn()
    {
        animator.SetTrigger("FadeIn");
        animator2.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator FadeOut()
    {
        animator.SetTrigger("FadeOut");
        animator2.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        IntroduceCategory();
        StartCoroutine(FadeIn());
    }


    public void Setup(Game game)
    {
        this.game = game;
        GameObject currentTemplate1 = categoryTemplates[game.GetCatCount() - 2];
        GameObject currentTemplate2 = categoryTemplates[game.GetCatCount() + 3];
        TemplateSetup(currentTemplate1,game);
        TemplateSetup(currentTemplate2,game);
    }

    public void InitPlayersFields()
    {
        string num = playersNumber.options[playersNumber.value].text;
        
        switch (num)
        {
            case "Enter Number of Players..":
                for (int i = 0; i < 4; i++)
                {
                    playersNames.transform.GetChild(i).gameObject.SetActive(false);
                }
                playerCount = 0;
                break;
            case "2":
                for (int i = 0; i < 2; i++)
                {
                    playersNames.transform.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = 2; i < 4; i++)
                {
                    playersNames.transform.GetChild(i).gameObject.SetActive(false);
                }
                playerCount = 2;
                break;
            case "3":
                for (int i = 0; i < 3; i++)
                {
                    playersNames.transform.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = 3; i < 4; i++)
                {
                    playersNames.transform.GetChild(i).gameObject.SetActive(false);
                }
                playerCount = 3;
                break;
            case "4":
                for (int i = 0; i < 4; i++)
                {
                    playersNames.transform.GetChild(i).gameObject.SetActive(true);
                }
                playerCount = 4;
                break;
        }

    }

    public void SavePlayers()
    {

        if(playerCount == 0)
        {
            warningObject.SetActive(true);
            warningObject.GetComponent<Text>().text = "PLEASE CHOOSE THE NUMBER OF PLAYERS";
            return;
        }

        int champs = 0;

        //Check that no field is empty
        for (int i = 0; i < playerCount; i++)
        {
            string name = playersNames.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
            if (name.Length == 1)
            {
                warningObject.SetActive(true);
                warningObject.GetComponent<Text>().text = "PLEASE FILL ALL PLAYERS' NAMES";
                return;
            }

            if (championPlayers[i].isOn)
                champs++;

        }

        if(champs != 1)
        {
            warningObject.SetActive(true);
            warningObject.GetComponent<Text>().text = "PLEASE SELECT JUST ONE PLAYER AS A RETURNING CHAMPION";
            return;
        }

        intro.SetActive(false);

        for (int i = 0; i < playerCount; i++) {
            string name = playersNames.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;

            List<Text> nameObjects = new List<Text>();
            List<Text> PointsObjects = new List<Text>();
            for (int j = 0; j < score_Locations.Count; j++)
            {
                GameObject p = Instantiate(player_Prefab, score_Locations[j].transform);
                p.transform.localPosition = new Vector3(score_Locations[j].transform.position.x + ((1600 - playerCount * 200) / (playerCount + 1)) + ((((1600 - playerCount*200)/(playerCount+1)) + 200)*i) +100, 0, 0);
                nameObjects.Add(p.transform.GetChild(0).GetComponent<Text>());
                PointsObjects.Add(p.transform.GetChild(1).GetComponent<Text>());
            }

            MyPlayer thisPlayer = new MyPlayer(name, nameObjects, PointsObjects);
            game.AddPlayer(thisPlayer);
            thisPlayer.SetTurn(championPlayers[i].isOn);
        }
        playersBoard.SetActive(false);
        playersBoard2.SetActive(false);
        CategoryIntro.SetActive(true);
        CategoryInro2.SetActive(true);
        IntroduceCategory();
       
    }

    public void IntroduceCategory()
    {
        
        if (currentCat == game.GetCatCount())
        {
            CategoryIntro.SetActive(false);
            CategoryInro2.SetActive(false);
            return;
        }
        Category cat = game.GetCatAt(currentCat);
        currentCategoryName.text = cat.getName();
        currentCategoryDescription.text = cat.getDescription();
        currentCategoryName2.text = cat.getName();
        currentCategoryNumber.text = "Category " + (currentCat+1) + ":";
        currentCat++;
    }

    private void TemplateSetup(GameObject template, Game game)
    {
        template.SetActive(true);

        //setup categories
        for (int i = 0; i < game.GetCatCount(); i++)
        {
            Text t = template.transform.GetChild(0).GetChild(i).GetComponent<Text>();
            t.text = game.GetCatAt(i).getName();
        }


    }
}
