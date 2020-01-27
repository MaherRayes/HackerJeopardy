using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer: IComparable<MyPlayer>
{
    public List<Text> nameObjects;
    private int points = 0;
    public List<Text> pointsObjects;
    private bool turn = false;
    public string name;


    public MyPlayer(string name, List<Text> nameObjects, List<Text> pointsObjects)
    {
        this.nameObjects = nameObjects;
        this.pointsObjects = pointsObjects;
        this.name = name;

        for (int i = 0; i < nameObjects.Count; i++)
        {
            nameObjects[i].color = Color.white;
            nameObjects[i].text = this.name;
            pointsObjects[i].text = "$" + points.ToString();
        }
    }

    public bool IsUp()
    {
        return turn;
    }

    public void AddPoints(int point)
    {
        this.points += point;
        foreach(Text t in pointsObjects)
            t.text = "$" + points.ToString();
    }

    public void SetTurn(bool turn)
    {
        this.turn = turn;

        if (turn)
        {
            for (int i = 0; i < nameObjects.Count; i++)
            {
                nameObjects[i].color = Color.green;
            }
        }
        else
        {
            for (int i = 0; i < nameObjects.Count; i++)
            {
                nameObjects[i].color = Color.white;
            }
        }
    }

    public void banPlayer()
    {
        for (int i = 0; i < nameObjects.Count; i++)
        {
            nameObjects[i].color = Color.red;
        }
    }

    public void unbanPlayer()
    {
        for (int i = 0; i < nameObjects.Count; i++)
        {
            nameObjects[i].color = Color.white;
        }
    }

    public int GetPoints()
    {
        return this.points;
    }

    public string GetName()
    {
        return this.name;
    }

    public int CompareTo(MyPlayer player)
    {
        if (this.points < player.GetPoints())
            return 1;
        if (this.points > player.GetPoints())
            return -1;
        else
            return 0;
    }


    

}
