using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Category 
{

    int id;
    string name;        
    string description;
    Dictionary<int, Clue> clues = new Dictionary<int, Clue>();
    private bool catEditFlag = false ; 

    public Category(int id, string name, string description, Dictionary<int, Clue> clues)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.clues = clues;
    }

    public Category()
    {
    }

    public int getId()
    {
        return this.id; 
    }
    public void setId(int id)
    {
        this.id = id; 
    }
    public string getName()
    {
        return this.name; 
    }
    public void setName(string name)
    {
        this.name = name; 
    }

    public string getDescription()
    {
        return this.description; 
    }
    public void setDescription(string description)
    {
        this.description = description; 
    }
    public Dictionary<int,Clue> getClues()
    {
        return this.clues; 
    }
    public void SetClues(Dictionary<int,Clue> clues)
    {
        this.clues = clues; 
    }
    ////////
    
    public Clue getClue(int i)
    {
        return clues[i];
    }
    
    public void addClue(int index, Clue c)
    {
        clues.Add(index, c);
    }
    public void SetCatEditFlag(bool b)
    {
        this.catEditFlag = b; 
    }
    public bool GetCatEditFlag()
    {
        return this.catEditFlag;
    }
}
