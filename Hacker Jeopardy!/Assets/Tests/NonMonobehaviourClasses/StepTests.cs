using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;

public class StepTests : MonoBehaviour
{

    PointStep p;
    List<MyPlayer> players;
    List<int> points;
    (int, int) cluePosition;

    //////////////////////////////

    ClueStep c;
    GameObject dummyObject1 = new GameObject("lol");
    GameObject dummyObject2 = new GameObject("lol2");
    MyPlayer player;

    [SetUp]
    public void Setup()
    {

        players = new List<MyPlayer>();
        players.Add(new MyPlayer("player1",new List<Text>(), new List<Text>()));
        players.Add(new MyPlayer("player2", new List<Text>(), new List<Text>()));
        points = new List<int>();
        points.Add(100);
        points.Add(-100);
        cluePosition = (1, 1);

        p = new PointStep(players, points, cluePosition, false);
        ///////

        player = new MyPlayer("player3", new List<Text>(), new List<Text>());
        dummyObject1.AddComponent<MoneyButton>();
        dummyObject1.SetActive(false);
        c = new ClueStep(dummyObject1, dummyObject2, player);
        
    }

    [Test]
    public void TestGetOldClue()
    {
        Assert.AreEqual(p.getOldClue(), cluePosition);
    }

    [Test]
    public void TestBoardModePointStep()
    {
        Assert.False(p.BoardMode());
    }
    [Test]
    public void TestUndoPointStep()
    {
        p.Undo();
        Assert.AreEqual(-100, players[0].GetPoints());
        Assert.AreEqual(100, players[1].GetPoints());
        Assert.False(players[0].IsUp());
        Assert.False(players[0].IsUp());
    }

    [Test]
    public void TestGetOldPlayer()
    {
        AssetBundle.Equals(player, c.GetOldPlayer());
    }

    [Test]
    public void TestBoardModeClueStep()
    {
        Assert.True(c.BoardMode());
    }

    [Test]
    public void TestUndoClueStep()
    {
        c.Undo();

        Assert.True(dummyObject1.activeSelf);
        Assert.False(dummyObject2.activeSelf);
        Assert.True(player.IsUp());

    }


}

