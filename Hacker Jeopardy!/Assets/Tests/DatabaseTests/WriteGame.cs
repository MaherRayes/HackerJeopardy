using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WriteGame {
    SQLiteManager sql;

    //single clue
    Clue textClue;
    Clue textClue2;
    Clue vidClue;
    Clue imgClue;
    Clue audioClue;
    Clue finalClue;

    //cats
    Category cat1;
    Category cat2;
    Category cat3;
    Category cat4;
    Category cat5;
    Category cat6;

    //clues
    Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
    List<Category> categories = new List<Category>();

    [SetUp]
    public void Setup()
    {
        sql = new SQLiteManager("URI=file:" + Application.dataPath + "/Tests/DatabaseTests/WriteClueDB.sqlite");
        clues = new Dictionary<int, Clue>();
        categories = new List<Category>();

        sql.removeAllClues();
        sql.removeAllCategories();

        textClue = new SingleClue("clue 1", "answer clue text", 1);
        textClue2 = new SingleClue("Clue 2", "answer clue text 2", 2);
        vidClue = new SingleClue("clue 2.mp4", "answer clue vid", 3);
        imgClue = new SingleClue("clue 3.jpg", "answer clue img", 4);
        audioClue = new SingleClue("clue 4.mp3", "answer clue audio", 5);
        finalClue = new SingleClue("final Clue", "answer", 0);
        
        //add clues to DB
        for (int i = 0; i<7; i++)
        {
            sql.WriteClue(i, textClue);
            sql.WriteClue(i, textClue2); 
            sql.WriteClue(i, vidClue);
            sql.WriteClue(i, imgClue);
            sql.WriteClue(i, audioClue);
        }
        sql.WriteClue(-1, finalClue);
        
        //add clues to Dict
        clues.Add(0, textClue);
        clues.Add(1, textClue);
        clues.Add(2, textClue);
        clues.Add(3, textClue);
        clues.Add(4, textClue);

        cat1 = new Category(0, "first cat", "first desc", clues);
        cat2 = new Category(1, "second cat", "second desc", clues);
        cat3 = new Category(2, "third cat", "third desc", clues);
        cat4 = new Category(3, "fourth cat", "fourth desc", clues);
        cat5 = new Category(4, "fifth cat", "fifth desc", clues);
        cat6 = new Category(5, "sixth cat", "sixth desc", clues);

        sql.WriteCategorie(cat1);
        categories.Add(cat1);
        sql.WriteCategorie(cat2);
        categories.Add(cat2);
        sql.WriteCategorie(cat3);
        categories.Add(cat3);
        sql.WriteCategorie(cat4);
        categories.Add(cat4);
        sql.WriteCategorie(cat5);
        categories.Add(cat5);
        sql.WriteCategorie(cat6);
        categories.Add(cat6);

        sql.removeAllGames();
    
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
            if (clue2 is DualClue)
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

    [Test]
    public void TestGameAttributes()
    {
        List<(int, int)> dailyDouble = new List<(int, int)>();
        dailyDouble.Add((0, 0));
        Game game = new Game(0, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");
        sql.WriteGame(game);
        List<Game> readGames = sql.ReadGamesAsObjects();

        Assert.AreEqual(readGames[0].getCategoriesNum(), 6);
        Assert.AreEqual(sql.ReadDailyDoublesOfGame("game 1")[0], (0, 0));
        Assert.AreEqual(sql.ReadGames()[0], "game 1");
        Assert.AreEqual(readGames[0].IsDouble(),false);
        Assert.AreEqual(readGames[0].GetPlayerCount(), 0);
        Assert.IsTrue(AreEqual(readGames[0].GetFinalJeopardyClue(), finalClue));
    }

    //check if clues and cats remain after remove
    [Test]
    public void TestCatsAndCluesAfterRemove()
    {
        List<(int, int)> dailyDouble = new List<(int, int)>();
        dailyDouble.Add((0, 0));
        Game game = new Game(0, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");
        sql.WriteGame(game);
        List<Game> oldReadGames = sql.ReadGamesAsObjects();
        Assert.AreEqual(oldReadGames.Count, 1);
        int cats = sql.ReadCategories().Count;
        int clues = 0;

        for(int i = 0; i<7; i++)
        {
            clues += sql.ReadCluesOfCategory(i).Count;
        }

        sql.removeSpecificGame(game.GetID());
        List<Game> readGames = sql.ReadGamesAsObjects();
        Assert.AreEqual(readGames.Count, 0);
        int newCats = sql.ReadCategories().Count;
        int newClues = 0;

        for (int i = 0; i < 7; i++)
        {
            newClues += sql.ReadCluesOfCategory(i).Count;
        }

        Assert.AreEqual(newCats, cats);
        Assert.AreEqual(newClues, clues);
    }

    //tests if the sql rejects to save games with abnormal number of categories
    [Test]
    public void TestWrongNumberOfCats()
    {
        List<(int, int)> dailyDouble = new List<(int, int)>();
        dailyDouble.Add((0, 0));
        categories.Add(cat1);
        Game game = new Game(0, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");

        Assert.That(() => sql.WriteGame(game), Throws.TypeOf<AssertionException>());

        categories.RemoveAt(6);
        categories.RemoveAt(5);

        Assert.That(() => sql.WriteGame(game), Throws.TypeOf<AssertionException>());
    }

    [Test]
    public void TestTwoGameSameName()
    {
        List<(int, int)> dailyDouble = new List<(int, int)>();
        dailyDouble.Add((0, 0));
        categories.Add(cat1);
        Game game1 = new Game(0, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");
        sql.WriteGame(game1);
        Game game2 = new Game(1, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");
        Assert.That(() => sql.WriteGame(game2), Throws.TypeOf<Mono.Data.Sqlite.SqliteException>());
    }

    [Test]
    public void TestReadGamesNames()
    {
        List<(int, int)> dailyDouble = new List<(int, int)>();
        dailyDouble.Add((0, 0));
        categories.Add(cat1);
        Game game1 = new Game(0, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");
        sql.WriteGame(game1);
        Game game2 = new Game(1, "game 2", "W/e", categories, finalClue, false, dailyDouble, "final category");
        sql.WriteGame(game2);
        List<string> names = sql.ReadGames();

        Assert.AreEqual(names[0], "game 1");
        Assert.AreEqual(names[1], "game 2");
    
    }


       [Test]
    public void TestRemoveCategoryWithGame()
    {
        List<(int, int)> dailyDouble = new List<(int, int)>();
        dailyDouble.Add((0, 0));
        Game game1 = new Game(0, "game 1", "W/e", categories, finalClue, false, dailyDouble, "final category");
        sql.WriteGame(game1);
        sql.removeSpecificCategory(1);
        //games should be removed
        Assert.AreEqual(sql.ReadGames().Count, 0);
    }

}
