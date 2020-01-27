using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clue
{
    string answer;
    int difficulty;
    int id;

    protected Clue(string answer, int difficulty)
    {
        this.answer = answer;
        this.difficulty = difficulty;
    }

    protected Clue()
    {
    }

    public abstract bool IsDual();


    public int GetAward(bool doubleJeo) {
        if (doubleJeo)
            return 200 * difficulty;
        else
            return 100 * difficulty;
    }

    public string GetAnswer()
    {
        return this.answer;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public int GetID()
    {
        return id;
    }

    public void SetID(int id)
    {
        this.id = id;
    }
}
