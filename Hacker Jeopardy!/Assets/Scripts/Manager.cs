using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;
using GifPlayer;

public class Manager : MonoBehaviour
{
    List<Step> steps = new List<Step>();

    public GameObject active_Now;
    public ((int,int),Clue) active_Clue;

    public GameObject DailyDoublePanel;

    
    public bool button_Pressable = false;
    public Game game;
    public GameObject player_Prefab;
    public List<GameObject> score_Locations;
    public GameObject soloClueTemplate;
    public GameObject dualClueTemplate;
    private int remClues = 0;
    private List<MyPlayer> bannedPlayers = new List<MyPlayer>();
    //final Jeopardy
    public GameObject FinalClueModPanel;
    public FinalJeopardy finalJeopardy;
    //final score display
    public GameObject ScoresModPanel;
    public GameObject ScoresPlayerPanel;
    //Extra Controllers
    public GameSetup gameSetup;
    public MultiScreenController screen_Controller;
    public ModViewController mod_Controller;

    private void Start()
    {
        
       
    }
     

    private void Update()
    {
        
        //get the players input
        if (button_Pressable)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!bannedPlayers.Contains(game.GetPlayer(0)))
                {
                    game.GetPlayer(0).SetTurn(true);
                    button_Pressable = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                if (!bannedPlayers.Contains(game.GetPlayer(1)))
                {
                    game.GetPlayer(1).SetTurn(true);
                    button_Pressable = false;
                }
            } else if (game.GetPlayerCount() > 2)
                {
                    if (Input.GetKeyDown(KeyCode.F3))
                    {
                    if (!bannedPlayers.Contains(game.GetPlayer(2)))
                    {
                        game.GetPlayer(2).SetTurn(true);
                        button_Pressable = false;
                    }
                    }

                    if(game.GetPlayerCount() > 3)
                    {
                         if (Input.GetKeyDown(KeyCode.F4))
                         {
                        if (!bannedPlayers.Contains(game.GetPlayer(3)))
                        {
                            game.GetPlayer(3).SetTurn(true);
                            button_Pressable = false;
                        }
                         }
                }
            }
        }
    }

     public void FillClues(Game game)
     {

        this.game = game;
        this.finalJeopardy.SetGame(game);                
        this.remClues = this.game.GetCatCount() * 5;
        gameSetup.Setup(this.game);
        
    }


    public void DisplayClue(int cat, int index, GameObject button, bool isFinal)
    {
        Clue clue;
        if (!isFinal)
            clue = game.GetCatAt(cat).getClue(index);
        else
            clue = game.GetFinalJeopardyClue();

        active_Clue = ((cat,index),clue);

        bool isDailyDouble = game.IsDailyDouble((cat, index));

        if (isDailyDouble)
        {
            DailyDoublePanel.SetActive(true);
        }

        if (!clue.IsDual())
        {
            SingleClue c = (SingleClue) clue;
            soloClueTemplate.SetActive(true);
            active_Now = soloClueTemplate;
            switch (c.GetClueType())
            {

                case Clue_Type.TEXT:
                    GameObject text_Obj = soloClueTemplate.transform.GetChild(0).gameObject;
                    text_Obj.SetActive(true);
                    Text_Clue_Chosen(c.GetClue(), text_Obj);
                    break;

                case Clue_Type.IMAGE:
                    GameObject image_Obj = soloClueTemplate.transform.GetChild(1).gameObject;
                    image_Obj.SetActive(true);
                    Image_Clue_Chosen(c.GetClue(), image_Obj);
                    break;

                case Clue_Type.VIDEO:
                    GameObject video_Obj = soloClueTemplate.transform.GetChild(2).gameObject;
                    video_Obj.SetActive(true);
                    Video_Clue_Chosen(c.GetClue(), video_Obj, !isDailyDouble);
                    break;

                case Clue_Type.AUDIO:
                    GameObject audio_Obj = soloClueTemplate.transform.GetChild(3).gameObject;
                    audio_Obj.SetActive(true);
                    Audio_Clue_Chosen(c.GetClue(), audio_Obj, !isDailyDouble);
                    break;

                case Clue_Type.GIF:
                    GameObject gif_Obj = soloClueTemplate.transform.GetChild(4).gameObject;
                    gif_Obj.SetActive(true);
                    gif_Obj.GetComponent<UnityGif>().playGif(c.GetClue(), gif_Obj);
                    break;

            }
        }
        else
        {
            DualClue c = (DualClue)clue;
            dualClueTemplate.SetActive(true);
            dualClueTemplate.transform.GetChild(5).gameObject.SetActive(true);
            active_Now = dualClueTemplate;
            switch (c.GetType1())
            {

                case Clue_Type.TEXT:
                    GameObject text_obj1 = dualClueTemplate.transform.GetChild(0).gameObject;
                    text_obj1.SetActive(true);
                    Text_Clue_Chosen(c.GetClue1(), text_obj1);
                    break;

                case Clue_Type.IMAGE:
                    GameObject image_obj1 = dualClueTemplate.transform.GetChild(1).gameObject;
                    image_obj1.SetActive(true);
                    Image_Clue_Chosen(c.GetClue1(), image_obj1);
                    break;

                case Clue_Type.VIDEO:
                    GameObject video_obj1 = dualClueTemplate.transform.GetChild(2).gameObject;
                    video_obj1.SetActive(true);
                    Video_Clue_Chosen(c.GetClue1(), video_obj1, !isDailyDouble);
                    break;

                case Clue_Type.AUDIO:
                    GameObject audio_Obj1 = dualClueTemplate.transform.GetChild(3).gameObject;
                    audio_Obj1.SetActive(true);
                    Audio_Clue_Chosen(c.GetClue1(), audio_Obj1, !isDailyDouble);
                    break;

                case Clue_Type.GIF:
                    GameObject gif_Obj1 = dualClueTemplate.transform.GetChild(4).gameObject;
                    gif_Obj1.SetActive(true);
                    gif_Obj1.GetComponent<UnityGif>().playGif(c.GetClue1(), gif_Obj1);
                    break;

            }

            switch (c.GetType2())
            {

                case Clue_Type.TEXT:
                    GameObject text_obj2 = dualClueTemplate.transform.GetChild(6).gameObject;
                    text_obj2.SetActive(true);
                    Text_Clue_Chosen(c.GetClue2(), text_obj2);
                    break;

                case Clue_Type.IMAGE:
                    GameObject image_obj2 = dualClueTemplate.transform.GetChild(7).gameObject;
                    image_obj2.SetActive(true);
                    Image_Clue_Chosen(c.GetClue2(), image_obj2);
                    break;

                case Clue_Type.VIDEO:
                    GameObject video_obj2 = dualClueTemplate.transform.GetChild(8).gameObject;
                    video_obj2.SetActive(true);
                    Video_Clue_Chosen(c.GetClue2(), video_obj2, !isDailyDouble);
                    break;

                case Clue_Type.AUDIO:
                    GameObject audio_Obj2 = dualClueTemplate.transform.GetChild(9).gameObject;
                    audio_Obj2.SetActive(true);
                    Audio_Clue_Chosen(c.GetClue2(), audio_Obj2, !isDailyDouble);
                    break;

                case Clue_Type.GIF:
                    GameObject gif_Obj2 = dualClueTemplate.transform.GetChild(10).gameObject;
                    gif_Obj2.SetActive(true);
                    gif_Obj2.GetComponent<UnityGif>().playGif(c.GetClue2(), gif_Obj2);
                    break;
            }
        }

        screen_Controller.Screen_Switcher();

        if (!isFinal)
        {
            for (int i = 0; i < game.GetPlayerCount(); i++)
            {
                if(game.GetPlayer(i).IsUp())
                    steps.Add(new ClueStep(button, active_Now, game.GetPlayer(i)));
            }
            mod_Controller.Prep_Clue(clue, game.GetCatAt(cat).getName(), game.IsDailyDouble((cat, index)), game.IsDouble());
            
            if (!game.IsDailyDouble((cat, index)))
            {
                button_Pressable = true;
                for (int i = 0; i < game.GetPlayerCount(); i++)
                {
                    game.GetPlayer(i).SetTurn(false);
                }
            }
        }
        else
        {
            mod_Controller.Prep_Clue(clue, game.GetFinalJeopardyCat(), false, game.IsDouble());
        }

        this.remClues--;
    }

    private void Text_Clue_Chosen(string c, GameObject guiObject)
    {
        guiObject.SetActive(true);
        guiObject.GetComponentInChildren<Text>().text = c;
    }

    private void Image_Clue_Chosen(string c, GameObject guiObject)
    {
        guiObject.SetActive(true);
        IEnumerator i = loadImage(guiObject, c);
        StartCoroutine(i);
    }

    IEnumerator loadImage(GameObject img_Object, string new_Image)
    {
        WWW www = new WWW(new_Image);
        while (!www.isDone)
            yield return null;
        img_Object.GetComponent<RawImage>().texture = www.texture;
        img_Object.GetComponent<ImageScaling>().Scale_Image();
    }

    private void Video_Clue_Chosen(string c, GameObject guiObject, bool play)
    {
        guiObject.SetActive(true);
        IEnumerator i = loadVideo(guiObject, c, play);
        StartCoroutine(i);
    }

    IEnumerator loadVideo(GameObject vid_Object, string new_Video, bool play)
    {
        VideoPlayer vid_Player = vid_Object.GetComponent<VideoPlayer>();
        vid_Player.url = new_Video;
        vid_Player.Prepare();
        while (!vid_Player.isPrepared)
            yield return null;

        if(play)
            vid_Player.Play();
        vid_Object.GetComponent<VideoScaling>().Scale_Video();
    }

    private void Audio_Clue_Chosen(string c, GameObject guiObject, bool play)
    {
        guiObject.SetActive(true);
        IEnumerator i = loadAudio(guiObject, c, play);
        StartCoroutine(i);
    }

    IEnumerator loadAudio(GameObject aud_Object, string new_Audio, bool play)
    {
        AudioImporter importer = aud_Object.GetComponent<NAudioImporter>();

        importer.Import(new_Audio);

        while (!importer.isDone)
            yield return null;

        AudioSource audio = aud_Object.GetComponent<AudioSource>();
        audio.clip = importer.audioClip;
        if (play)
            audio.Play();
    }

    public void Undo()
    {
        if (steps.Count == 0)
            return;

        remClues++;
        Step s = steps[steps.Count - 1];
        s.Undo();
        if (s.BoardMode())
        {
            button_Pressable = false;
            DailyDoublePanel.SetActive(false);
            screen_Controller.Screen_Switcher();
        }
        else
        {
            PointStep p = (PointStep)s;
            button_Pressable = true;
            DisplayClue(p.getOldClue().Item1, p.getOldClue().Item2, null, false);

            steps.Remove(steps[steps.Count - 1]);
        }
        steps.Remove(s);

    }

    public void Rate(bool rate)
    {
        //check if one of the players had already pressed the button
        MyPlayer player = null;

        for (int i = 0; i < game.GetPlayerCount(); i++)
        {
            if (game.GetPlayer(i).IsUp()) {
                player = game.GetPlayer(i);
                break;
            }
        }
        if (player == null)
            return;


        //determine the amount of points that should be added/subtracted from the player
        int points = 0;
        if (rate)
        {
            if (mod_Controller.money_wagered == 0)
            {
                points = active_Clue.Item2.GetAward(game.IsDouble());
                
            }
            else
                points = mod_Controller.GetMoneyWagered();
        }
        else
        {
            if (mod_Controller.money_wagered == 0)
            {
                points = -active_Clue.Item2.GetAward(game.IsDouble());
                player.AddPoints(points);
                player.SetTurn(false);
                player.banPlayer();
                bannedPlayers.Add(player);
                button_Pressable = true;
                return;
            }
            else
                points = -mod_Controller.GetMoneyWagered();
        }
        player.AddPoints(points);

        //add all the necessary info to create a step that could be reversed.
        List<MyPlayer> players = new List<MyPlayer>(bannedPlayers);
        List<int> pointsList = new List<int>();
        for (int i = 0; i < players.Count; i++)
            pointsList.Add(-points);
        players.Add(player);
        pointsList.Add(points);
        steps.Add(new PointStep(players, pointsList, active_Clue.Item1, game.IsDailyDouble(active_Clue.Item1)));

        //reset the player's state and the screen
        for (int i = 0; i < bannedPlayers.Count; i++)
            bannedPlayers[i].unbanPlayer();
        bannedPlayers.RemoveRange(0,bannedPlayers.Count);

        for ( int i = 0; i < active_Now.transform.childCount; i++)
            active_Now.transform.GetChild(i).gameObject.SetActive(false);

        active_Now.SetActive(false);

        button_Pressable = false;
        screen_Controller.Screen_Switcher();

        //check if we should enter the final jeopardy stage
        if (this.remClues == 0)
        {
            finalJeopardy.checkFinalJeopardy();
        }

    }

    public void Skip()
    {
        //add all the necessary info to create a step that could be reversed.
        List<int> pointsList = new List<int>();
        foreach (MyPlayer player in bannedPlayers)
            pointsList.Add(-active_Clue.Item2.GetAward(game.IsDouble()));
        steps.Add(new PointStep(bannedPlayers, pointsList, active_Clue.Item1, game.IsDailyDouble(active_Clue.Item1)));

        //reset the player's state and the screen
        for (int i = 0; i < bannedPlayers.Count; i++)
            bannedPlayers[i].unbanPlayer();

        if (bannedPlayers.Count != 0)
        {
            bannedPlayers[bannedPlayers.Count - 1].SetTurn(true);
            bannedPlayers.RemoveRange(0, bannedPlayers.Count);
        }
        else
        {
            //if the moderator skipped without any player pressing the button, activate the last player from the last step
            ClueStep c = (ClueStep)steps[steps.Count - 2];
            c.GetOldPlayer().SetTurn(true);
        }

        for (int i = 0; i < active_Now.transform.childCount; i++)
            active_Now.transform.GetChild(i).gameObject.SetActive(false);

        active_Now.SetActive(false);

        button_Pressable = false;
        screen_Controller.Screen_Switcher();

        //check if we should enter the final jeopardy stage
        if (this.remClues == 0)
        {
            finalJeopardy.checkFinalJeopardy();
        }
    }

    public void DisplayFinalClue()
    {
        if (finalJeopardy.ConfirmFinalWagers())
            DisplayClue(-1, -1, null, true);
        else
            return;
        
    }

    public void DisplayWinner()
    {

        if (finalJeopardy.CanConfirmRate())
        {
            List<MyPlayer> players = game.GetPlayers();
            players.Sort();
            for (int i = 0; i < game.GetPlayerCount(); i++)
            {
                MyPlayer player = game.GetPlayer(i);
                GameObject playerScore = ScoresPlayerPanel.transform.GetChild(0).GetChild(i).gameObject;
                playerScore.SetActive(true);
                playerScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.GetName();
                playerScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "$" + player.GetPoints().ToString();

                GameObject playerModScore = ScoresModPanel.transform.GetChild(0).GetChild(i).gameObject;
                playerModScore.SetActive(true);
                playerModScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "#" + (i + 1) + " " + player.GetName();
                playerModScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "$" + player.GetPoints().ToString();
            }
            for (int i = 0; i < active_Now.transform.childCount; i++)
                active_Now.transform.GetChild(i).gameObject.SetActive(false);
            //turn off old Panels
            FinalClueModPanel.SetActive(false);
            active_Now.SetActive(false);
            //turn on score panels
            ScoresPlayerPanel.SetActive(true);
            ScoresModPanel.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseDailyDoublePanel()
    {
        
       
        if (mod_Controller.finished_Wagering())
        {
            DailyDoublePanel.SetActive(false);
            if (active_Clue.Item2.IsDual())
            {

                if (dualClueTemplate.transform.GetChild(2).gameObject.activeSelf)
                    dualClueTemplate.transform.GetChild(2).GetComponent<VideoPlayer>().Play();
                else if (dualClueTemplate.transform.GetChild(3).gameObject.activeSelf)
                    dualClueTemplate.transform.GetChild(3).GetComponent<AudioSource>().Play();

                if (dualClueTemplate.transform.GetChild(8).gameObject.activeSelf)
                    dualClueTemplate.transform.GetChild(8).GetComponent<VideoPlayer>().Play();
                else if (dualClueTemplate.transform.GetChild(9).gameObject.activeSelf)
                    dualClueTemplate.transform.GetChild(9).GetComponent<AudioSource>().Play();

            }
            else
            {
                if (soloClueTemplate.transform.GetChild(2).gameObject.activeSelf)
                    soloClueTemplate.transform.GetChild(2).GetComponent<VideoPlayer>().Play();
                else if (soloClueTemplate.transform.GetChild(3).gameObject.activeSelf)
                    soloClueTemplate.transform.GetChild(3).GetComponent<AudioSource>().Play();

            }
        }
    }

    
}