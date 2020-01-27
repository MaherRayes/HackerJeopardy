using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class TestSuite
{
    public Manager manager;



    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GameObject"));
        manager = gameGameObject.transform.GetChild(7).GetComponent<Manager>();
        // messeger = gameGameObject.transform.GetChild(7).GetComponent<Messeger>();
        // Debug.Log(messeger == null);
        // Debug.Log(manager == null);

        // messeger = GameObject.FindGameObjectWithTag("Messeger").GetComponent<Messeger>();
        // Debug.Log(messeger == null);


        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        //  clues.Add(0, new SingleClue("What is a set of steps that create an ordered approach to a problem solution?", "Algorithm.", 1));
        clues.Add(0, new SingleClue(Application.dataPath + "/Image_Clues/gif.gif", "Algorithm.", 1));
        clues.Add(1, new SingleClue(Application.dataPath + "/Video_Clues/d.mp3", "Plain English/Pseudocode/Flowchart.", 2));
        clues.Add(2, new SingleClue(Application.dataPath + "/Video_Clues/avenger.mp4", "how to do it.", 3));
        clues.Add(3, new DualClue("1. Leave classroom. 2. Turn right out of school building. 3. Walk 1.2 miles. 4. Turn right on street. 5. Go to 4th house.", Application.dataPath + "/Image_Clues/d.jpg", "This is an example of an DClue TextImage.", 4));
        clues.Add(4, new SingleClue(Application.dataPath + "/Image_Clues/d.jpg", "This is an example of an algorithm.", 5));

        //  clues.Add(4, new DualClue("If the user does not provide the correct data into an algorithm, then you can use this to restart from a previous step.", Application.dataPath + "/Video_Clues/avenger.mp4", "Loop.", 5));
        Dictionary<int, Clue> clues2 = new Dictionary<int, Clue>();

        clues2.Add(0, new DualClue(Application.dataPath + "/Image_Clues/d.jpg", Application.dataPath + "/Image_Clues/gif.gif", "This is an example of an DClue ImageGif.", 1));
        clues2.Add(1, new DualClue(Application.dataPath + "/Video_Clues/avenger.mp4", "1. Leave classroom. 2. Turn right out of school building. 3. Walk 1.2 miles. 4. Turn right on street. 5. Go to 4th house.", "This is an example of an DClue VideoText.", 2));
        clues2.Add(2, new DualClue(Application.dataPath + "/Video_Clues/d.mp3", Application.dataPath + "/Video_Clues/avenger.mp4", "This is an example of an DClue AudioVideo.", 3));
        clues2.Add(3, new DualClue(Application.dataPath + "/Video_Clues/d.mp3", Application.dataPath + "/Video_Clues/avenger.mp4", "This is an example of an DClue AudioVideo.", 4));
        clues2.Add(4, new DualClue(Application.dataPath + "/Image_Clues/gif.gif", Application.dataPath + "/Video_Clues/d.mp3", "This is an example of an DClue GifAudio.", 5));


        List<Category> cats = new List<Category>();
        cats.Add(new Category(0, "Algorithms", "Dealing with cool algorithms", clues));
        cats.Add(new Category(1, "Algorithms", "Dealing with cool algorithms", clues));
        cats.Add(new Category(2, "Algorithms", "Dealing with cool algorithms", clues));
        cats.Add(new Category(3, "Algorithms", "Dealing with cool algorithms", clues));
        cats.Add(new Category(4, "Algorithms", "Dealing with cool algorithms", clues2));

        

      //  List<Category> cats2 = new List<Category>();
      //  cats2.Add(new Category(0, "Algorithms", "Dealing with cool algorithms", clues2));
      //  cats2.Add(new Category(1, "Algorithms", "Dealing with cool algorithms", clues2));
      //  cats2.Add(new Category(2, "Algorithms", "Dealing with cool algorithms", clues2));
      //  cats2.Add(new Category(3, "Algorithms", "Dealing with cool algorithms", clues2));
      //  cats2.Add(new Category(4, "Algorithms", "Dealing with cool algorithms", clues2));




        List<(int, int)> lelist = new List<(int, int)>();
       lelist.Add((0, 0));
     //   lelist.Add((3, 3));

        Game game1 = new Game(0, "demo", "just a test", cats, new SingleClue("Finale", "true", 1), false, lelist, "final category");
     //   Game game2 = new Game(0, "demo2", "just a test2", cats2, new SingleClue("Finale", "true", 1), false, lelist, "final category");

        manager.FillClues(game1);

    }

    // A Test behaves as an ordinary method
    // Number of categories
    [Test]
    public void TestCategoriesNum()
    {
        Assert.AreEqual(manager.game.getCategoriesNum(), 5);
    }

    // GameName
    [Test]
    public void TestGameName()
    {
        Assert.AreEqual(manager.game.GetGameName(), "demo");
    }

    // remove one Category from DataBase
    [Test]
    public void TestGameRemoveCategoryMethod()
    {
        manager.game.RemoveCategory(0);

        Assert.AreEqual(manager.game.getCategoriesNum(), 4);

    }

    // Display FinalJeopardy Clue
    [Test]
    [Obsolete]
    public void TestManagerDisplayFinalJeopardyClue()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(3, 3, button, true);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "true");

        Assert.IsTrue(manager.active_Now.active);

    }

    // Display a VideoClue
    [Test]
    [Obsolete]
    public void TestManagerDisplayClueV()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "Plain English/Pseudocode/Flowchart.");

        Assert.IsTrue(manager.active_Now.active);

    }

    // Display a DoupleClueTexImage
    [Test]
    [Obsolete]
    public void TestManagerDisplayDoubleClueTI()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(3, 3, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "This is an example of an DClue TextImage.");

        Assert.IsTrue(manager.active_Now.active);

    }

    // Display a DoupleClueImageGif
    [Test]
    [Obsolete]
    public void TestManagerDisplayDoubleClueIG()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(4, 0, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "This is an example of an DClue ImageGif.");

        Assert.IsTrue(manager.active_Now.active);

    }

    // Display a DoupleClueVideoText
    [Test]
    [Obsolete]
    public void TestManagerDisplayDoubleClueVT()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(4, 1, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "This is an example of an DClue VideoText.");

        Assert.IsTrue(manager.active_Now.active);

    }

    // Display a DoupleClueAudioVideo
    [Test]
    [Obsolete]
    public void TestManagerDisplayDoubleClueAV()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(4, 2, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "This is an example of an DClue AudioVideo.");

        Assert.IsTrue(manager.active_Now.active);

    }

    // Display a DoupleClueGifAudio
    [Test]
    [Obsolete]
    public void TestManagerDisplayDoubleClueGA()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(4, 4, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "This is an example of an DClue GifAudio.");

        Assert.IsTrue(manager.active_Now.active);

    }


    // Display a ImageClue
    [Test]
    [Obsolete]
    public void TestManagerDisplayClueI()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(3, 4, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "This is an example of an algorithm.");

        Assert.IsTrue(manager.active_Now.active);
    }

    // Display a AudioClue
    [Test]
    public void TestManagerDisplayClueA()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(2, 2, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "how to do it.");

    }

    // Display a GifClue
    [Test]
    [Obsolete]
    public void TestManagerDisplayClueG()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "Algorithm.");

        Assert.IsTrue(manager.active_Now.active);

    }
    /*
    // Display a Clue two times
    [Test]
    public void TestManagerDisplayClueTwoTimes()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        button.GetComponent<MoneyButton>().manager = manager;

        Assert.AreEqual(Answer, "Plain English/Pseudocode/Flowchart.");

        Assert.That(() => manager.DisplayClue(1, 1, button, false), Throws.TypeOf<System.ArgumentOutOfRangeException>());

    }
    */

    // Difficultey Levels 
    [Test]
    public void TestManagerDisplayClueDifficulteyLevels()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        Assert.AreEqual(clue.GetDifficulty(), 2);

    }


    [Test]
    public void TestManagerDisplayClueGetAward()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;


        Assert.AreEqual(clue.GetAward(false), 100);

    }

    //disappear the Button after desplay the Clue
    [Test]
    [Obsolete]
    public void TestManagerDisplayClueThenDisappearButton()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.button_Pressable = true;
        button.GetComponent<MoneyButton>().manager = manager;

        button.GetComponent<MoneyButton>().OnPointerClick(null);
        

        manager.button_Pressable = false;
        Assert.IsFalse(button.active);
    }


    //catch Exception while you tring to Display a not existing Clue
    [Test]
    public void TestManagerDisplayNotExistingClue()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        /*
         // dies Methode ist auch richtig
        Exception exception = null;
        try
        {
            manager.DisplayClue(8, 8, button, false);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);*/

        Assert.That(() => manager.DisplayClue(8, 8, button, false), Throws.TypeOf<System.ArgumentOutOfRangeException>());

    }

    //catch Exception while the Button is null
    [Test]
    public void TestManagerDisplayCluButtonIsNull()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        Assert.That(() => manager.DisplayClue(8, 8, null, false), Throws.TypeOf<System.ArgumentOutOfRangeException>());

    }

    [Test]
    [Obsolete]
    public void TestManagerUndo()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);
        Assert.IsTrue(manager.screen_Controller.mod_cam.gameObject.active);
        manager.Undo();
        Assert.IsFalse(manager.screen_Controller.pub_cam.gameObject.active);

    }

    //___________ Test Rate right answer
    [Test]
    public void TestManagerRateAnswerIsCorrect()
    {
       
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        List<Text> nameObjects2 = new List<Text>();
        List<Text> pointsObjects2 = new List<Text>();

        MyPlayer p2 = new MyPlayer("P2", nameObjects2, pointsObjects2);

        manager.game.AddPlayer(p2);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.Rate(true);

        int points = manager.game.GetPlayer(0).GetPoints();

        Assert.AreEqual(points, 100);
    }

    //___________ Test Rate right answer
    [Test]
    public void TestManagerRateAnswerIsUncorrect()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);
        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();
        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        List<Text> nameObjects2 = new List<Text>();
        List<Text> pointsObjects2 = new List<Text>();
        MyPlayer p2 = new MyPlayer("P2", nameObjects2, pointsObjects2);

        manager.game.AddPlayer(p2);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.Rate(false);
        int points = manager.game.GetPlayer(0).GetPoints();

        Assert.AreEqual(points, -200);

    }

    //   Display the winner 
    [Test]
    [Obsolete]
    public void TestManagerDisplayTheWinner()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayFinalClue();

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();
        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        List<Text> nameObjects2 = new List<Text>();
        List<Text> pointsObjects2 = new List<Text>();
        MyPlayer p2 = new MyPlayer("P2", nameObjects2, pointsObjects2);

        manager.game.AddPlayer(p2);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.Rate(true);

        manager.DisplayWinner();

        Assert.False(manager.FinalClueModPanel.active);
        Assert.False(manager.active_Now.active);
        Assert.True(manager.ScoresPlayerPanel.active);
        Assert.True(manager.ScoresModPanel.active);
    
    }

    //___________ Test Wager points
    [Test]
    public void TestWagerPoints()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        List<Text> nameObjects2 = new List<Text>();
        List<Text> pointsObjects2 = new List<Text>();

        MyPlayer p2 = new MyPlayer("P2", nameObjects2, pointsObjects2);

        manager.game.AddPlayer(p2);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.mod_Controller.enter_Wager("400");
       // manager.mod_Controller.finished_Wagering();
        Assert.IsTrue(manager.mod_Controller.finished_Wagering());
       
        manager.Rate(true);

        Assert.AreEqual(manager.game.GetPlayer(0).GetPoints(), 400);

    }

    
    //___________ Test Wager points by giving an input char
    [Test]
    public void TestWagerPointsChar()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        List<Text> nameObjects2 = new List<Text>();
        List<Text> pointsObjects2 = new List<Text>();

        MyPlayer p2 = new MyPlayer("P2", nameObjects2, pointsObjects2);

        manager.game.AddPlayer(p2);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.mod_Controller.enter_Wager("A");

        Assert.IsFalse(manager.mod_Controller.finished_Wagering());
        Assert.IsTrue(manager.mod_Controller.wager_Error);
        
    }

    //Display a clue and thier is no player
    [Test]
    [Obsolete]
    public void TestManagerDisplayClueWithPlayerIsUp()
    {
        ((int, int), Clue) Test_Clue;
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        manager.game.GetPlayer(0).SetTurn(true);

        Test_Clue = manager.active_Clue;

        Clue clue = Test_Clue.Item2;

        string Answer = clue.GetAnswer();

        Assert.AreEqual(Answer, "Plain English/Pseudocode/Flowchart.");

        Assert.IsTrue(manager.active_Now.active);

    }

    [Test]
    [Obsolete]
    public void TestManagerUndoMoreSteps()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.Rate(true);

        manager.DisplayClue(1, 1, button, false);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.Rate(true);

        manager.Undo();
        manager.Undo();
        manager.Undo();
        manager.Undo();

        Assert.AreEqual(manager.game.GetPlayer(0).GetPoints(), 0);
    }

    [Test]
    [Obsolete]
    public void TestManagerSkip()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        List<Text> nameObjects = new List<Text>();
        List<Text> pointsObjects = new List<Text>();

        MyPlayer p = new MyPlayer("P", nameObjects, pointsObjects);

        manager.game.AddPlayer(p);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.Rate(true);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        manager.DisplayClue(1, 1, button, false);

        manager.Skip();

        Assert.IsTrue(manager.game.GetPlayer(0).IsUp());

    }

    [Test]
    public void TestManagerRatePlayerIsNull()
    {

        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        MyPlayer p1 = null;

        manager.game.AddPlayer(p1);

        Exception exception = new Exception();

        try
        {
            manager.game.GetPlayer(0).SetTurn(true);
            manager.Rate(false);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);

    }

    //_____ Test Wager is not Zero and false answer
    [Test]
    public void TestWagerNotZeroAndRateFalse()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);

        List<Text> nameObjects1 = new List<Text>();
        List<Text> pointsObjects1 = new List<Text>();

        MyPlayer p1 = new MyPlayer("P1", nameObjects1, pointsObjects1);

        manager.game.AddPlayer(p1);

        manager.game.GetPlayer(0).SetTurn(true);

        manager.mod_Controller.enter_Wager("400");

        manager.mod_Controller.finished_Wagering();

        manager.Rate(false);

        Assert.AreEqual(manager.game.GetPlayer(0).GetPoints(), -400);

    }

    //_____ Close Daily Double Panel
    [Test]
    [Obsolete]
    public void TestManagerCloseDailyDoublePanel()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(0, 0, button, false);


        Assert.IsTrue(manager.screen_Controller.mod_cam.gameObject.active);
        manager.CloseDailyDoublePanel();
        Assert.IsFalse(manager.screen_Controller.pub_cam.gameObject.active);

    }

    //_____ Close Daily Double Panel
    [Test]
    [Obsolete]
    public void TestManagerCloseDailyDoublePanelVideo()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(2, 2, button, false);


        Assert.IsTrue(manager.screen_Controller.mod_cam.gameObject.active);
        manager.CloseDailyDoublePanel();
        Assert.IsFalse(manager.screen_Controller.pub_cam.gameObject.active);

    }
    //_____ Close Daily Double Panel
    [Test]
    [Obsolete]
    public void TestManagerCloseDailyDoublePanelDual()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(3, 3, button, false);


        Assert.IsTrue(manager.screen_Controller.mod_cam.gameObject.active);
        manager.CloseDailyDoublePanel();
        Assert.IsFalse(manager.screen_Controller.pub_cam.gameObject.active);

    }
    //_____ Close Daily Double Panel
    [Test]
    [Obsolete]
    public void TestManagerCloseDailyDoublePanelAudio()
    {
        GameObject button = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Button"));

        manager.DisplayClue(1, 1, button, false);


        Assert.IsTrue(manager.screen_Controller.mod_cam.gameObject.active);
        manager.CloseDailyDoublePanel();
        Assert.IsFalse(manager.screen_Controller.pub_cam.gameObject.active);

    }


}
 