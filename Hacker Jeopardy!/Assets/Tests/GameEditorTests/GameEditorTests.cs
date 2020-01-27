using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.UI;
using TMPro;

public class GameEditorTests : MonoBehaviour
{
    /*
        Some Info About this Test : 
        Befor running this test you have to comment the start method in the GameEditor Class

     *   this DataBase Contains 2 Games and 6 Categories
     *   Becarful when you using SaveGame or SaveCategory methods , this will make changes on the DataBase that we use to test our GameEditor
     *   the first Game with the name (as) contains 3 Categories and tow dailyDoubles
     *   the second Game with the name (k) Contains 2 Categories and Two DailyDoubles
     */


    GameEditor editor;

    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Canvas"));
        editor = gameGameObject.GetComponent<GameEditor>();

        editor.ConnectToSql("URI=file:" + Application.dataPath + "/Tests/GameEditorTests/GameEditorDBTest.sqlite");
        editor.LoadData();
    }

    [Test]
    public void TestAddedCatsNumber()
    {
        Debug.Log(editor.categories.Count);

        Assert.AreEqual(6, editor.categories.Count);
        Assert.AreEqual(6, editor.CategoriesInDataBase.Count);

    }

    [Test]
    public void SelectTwoCategory()
    {

        editor.InitGame();
        int i = 2;
        foreach (GameObject cat in editor.CategoriesInDataBase.Values)
        {
            if (i > 0)
            {
                Toggle toggle = cat.GetComponentInChildren<Toggle>();
                toggle.isOn = true;
                i--;
            }

        }

        Assert.AreEqual(2, editor.selectedcategoryObjects.Count);

    }
    [Test]
    public void UnSelectTwoCategory()
    {
        editor.InitGame();
        int i = 2;

        foreach (GameObject cat in editor.CategoriesInDataBase.Values)
        {
            Toggle toggle = cat.GetComponentInChildren<Toggle>();
            toggle.isOn = true;
        }
        Assert.AreEqual(6, editor.selectedcategoryObjects.Count);

        foreach (GameObject cat in editor.CategoriesInDataBase.Values)
        {
            if (i > 0)
            {
                Toggle toggle = cat.GetComponentInChildren<Toggle>();
                toggle.isOn = false;
                i--;
            }

        }
        Assert.AreEqual(4, editor.selectedcategoryObjects.Count);

    }

    [Test]
    public void CreatFiveClues()
    {
        editor.InitGame();
        editor.InitCategory();
        editor.categoryName.text = "A";
        editor.description.text = "A";
        for (int i = 0; i < 5; i++)
        {
            editor.setCurrentClueIndex(i);
            editor.clueText1.text = (i + 1).ToString();
            editor.answerText.text = (i + 1).ToString();
            editor.CreateClue();
        }
        Assert.AreEqual(5, editor.currentCategory.getClues().Count);

    }
    [Test]
    public void LoadClue()
    {
        editor.InitGame();
        editor.InitCategory();
        editor.setCurrentClueIndex(1);
        editor.clueText1.text = "1";
        editor.answerText.text = "1";
        editor.CreateClue();
        editor.setCurrentClueIndex(0);
        editor.LoadClue();
        Assert.AreEqual("" + "", editor.clueText1.text + editor.answerText.text);
        editor.setCurrentClueIndex(1);
        editor.LoadClue();
        Assert.AreEqual("1" + "1", editor.clueText1.text + editor.answerText.text);


    }

    [Test]
    public void CheckClueType()
    {
        editor.InitCategory();
        editor.setCurrentClueIndex(0);
        editor.LoadClue();
        editor.clueText1.text = "A";
        editor.clueText2.text = "B";
        editor.answerText.text = "Dual";
        editor.CreateClue();
        editor.setCurrentClueIndex(1);
        editor.LoadClue();
        editor.clueText1.text = "";
        editor.answerText.text = "Single";
        editor.CreateClue();
        Assert.IsFalse(string.IsNullOrWhiteSpace(editor.warningTextClue.text));
        editor.clueText1.text = "A";
        editor.CreateClue();
        Clue c = editor.currentCategory.getClues()[0];
        Clue c1 = editor.currentCategory.getClues()[1];
        Assert.True(c.IsDual());
        Assert.False(c1.IsDual());
        DualClue dual = (DualClue)c;
        SingleClue single = (SingleClue)c1;
        Assert.AreEqual(dual.GetType1(), Clue_Type.TEXT);
        Assert.AreEqual(dual.GetType2(), Clue_Type.TEXT);
        Assert.AreEqual(single.GetClueType(), Clue_Type.TEXT);


    }


    [Test]
    public void AddEditAndDeleteGame()
    {
        ///////////// Add New Game ////////////////////
        editor.InitGame();
        editor.gameName.text = "NewGame";
        editor.SetFinalJeopardyFlag(true);
        editor.LoadClue();
        editor.ShowHideFJCatInputField();
        editor.finalJeopardyCat.GetComponentInChildren<TMP_InputField>().text = "";
        editor.clueText1.text = "FinalClue";
        editor.answerText.text = "FinalAnswer";
        editor.CreateClue();
        Assert.IsFalse (string.IsNullOrWhiteSpace(editor.warningTextClue.text));
        editor.finalJeopardyCat.GetComponentInChildren<TMP_InputField>().text = "Final";
        editor.CreateClue();
        int i = 2;
        foreach (GameObject cat in editor.CategoriesInDataBase.Values)
        {
            if (i > 0)
            {
                Toggle toggle = cat.GetComponentInChildren<Toggle>();
                toggle.isOn = true;
                i--;
            }

        }

        Assert.AreEqual(2, editor.selectedcategoryObjects.Count);
        editor.SaveGame();
        Assert.AreEqual(3, editor.GetGames().Count);
        Assert.AreEqual("NewGame", editor.GetGames()[2].GetGameName());
        Assert.AreEqual(2, editor.GetGames()[2].getCategoriesNum());
        SingleClue final = (SingleClue)editor.GetGames()[2].GetFinalJeopardyClue();
        Assert.AreEqual("FinalClue" + "FinalAnswer", final.GetClue() + final.GetAnswer());
        Assert.AreEqual(4, editor.dropdown.options.Count);
        Assert.AreEqual(4, editor.chooseGameDropDown.options.Count);

        ///////////////////////////////Edit the New Game ///////////////////////
        editor.dropdown.value = 3;
        editor.LoadGame();
        editor.SetGameEditFlag();
        editor.DisableCatsButtons(false);
        Assert.AreEqual(2, editor.GetGames()[2].getCategoriesNum());
        editor.gameName.text = "ba";
        editor.SaveGame();
        Assert.AreEqual(3, editor.GetGames().Count);
        editor.dropdown.value = 3;
        editor.SetGameEditFlag();
        editor.LoadGame();
        Assert.AreEqual("ba", editor.GetGames()[2].GetGameName());
       
        ////////////////Delete the New Game //////////////
        ///
        editor.dropdown.value = 3;
        editor.DeleteGame();
        Assert.AreEqual(2, editor.GetGames().Count);
        Assert.AreEqual(3, editor.dropdown.options.Count);
        Assert.AreEqual(3, editor.chooseGameDropDown.options.Count);
    }

    [Test]
    public void AddEditAndDeleteCategory()
    {
        ////////////// Add New Cat /////////////
        editor.InitGame();
        editor.InitCategory();
        editor.categoryName.text = "";
        editor.description.text = "NewDes";
        for (int i = 0; i < 5; i++)
        {
            editor.setCurrentClueIndex(i);
            editor.clueText1.text = (i + 1).ToString();
            editor.answerText.text = (i + 1).ToString();
            editor.CreateClue();
        }
        Assert.AreEqual(5, editor.currentCategory.getClues().Count);
        editor.SaveCategory();
        Assert.IsFalse(string.IsNullOrWhiteSpace(editor.warningTextCategory.text));
        editor.categoryName.text = "NewCat";
        editor.SaveCategory();
        int id = editor.currentCategory.getId();
        Assert.AreEqual(7, editor.categories.Count);
        Assert.AreEqual(7, editor.CategoriesInDataBase.Count);

        ////////////// Edit the New Cat /////////////

        Category newCat = editor.categories[id];          // The Category with ID = 2 in this DataBase called CSS
        GameObject newCatObject = editor.CategoriesInDataBase[id];
        Button[] buttons = newCatObject.GetComponentsInChildren<Button>();
        Button edit = buttons[0];       // Edit Button
        edit.onClick.Invoke();
        editor.description.text = "modified";
        editor.SaveCategory();
        Assert.AreEqual("modified", newCat.getDescription());
        Assert.AreEqual(7, editor.categories.Count);
        Assert.AreEqual(7, editor.CategoriesInDataBase.Count);

        ////////////// Delete the New Cat /////////////
        editor.DeleteCategory(id);
        Assert.AreEqual(6, editor.CategoriesInDataBase.Count);
        Assert.AreEqual(6, editor.categories.Count);

    }
}
