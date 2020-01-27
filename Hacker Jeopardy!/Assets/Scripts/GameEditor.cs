using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEditor : MonoBehaviour
{
    public GameObject gameCreatorPanel;
    public GameObject creatCategoryPanel;
    public GameObject CluePanel; 
    public GameObject categoryPrefab;
    public GameObject selectedCategoryPrefab;
    public GameObject categoryContainer;
    public GameObject selectedCategoryContainer;
    public TMP_InputField answerText;
    public TMP_InputField clueText1;
    public TMP_InputField clueText2;
    public TMP_InputField gameName; 
    public TMP_InputField categoryName;
    public TMP_InputField description;
    public TMP_Text warningTextClue;
    public TMP_Text warningTextCategory;
    public TMP_Text WarningTextGame;
    public int currentClueIndex;
    public Category currentCategory; 
    public Dictionary<int, GameObject> selectedcategoryObjects = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> CategoriesInDataBase = new Dictionary<int, GameObject>();
    public Dictionary<int, Category> categories = new Dictionary<int, Category>();
    public TMP_Text d100, d200, d300, d400, d500,dfinal;
    SQLiteManager sql;
    Game currentGame;
    public TMP_Dropdown dropdown;
    public TMP_Dropdown chooseGameDropDown;
    List<Game> games = new List<Game>();
    private bool finalJeopardyFlag = false;
    public GameObject finalJeopardyCat;
    private List<int> templateCats = new List<int>();
    public Button creatCatButton;
    private void Start()
    {
        ConnectToSql("URI=file:" + Application.dataPath + "/HackerDB.sqlite");              // If You Want to Run Tests , you have to comment the start methode 
        LoadData();
    }

    public void ConnectToSql(string sqlConnection)
    {
        sql = new SQLiteManager(sqlConnection);
    }
    public void LoadData()
    {
        sql.CreateSQLiteTablesIfNotExists();
        this.LoadCategoriesFromDataBank();
        games = sql.ReadGamesAsObjects();
        LoadGamesNames();

    }
    public void UpdateGames()
    {
        this.games = sql.ReadGamesAsObjects();
        LoadGamesNames();
    }

    public void CreateClue ()
    {
      
        if (CheckClue())
        {

            Clue clue;
           
            if (IsDualClue())
            {
                string str1 = clueText1.text;
                string str2 = clueText2.text;
                clue = new DualClue(str1, str2, answerText.text, currentClueIndex+1);
            }
            else
            {
                string str1 = clueText1.text;
                clue = new SingleClue(str1, answerText.text, currentClueIndex+1);
            }
            if (!this.finalJeopardyFlag)
            {

                if (currentCategory.getClues().ContainsKey(currentClueIndex))            //why using getClues!!!
                    currentCategory.getClues().Remove(currentClueIndex);                 // 
                currentCategory.getClues().Add(currentClueIndex, clue);                  //
                SetColorToFilledClue();
            }
            else
            {
                currentGame.SetFinalJeopardyClue(clue);
                currentGame.SetFinalJeopardyCat(this.finalJeopardyCat.GetComponentInChildren<TMP_InputField>().text);
                dfinal.color = new Color(0, 1, 0, 1);
                this.finalJeopardyCat.SetActive(false);
            }
        
            CluePanel.SetActive(false);
        }
    }

    public void LoadClue()
    {
        ClearClue();
        Clue clue;

        if (finalJeopardyFlag)
        {
            if (currentGame.GetFinalJeopardyClue() == null)
            {
                return;
            }
            else
            {
                clue = currentGame.GetFinalJeopardyClue();
                finalJeopardyCat.GetComponentInChildren<TMP_InputField>().text = currentGame.GetFinalJeopardyCat();

            }
        }else
        {
            if (!currentCategory.getClues().TryGetValue(currentClueIndex, out clue)) return;
        }

        this.answerText.text = clue.GetAnswer();

        if (clue.IsDual())
        {
            DualClue dualclue = (DualClue)clue;
            this.answerText.text = dualclue.GetAnswer();

            clueText1.text  = dualclue.GetClue1();
            clueText2.text = dualclue.GetClue2();
        }
        else
        {
            SingleClue singleclue = (SingleClue)clue;
            clueText1.text = singleclue.GetClue();
        }   
    }

    public void LoadCategoriesFromDataBank()
    {
        ClearCatPrefabs();
        Dictionary<int, Category> cats = sql.ReadCategories();
        foreach (Category cat in cats.Values)
        {
            GameObject go = Instantiate(categoryPrefab, categoryContainer.transform) as GameObject; // display Cats in database; 
            go.transform.parent = categoryContainer.transform;

            Toggle toggle = go.GetComponentInChildren<Toggle>();

            TMP_Text text = toggle.GetComponentInChildren<TMP_Text>();        // Set Categorys Name in the created Prefab
            text.text = cat.getName();

            toggle.isOn = false;

            toggle.onValueChanged.AddListener(delegate { ShowSelectedCategory(cat.getId(), toggle); });
            Button[] buttons = go.GetComponentsInChildren<Button>();
            Button edit = buttons[0];       // Edit Button
            Button remove = buttons[1];
            edit.onClick.AddListener(delegate { EditCategory(cat.getId()); });
            remove.onClick.AddListener(delegate { DeleteCategory(cat.getId()); });
            categories.Add(cat.getId(), cat);
            CategoriesInDataBase.Add(cat.getId(), go);
        }
    }

    public void SaveCategory()
    {
        currentCategory.setName(this.categoryName.text);
        currentCategory.setDescription(this.description.text);
        {
            if (CheckCategory())
            {
                sql.WriteCategorie(currentCategory);
                int id = this.currentCategory.getId();     // save id locally  (call by value)
                this.LoadCategoriesFromDataBank();
                ReSelectCategories();
                this.creatCategoryPanel.SetActive(false);
                UpdateGames();
            }
        }
    }

    private void ClearCatPrefabs()
    {
        this.categories.Clear();

        foreach (GameObject go in this.CategoriesInDataBase.Values)
        {
            Destroy(go);
        }
        this.CategoriesInDataBase.Clear();

    }

    private void ReSelectCategories()
    {
        List<int> selectedkeys = new List<int>(this.templateCats);

        foreach (GameObject go in selectedcategoryObjects.Values)
        {
            Destroy(go);
        }
        selectedcategoryObjects.Clear();
        this.templateCats.Clear();
        foreach (int i in selectedkeys)
        {
            CategoriesInDataBase[i].GetComponentInChildren<Toggle>().isOn = false;
            CategoriesInDataBase[i].GetComponentInChildren<Toggle>().isOn=true;
        }
    }

    public void ShowSelectedCategory(int id , Toggle toggle)

    {
        int selectedCats = this.selectedcategoryObjects.Count;
       
        if (toggle.isOn && selectedCats<6)       
        {
            ++selectedCats;
            
            GameObject go = Instantiate(selectedCategoryPrefab, selectedCategoryContainer.transform) as GameObject;
            go.transform.parent = selectedCategoryContainer.transform;
            TMP_Text t = go.GetComponentInChildren<TMP_Text>();
            t.text = this.categories[id].getName();
            this.selectedcategoryObjects.Add(id, go);
            this.templateCats.Add(id);
            this.ColorDailyDoublesRelatedButtons(id, go);
        }
        else
        {
            if (!toggle.isOn && selectedCats > 0 && selectedCats<=6)        
            {
                --selectedCats;
                this.UnSelectCategory(id);
            }
            else
            {
                toggle.isOn = false; 
            }
        }
        
    }

    public void ColorDailyDoublesRelatedButtons(int catId, GameObject go )
    {
        int i = 0;
        foreach (Button b in go.GetComponentsInChildren<Button>())
        {
            int clueIndex = i;
            ShowCluesOnMouseOver showClue = b.GetComponent<ShowCluesOnMouseOver>();
            showClue.SetUpButton(templateCats.IndexOf(catId), clueIndex, this.templateCats, this.categories);
            b.onClick.AddListener(delegate { SetDailyDouble(catId, clueIndex, b); });
            (int, int) tuple = (this.currentGame.GetIndexFromId(catId), clueIndex);
                if (currentGame.IsDailyDouble(tuple))
                {
                    b.image.color = new Color(0, 1, 0, 1);
                }          
            i++;
        }
    }

    public void UnSelectCategory(int id)
    {
        GameObject gameOb;
        this.selectedcategoryObjects.TryGetValue(id, out gameOb);
        if (gameOb != null)
        {
            Destroy(gameOb);
            this.selectedcategoryObjects.Remove(id);
            this.templateCats.Remove(id);
            ReSelectCategories();
            currentGame.RemoveDailyDoubles();
            foreach (GameObject go in this.selectedcategoryObjects.Values)
            {
                foreach (Button b in go.GetComponentsInChildren<Button>())
                {
                    b.image.color = new Color(1, 1, 1, 1);
                }
            }
        }
    }

    public void EditCategory(int id)
    {
        AllClueValuesColorGreen();
        categories.TryGetValue(id, out this.currentCategory);
        this.warningTextCategory.text = "";
        this.categoryName.text = currentCategory.getName();
        this.description.text = currentCategory.getDescription();
        currentCategory.SetCatEditFlag(true);
        creatCategoryPanel.SetActive(true);
    }

    public void SaveGame()
    {
        currentGame.SetGameName(this.gameName.text);
        if (CheckGame())
        {
            currentGame.GetCategories().Clear();
            foreach(int i in templateCats)
            {
                currentGame.AddCat(this.categories[i]);
            }
            if (currentGame.GetGameEditFlag())
            {
                this.sql.removeGameRelatedFinalJeopardy(currentGame.GetID());
                this.sql.WriteClue(-1, currentGame.GetFinalJeopardyClue());
                this.sql.WriteGame(this.currentGame);
                gameCreatorPanel.SetActive(false);
            }
            else
            {
                this.sql.WriteClue(-1, currentGame.GetFinalJeopardyClue());
                this.sql.WriteGame(this.currentGame);
                gameCreatorPanel.SetActive(false);
            }
            currentGame.SetGameEditFlag(false);
        }
        UpdateGames();
    }

    public void LoadGame()
    {
        InitGame();
        if (dropdown.value > 0)
        { 
            int index = dropdown.value - 1;
            this.currentGame = games[index];
            this.gameName.text = currentGame.GetGameName();
            dfinal.color = new Color(0, 1, 0, 1);
            foreach (Category c in currentGame.GetCategories())
            {
                GameObject obj = CategoriesInDataBase[c.getId()];
                Toggle toggle = obj.GetComponentInChildren<Toggle>();
                toggle.isOn = true;             
            }
            gameCreatorPanel.SetActive(true);
        }
      
    }

    public void DisableCatsButtons(bool b)
    {
        foreach(GameObject go in CategoriesInDataBase.Values)
        {
            Toggle toggle = go.GetComponentInChildren<Toggle>();
            Button[] buttons = go.GetComponentsInChildren<Button>();
            buttons[0].interactable =b;
            buttons[1].interactable =b;
        }
        creatCatButton.interactable = b; 
    }

    public void InitCategory()
    {
        currentCategory= new Category();
        this.categoryName.text = "";
        this.description.text = "";
        this.warningTextCategory.text = "";
        AllClueValuesColorWhite();

    }

    public void InitGame()
    {
        currentGame = new Game();
        dfinal.color = new Color(1, 1, 1, 1);
        ClearGame();
    }

    private void ClearGame()
    {
        this.gameName.text = "";
        this.WarningTextGame.text = "";

        foreach (GameObject go in CategoriesInDataBase.Values)
        {
            go.GetComponentInChildren<Toggle>().isOn = false;
        }
        ClearClue();
        selectedcategoryObjects.Clear();

    }

    public void setCurrentClueIndex(int i)
    {
        this.currentClueIndex = i;
    }

    private bool IsDualClue()
    {
        if ((!string.IsNullOrWhiteSpace(this.clueText1.text)) && (!string.IsNullOrWhiteSpace(this.clueText2.text) )) return true;
        else return false; 
    }

    private void ClearClue ()
    {
        this.answerText.text = "";
        this.clueText1.text = "";
        this.clueText2.text = "";
        this.warningTextClue.text = "";
        this.finalJeopardyCat.GetComponentInChildren<TMP_InputField>().text = "";   

    }

    private void SetDailyDouble(int catId, int clueIndex,Button b)
    {
        (int, int) tuple = (templateCats.IndexOf(catId), clueIndex);

        if (currentGame.IsDailyDouble(tuple))
        {        
            b.image.color = new Color(1, 1, 1, 1);
            currentGame.UnsetDailyDouble(tuple);
        }
        else
        {          
            b.image.color = new Color(0, 1, 0, 1);
            currentGame.SetDailyDouble(tuple);
        }
        
    }

    private void LoadGamesNames()
    {
        this.dropdown.options.Clear();
        this.chooseGameDropDown.options.Clear();
        TMP_Dropdown.OptionData item = new TMP_Dropdown.OptionData("");
        this.dropdown.options.Add(item);
        this.chooseGameDropDown.options.Add(item);
        foreach (Game g in games)
        {
            TMP_Dropdown.OptionData name = new TMP_Dropdown.OptionData(g.GetGameName());
            this.dropdown.options.Add(name);
            this.chooseGameDropDown.options.Add(name);

        }
        this.dropdown.value = 0;
        this.chooseGameDropDown.value = 0;          
        this.dropdown.Select();
        this.chooseGameDropDown.Select();      // To refresh the dropdown values We set the value on 0 then execute Select - its better than the method dropdown.RefreshValue()
    }

    public void DeleteGame()
    {
        int index;

        if (dropdown.value > 0) {
            index = dropdown.value - 1;
            sql.removeSpecificGame(games[index].GetID());
            games.RemoveAt(index);
        }

        this.UpdateGames();
    }

    public void DeleteCategory(int id)
    {
        GameObject go = this.CategoriesInDataBase[id];
        Toggle toggle = go.GetComponentInChildren<Toggle>();
        toggle.isOn = false;
        sql.removeSpecificCategory(id);
        this.LoadCategoriesFromDataBank();
        this.ReSelectCategories();
        this.UpdateGames();                                                   
    }

    public void SetFinalJeopardyFlag(bool b)
    {
        this.finalJeopardyFlag = b;
    }

    private bool CheckClue()
    {
        if (finalJeopardyFlag)
        {
            if (string.IsNullOrWhiteSpace(this.finalJeopardyCat.GetComponentInChildren<TMP_InputField>().text))
            {
                this.warningTextClue.text = "Fill the Final category field please";
                return false; 
            }
        }

        if (string.IsNullOrWhiteSpace(this.clueText1.text))
        {
            warningTextClue.text = "Fill the first clue by adding a text or uploading a mediafile please ";
            return false;
        }
        if (string.IsNullOrWhiteSpace(this.answerText.text))
        {
            this.warningTextClue.text = "Enter an answer please";
            return false;
        }
        return true;
    }

    private bool CheckCategory()
    {
        if (string.IsNullOrWhiteSpace(this.categoryName.text))
        {
            warningTextCategory.text = "Fill the category name field please";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(this.description.text))
        {
            warningTextCategory.text = "Fill the description field please";
            return false;
        }
        else if (this.currentCategory.getClues().Count < 5)
        {
            warningTextCategory.text = "Fill all 5 clues please";
            return false;
        }
        else return true;
    }

    private bool CheckGame()
    {
        if (this.selectedcategoryObjects.Count < 2 || this.selectedcategoryObjects.Count > 6)
        {
            this.WarningTextGame.text = "Select between 2 to 6 categories please"; return false;
        }
        if (string.IsNullOrWhiteSpace(currentGame.GetGameName()))
        {
            this.WarningTextGame.text = "Enter the Game Name Please";
            return false;
        }
        else
        {
            if (currentGame.GetFinalJeopardyClue() == null)
            {
                this.WarningTextGame.text = "Create a finaljeopardy clue please"; return false;
            }
            else return true;

        }
    }

    public List<Game> GetGames()
    {
        return this.games; 
    }

    public void ShowHideFJCatInputField()
    {
        this.finalJeopardyCat.SetActive(finalJeopardyFlag);
    }

    private void SetColorToFilledClue()
    {
        if (currentClueIndex == 0) d100.color = new Color(0, 1, 0, 1);
        if (currentClueIndex == 1) d200.color = new Color(0, 1, 0, 1);
        if (currentClueIndex == 2) d300.color = new Color(0, 1, 0, 1);
        if (currentClueIndex == 3) d400.color = new Color(0, 1, 0, 1);
        if (currentClueIndex == 4) d500.color = new Color(0, 1, 0, 1);
    }

    private void AllClueValuesColorWhite()
    {
        d100.color = new Color(1, 1, 1, 1);
        d200.color = new Color(1, 1, 1, 1);
        d300.color = new Color(1, 1, 1, 1);
        d400.color = new Color(1, 1, 1, 1);
        d500.color = new Color(1, 1, 1, 1);
    }

    private void AllClueValuesColorGreen()
    {
        d100.color = new Color(0, 1, 0, 1);
        d200.color = new Color(0, 1, 0, 1);
        d300.color = new Color(0, 1, 0, 1);
        d400.color = new Color(0, 1, 0, 1);
        d500.color = new Color(0, 1, 0, 1);
    }

    public void SetGameEditFlag()
    {
        this.currentGame.SetGameEditFlag(true);
    }
}

