using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Messeger : MonoBehaviour
{
    public Game game;
    public GameObject warning;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void gamePlay()
    {
        if (!CheckTwoDisplays())
            return;

        if (getGameFromDropdown())
        {
            game.SetDoubleJeopardy(GameObject.FindGameObjectWithTag("Double").GetComponent<Toggle>().isOn);
            IEnumerator i = LoadAndSet();
            StartCoroutine(i);
        }

    }
    private bool CheckTwoDisplays()
    {
        if (Display.displays.Length < 2)
        {
            warning.SetActive(true);
            return false;
        }
        else
            return true;
    }

    public void PlayDemo()
    {
        if (!CheckTwoDisplays())
            return;

        //Initialising the demo game
        List<Category> cats = new List<Category>();
        Dictionary<int, Clue> clues1 = new Dictionary<int, Clue>();
        Dictionary<int, Clue> clues2 = new Dictionary<int, Clue>();
        Dictionary<int, Clue> clues3 = new Dictionary<int, Clue>();
        Dictionary<int, Clue> clues4 = new Dictionary<int, Clue>();
        Dictionary<int, Clue> clues5 = new Dictionary<int, Clue>();
        clues1.Add(0, new SingleClue("The screen that displays what you are doing on the computer", "What is the monitor?", 1));
        clues1.Add(1, new SingleClue("The port in which you can plug in many add-ons to the computer, including storage device such as a flash drive.", "What is the USB port?", 2));
        clues1.Add(2, new SingleClue("The card that lets the computer display what you are doing on the computer.", "What is the graphics card?", 3));
        clues1.Add(3, new SingleClue("The main component of the entire PC or laptop to which almost all parts of the computer are connected to.", "What is the motherboard?", 4));
        clues1.Add(4, new SingleClue("The memory that loads data from the hard drive to increase speed and reduce lag time. The more of this you have, the faster your computer will be.", "What is RAM?", 5));

        clues2.Add(0, new SingleClue("The full name of the newest Apple operating systems", "What is Internet Operating System 7", 1));
        clues2.Add(1, new SingleClue("The three most well know operating systems.", "What is Linux, Windows, and Mac.", 2));
        clues2.Add(2, new SingleClue("The name of the company that creates the Android OS's that are named after deserts, such as Ice Cream Sandwich.", "What is Google", 3));
        clues2.Add(3, new SingleClue("The first Windows operating system to have a 64 bit version.", "What is Windows XP", 4));
        clues2.Add(4, new SingleClue("The name of the last version of Windows based on MS-DOS.", "What is Windows 98", 5));

        clues3.Add(0, new SingleClue("Used to type up essays, letters, and other documents made by the company that was founded by William H. Gates III", "What is Microsoft Word?", 1));
        clues3.Add(1, new SingleClue("The default internet browser that comes with every Microsoft Windows PC.", "What is Internet Explorer?", 2));
        clues3.Add(2, new SingleClue("This piece of software allows you to talk to people around the world with the same software and has a blue logo.", "What is Skype?", 3));
        clues3.Add(3, new SingleClue("The built in software in Windows that allows you to check the processes that are running and end them.", "What is Task Manager?", 4));
        clues3.Add(4, new SingleClue("The built in Mac software that allows you to create and edit all aspects of a song using imported songs or pre-loaded tunes.", "What is GarageBand", 5));

        clues4.Add(0, new SingleClue("The main component that processes information of a computer. It can come in many variations of speed and power.", "What is the processor?", 1));
        clues4.Add(1, new SingleClue("The names of the two main processor brands.", "What is Intel and AMD?", 2));
        clues4.Add(2, new SingleClue("The unit in which a processor's clock speed is measured.", "What is gigahertz?", 3));
        clues4.Add(3, new SingleClue("This Intel processor is the least powerful dual-core processor of the brand", "What is Intel Core i3?", 4));
        clues4.Add(4, new SingleClue("The company that has created the Tegra 3 processor, which is mainly featured in tablets and one handheld gaming system.", "What is Nvidia?", 5));

        clues5.Add(0, new DualClue(Application.dataPath + "/Resources/pa.jpg", Application.dataPath + "/Resources/pb.jpg", "dual clue", 1));
        clues5.Add(1, new SingleClue(Application.dataPath + "/Resources/d.mp3",  "What is Despacito", 2));
        clues5.Add(2, new SingleClue(Application.dataPath + "/Resources/avenger.mp4", "Who is avengers", 3));
        clues5.Add(3, new SingleClue(Application.dataPath + "/Resources/c.jpg", "What is Aliens", 4));
        clues5.Add(4, new SingleClue(Application.dataPath + "/Resources/tekken.jpg", "What is tekken", 5));

        Category computers = new Category(1, "Computers", "Questions about Computers", clues1);
        Category operatingSystems = new Category(2, "Operating Systems", "Questinos about Operating Systems", clues2);
        Category software = new Category(3, "Software", "Questions about Software", clues3);
        Category processor = new Category(4, "Processors", "Questions about processors", clues4);
        Category media = new Category(5, "Media", "Questions about videos and music and photos", clues5);
        List <(int, int)> lelist = new List<(int, int)>();
        lelist.Add((3, 3));
        lelist.Add((2, 5));
        lelist.Add((4, 4));

        cats.Add(computers);
        cats.Add(operatingSystems);
        cats.Add(software);
        cats.Add(processor);
        cats.Add(media);

        this.game = new Game(0, "demo", "just a test", cats, new SingleClue("Finale", "true.", 1), true, lelist, "final category");

        IEnumerator i = LoadAndSet();
        StartCoroutine(i);
    }

    IEnumerator LoadAndSet()
    {

        AsyncOperation a = SceneManager.LoadSceneAsync("InGame", LoadSceneMode.Single);
        while (!a.isDone)
            yield return null;

        GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>().FillClues(this.game);

    }


    private bool getGameFromDropdown()
    {
        TMP_Dropdown chooseGameDropDown = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameEditor>().chooseGameDropDown;
        if (chooseGameDropDown.value >= 1)
        {

            int index = chooseGameDropDown.value - 1;
            this.game = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameEditor>().GetGames()[index];
            return true;
        }
        return false;
    }

}