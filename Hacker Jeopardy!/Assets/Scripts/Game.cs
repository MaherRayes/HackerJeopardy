using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game 
{
    private bool gameEditFlag = false;
    string gameName;
    List<Category> categories = new List<Category>();

    public List<(int, int)> dailyDoubles = new List<(int, int)>();

    Clue finalJeopardy ;
    string finalJeopardyCat;
    List<MyPlayer> players = new List<MyPlayer>();
    string description;
    bool doubleJeopardy;
    int id; 
    public Game(int id , string name, string description, List<Category> categories, Clue finalJeopardy, bool doubleJeopardy, List<(int,int)> dailyDoubles, string finalCategory)
    {
        this.id = id; 
        this.gameName = name;
        this.description = description;
        this.categories = categories;
        this.finalJeopardy = finalJeopardy;
        this.doubleJeopardy = doubleJeopardy;
        this.dailyDoubles = dailyDoubles;
        this.finalJeopardyCat = finalCategory;
    }

    public Game()
    {

    }

    public string GetGameName()
    {
        return this.gameName;
    }

    public void SetGameName(string str)
    {
        this.gameName = str; 
    }

    public int getCategoriesNum()
    {
        return categories.Count;
    }

    public Category GetCatAt(int index)
    {
        return categories[index];
    }

    public void AddCat(Category c)
    {
        categories.Add(c);
    }

    public int GetCatCount()
    {
        return categories.Count;
    }

    public bool IsDouble()
    {
        return doubleJeopardy;
    }

    public void RemoveCategory(int ID)
    {
        int index = -1;
        for (int i = 0; i < this.categories.Count; i++)
        {
            if (categories[i].getId() == ID)
                index = i;
        }

        if (index > -1)
            this.categories.RemoveAt(index);
     }

    public void RemoveDailyDoubles()
    {
        this.dailyDoubles.Clear();
    }

    public void SetFinalJeopardyClue(Clue clue)
    {
        this.finalJeopardy = clue;
    }

    public Clue GetFinalJeopardyClue()
    {
        return this.finalJeopardy;
    }

    public bool IsDailyDouble((int, int) tuple)
    {
        return this.dailyDoubles.Contains(tuple);
    }

    public void UnsetDailyDouble((int, int) tuple)
    {
        this.dailyDoubles.Remove(tuple);
    }

    public void SetDailyDouble((int, int) tuple)
    {
        this.dailyDoubles.Add(tuple);
    }

    //should be fixed, it was returning -1 all the time when in edit game mode! it's fixed now but we have to consider what to do if the corresponding id is not found rather than just returning -1!
    public int GetIndexFromId(int Id)           
    {
        foreach (Category cat in this.categories)
        {
            if (cat.getId() == Id)
            {
                return categories.IndexOf(cat);
            }
        }

        return -1;
    }

    public bool CategoryExist(int ID)
    {
        foreach (Category cat in this.categories)
        {
            if (cat.getId() == ID)
            {
                return true;
            }
        }
        return false;
    }

    public void AddPlayer(MyPlayer p)
    {
        players.Add(p);
    }

    public MyPlayer GetPlayer(int index)
    {
        return players[index];
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    public string GetDescription()
    {
        return this.description;
    }

    public List<(int,int)> GetDailyDoubles()
    {
        return this.dailyDoubles;
    }

    public List<Category> GetCategories()
    {
        return this.categories;
    } 

    public void RemovePlayer(MyPlayer p)
    {
        players.Remove(p);
        
    }

    public string GetFinalJeopardyCat()
    {
        return this.finalJeopardyCat;
    }
    public void SetFinalJeopardyCat(string s)
    {
        this.finalJeopardyCat = s;
    }

    public int GetID()
    {
        return this.id;
    }

    public void SetGameID(int id)
    {
        this.id = id;
    }

    public List<MyPlayer> GetPlayers()
    {
        return players;
    }

    public void SetDoubleJeopardy(bool i)
    {
        doubleJeopardy = i;
    }
    public void SetGameEditFlag(bool b)
    {
        this.gameEditFlag = b;
    }
    public bool GetGameEditFlag()
    {
        return this.gameEditFlag; 
    }
}
