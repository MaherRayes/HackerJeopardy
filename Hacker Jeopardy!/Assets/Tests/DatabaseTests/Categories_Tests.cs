using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class Categories_Tests : MonoBehaviour
{
    SQLiteManager sql = new SQLiteManager("URI=file:" + Application.dataPath + "/Tests/DatabaseTests/WriteClueDB.sqlite");
    [SetUp]
    public void Setup()
    {

        sql.removeAllCategories();
        sql.removeAllClues();


    }

    [Test]
    public void TestWriteAndGetCategory()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 5);
        clues.Add(1, c1);
        clues.Add(2, c2);
        clues.Add(3, c3);
        clues.Add(4, c4);
        clues.Add(5, c5);
        Category cat = new Category(2000, "TestUnit", "DD", clues);

        //write all clues and the category
        sql.WriteClue(sql.getHighestCategoriesID(), c1);
        sql.WriteClue(sql.getHighestCategoriesID(), c2);
        sql.WriteClue(sql.getHighestCategoriesID() , c3);
        sql.WriteClue(sql.getHighestCategoriesID() , c4);
        sql.WriteClue(sql.getHighestCategoriesID() , c5);
        sql.WriteCategorie(cat);

        //read the category
        Category lcat = sql.GetSpecificCategory(cat.getId());

        //compare the contents of the category with the ones we wrote
        Clue c = lcat.getClue(0);
        Assert.IsTrue(ClueEquals(c, c1));
        c = lcat.getClue(1);
        Assert.IsTrue(ClueEquals(c, c2));
        c = lcat.getClue(2);
        Assert.IsTrue(ClueEquals(c, c3));
        c = lcat.getClue(3);
        Assert.IsTrue(ClueEquals(c, c4));
        c = lcat.getClue(4);
        Assert.IsTrue(ClueEquals(c, c5));
        Assert.AreEqual(lcat.getName(), "TestUnit");
        Assert.AreEqual(lcat.getDescription(), "DD");
        Assert.AreEqual(lcat.getId(), cat.getId());

        //Assert.AreNotEqual(c.GetAnswer(), "worked");

    }

    [Test]
    public void TestReadCategories()
    {

        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 5);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat1 = new Category(2000, "TestUnit1", "DD", clues);

        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        sql.WriteCategorie(cat1);

        //add new category and write it
        clues.Remove(0);
        Clue c6 = new DualClue("test clue1", "test clue2", "DD", 1);
        clues.Add(0, c6);
        Category cat2 = new Category(3000, "TestUnit2", "DD", clues);
        sql.WriteClue(3000, c6);
        sql.WriteClue(3000, c2);
        sql.WriteClue(3000, c3);
        sql.WriteClue(3000, c4);
        sql.WriteClue(3000, c5);
        sql.WriteCategorie(cat2);

        //read the category
        Dictionary<int, Category> cats = sql.ReadCategories();

        //check if the categories read are the ones we wrote
        Assert.AreEqual(cats[0].getId(), cat1.getId());
        Assert.AreEqual(cats[1].getId(), cat2.getId());
        Assert.AreEqual(cats[0].getName(), "TestUnit1");
        Assert.AreEqual(cats[1].getName(), "TestUnit2");
        Assert.IsTrue(ClueEquals(cats[0].getClue(0), c1));
        Assert.IsTrue(ClueEquals(cats[1].getClue(0), c6));


    }

    [Test]
    public void TestReadCluesOfCategory()
    {

        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 5);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat1 = new Category(2000, "TestUnit1", "DD", clues);

        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        sql.WriteCategorie(cat1);

        //read the category
        Dictionary<int, Clue> loadedClues = sql.ReadCluesOfCategory(2000);

        //compare the contents of the category with the ones we wrote
        Clue c = loadedClues[0];
        Assert.IsTrue(ClueEquals(c, c1));
        c = loadedClues[1];
        Assert.IsTrue(ClueEquals(c, c2));
        c = loadedClues[2];
        Assert.IsTrue(ClueEquals(c, c3));
        c = loadedClues[3];
        Assert.IsTrue(ClueEquals(c, c4));
        c = loadedClues[4];
        Assert.IsTrue(ClueEquals(c, c5));

    }

    [Test]
    public void TestRemoveCategory()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 5);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat = new Category(2000, "TestUnit", "DD", clues);

        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        sql.WriteCategorie(cat);

        sql.removeSpecificCategory(2000);
        Assert.That(() => sql.GetSpecificCategory(2000), Throws.TypeOf<System.InvalidOperationException>());
    }

    [Test]
    public void TestRemoveAllCategories()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 5);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat = new Category(2000, "TestUnit", "DD", clues);

        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        sql.WriteCategorie(cat);


        //add new category and write it
        clues.Remove(0);
        Clue c6 = new DualClue("test clue1", "test clue2", "DD", 1);
        clues.Add(0, c6);
        Category cat2 = new Category(3000, "TestUnit2", "DD", clues);
        sql.WriteClue(3000, c6);
        sql.WriteClue(3000, c2);
        sql.WriteClue(3000, c3);
        sql.WriteClue(3000, c4);
        sql.WriteClue(3000, c5);
        sql.WriteCategorie(cat2);

        sql.removeAllCategories();
        Assert.That(() => sql.GetSpecificCategory(2000), Throws.TypeOf<System.InvalidOperationException>());
        Assert.That(() => sql.GetSpecificCategory(3000), Throws.TypeOf<System.InvalidOperationException>());
    }

    [Test]
    public void TestGetHighestCategoryID()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 5);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat = new Category(2000, "TestUnit", "DD", clues);

        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        sql.WriteCategorie(cat);

        //add new category and write it
        clues.Remove(0);
        Clue c6 = new DualClue("test clue1", "test clue2", "DD", 1);
        clues.Add(0, c6);
        Category cat2 = new Category(3000, "TestUnit2", "DD", clues);
        sql.WriteClue(3000, c6);
        sql.WriteClue(3000, c2);
        sql.WriteClue(3000, c3);
        sql.WriteClue(3000, c4);
        sql.WriteClue(3000, c5);
        sql.WriteCategorie(cat2);

        Assert.AreEqual(sql.getHighestCategoriesID(),2);
    }


    [Test]
    public void TestCategoryLessThanFiveClues()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        Category cat = new Category(2000, "TestUnit", "DD", clues);
        
        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        Assert.That(() => sql.WriteCategorie(cat), Throws.TypeOf<System.InvalidOperationException>()); 
    }

    [Test]
    public void TestWrongDifficulty()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 2);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 4);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat = new Category(2000, "TestUnit", "DD", clues);


        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        Assert.That(() => sql.WriteCategorie(cat), Throws.TypeOf<System.InvalidOperationException>());
    }

    [Test]
    public void TestOrderDifficulty()
    {
        //initialise a category
        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
        Clue c1 = new SingleClue("test clue", "works", 1);
        Clue c2 = new SingleClue("test clue", "works", 5);
        Clue c3 = new SingleClue("test clue", "works", 3);
        Clue c4 = new SingleClue("test clue", "works", 4);
        Clue c5 = new SingleClue("test clue", "works", 2);
        clues.Add(0, c1);
        clues.Add(1, c2);
        clues.Add(2, c3);
        clues.Add(3, c4);
        clues.Add(4, c5);
        Category cat = new Category(2000, "TestUnit", "DD", clues);


        //write all clues and the category
        sql.WriteClue(2000, c1);
        sql.WriteClue(2000, c2);
        sql.WriteClue(2000, c3);
        sql.WriteClue(2000, c4);
        sql.WriteClue(2000, c5);
        Assert.That(() => sql.WriteCategorie(cat), Throws.TypeOf<System.InvalidOperationException>());
    }

    private bool ClueEquals(Clue c1, Clue c2)
    {
        if (c1 is SingleClue)
        {
            if (c2 is SingleClue)
            {
                SingleClue clue1 = (SingleClue)c1;
                SingleClue clue2 = (SingleClue)c2;
                return (clue1.GetAnswer() == clue2.GetAnswer()) && (clue1.GetClue() == clue2.GetClue()) && (clue1.GetAward(false) == clue2.GetAward(false)) && (clue1.GetClueType() == clue2.GetClueType()) && (clue1.GetDifficulty() == clue2.GetDifficulty());
            }
            else
                return false;
        }
        else
        {
            if (c2 is DualClue)
            {
                DualClue clue1 = (DualClue)c1;
                DualClue clue2 = (DualClue)c2;
                return (clue1.GetAnswer() == clue2.GetAnswer()) && (clue1.GetClue1() == clue2.GetClue1()) && (clue1.GetClue2() == clue2.GetClue2()) && (clue1.GetAward(false) == clue2.GetAward(false)) && (clue1.GetType1() == clue2.GetType1()) && (clue1.GetType2() == clue2.GetType2()) && (clue1.GetDifficulty() == clue2.GetDifficulty());
            }
            else
                return false;
        }


    }

}
