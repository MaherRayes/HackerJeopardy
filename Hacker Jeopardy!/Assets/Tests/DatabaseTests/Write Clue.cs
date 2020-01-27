using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using NUnit.Framework;

public class DataBaseUnitTest
{
    SQLiteManager sql;

    //single clue
    Clue textClue;
    Clue vidClue;
    Clue imgClue;
    Clue audioClue;

    //dual clues
    Clue textVidClue;
    Clue imgAudioClue;
    Clue textTextClue;



    [SetUp]
    public void Setup()
    {
        textClue = new SingleClue("clue 1", "answer clue text", 1);
        vidClue = new SingleClue("clue 2.mp4", "answer clue vid", 1);
        imgClue = new SingleClue("clue 3.jpg", "answer clue img", 1);
        audioClue = new SingleClue("clue 4.mp3", "answer clue audio", 1);

        textVidClue = new DualClue("clue 1", "clue 2.mp4", "answer text vid", 1);
        imgAudioClue = new DualClue("clue 3.jpg", "clue 4.mp3", "answer img audio", 1);
        textTextClue = new DualClue("clue 1","clue 1", "answer clue text", 1);

        sql = new SQLiteManager("URI=file:" + Application.dataPath + "/Tests/DatabaseTests/WriteClueDB.sqlite");
        sql.removeAllClues();
        sql.removeAllCategories();
    }

    private bool AreEqual(Clue clue1, Clue clue2)
    {
        if (clue1 is SingleClue) 
        {
            if (clue2 is SingleClue)
            {
                SingleClue c1 = (SingleClue)clue1;
                SingleClue c2 = (SingleClue)clue2;
                return (c1.GetAnswer() == c2.GetAnswer() && c1.GetDifficulty() == c2.GetDifficulty() && c1.GetClueType() == c2.GetClueType() && c1.GetClue() == c2.GetClue());
            }
            else
                return false;
        }
        else
        {
            if(clue2 is DualClue)
            {
                DualClue c1 = (DualClue)clue1;
                DualClue c2 = (DualClue)clue2;
                return (c1.GetAnswer() == c2.GetAnswer() && c1.GetDifficulty() == c2.GetDifficulty() && c1.GetType1() == c2.GetType1() && c1.GetType2() == c2.GetType2()
                        && c1.GetClue1() == c2.GetClue1() && c1.GetClue2() == c2.GetClue2());
            }
            else
                return false;
        }
    }

    //insert a couple of clues and check their ID 
    [Test]
    public void TestClueID()
    {
        sql.WriteClue(0, textClue);
        sql.WriteClue(0, vidClue);
        Dictionary<int, Clue> clues = sql.ReadCluesOfCategory(0);
        Assert.AreEqual(clues[0].GetID(), textClue.GetID());
        Assert.AreEqual(clues[1].GetID(), vidClue.GetID());
    }

    //test if Clue is removed after calling remove clue
    [Test]
    public void TestClueRemove() 
    {
        sql.WriteClue(0, textClue);
        sql.WriteClue(0, vidClue);
        sql.removeSpecificClue(textClue.GetID());
        Assert.AreEqual(sql.ReadCluesOfCategory(0).Keys.Count, 1);
        Assert.That(() => sql.ReadCluesOfCategory(0)[1], Throws.TypeOf<KeyNotFoundException>());
    }

    //test if all Clues are removed after calling remove all clues
    [Test]
    public void TestClueRemoveAll()
    {
        sql.WriteClue(0, textClue);
        sql.WriteClue(0, vidClue);
        sql.removeAllClues();
        Assert.AreEqual(sql.ReadCluesOfCategory(0).Keys.Count, 0);
        Assert.That(() => sql.ReadCluesOfCategory(0)[0], Throws.TypeOf<KeyNotFoundException>());
        Assert.That(() => sql.ReadCluesOfCategory(0)[1], Throws.TypeOf<KeyNotFoundException>());
    }


    //test the writing and reading of single clues
    [Test]
    public void TestWriteSingleClues()
    {
        sql.WriteClue(0, textClue);
        sql.WriteClue(0, vidClue);
        sql.WriteClue(0, imgClue);
        sql.WriteClue(0, audioClue);
        Dictionary<int, Clue> clues = sql.ReadCluesOfCategory(0);
        
        Assert.IsTrue(AreEqual(clues[0], textClue));
        Assert.IsTrue(AreEqual(clues[1], vidClue));
        Assert.IsTrue(AreEqual(clues[2], imgClue));
        Assert.IsTrue(AreEqual(clues[3], audioClue));

        Assert.AreEqual(clues.Count, 4);
    }

    //test the writing and reading of dual clues
    [Test]
    public void TestWriteDualClues()
    {
        sql.WriteClue(0, textVidClue);
        sql.WriteClue(0, imgAudioClue);
        sql.WriteClue(0, textTextClue);
        Dictionary<int, Clue> clues = sql.ReadCluesOfCategory(0);

        Assert.IsTrue(AreEqual(clues[0], textVidClue));
        Assert.IsTrue(AreEqual(clues[1], imgAudioClue));
        Assert.IsTrue(AreEqual(clues[2], textTextClue));

        Assert.AreEqual(clues.Count, 3);
    }

    //tests what is the Highest clue ID in the DB
    [Test]
    public void TestHighestClueID()
    {
        sql.WriteClue(0, textClue);
        sql.WriteClue(0, textClue);
        sql.WriteClue(1, imgClue);
        
        Assert.AreEqual(sql.getHighestCluesID(), 3);
    }


}