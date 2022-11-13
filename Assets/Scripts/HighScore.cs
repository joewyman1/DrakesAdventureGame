using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore
{
    private string _name;
    private int _score;

    public string Name { get { return _name; } set { _name = value; } }
    public int Score { get { return _score; } set { _score = value; } }
    public HighScore(string name, int score)
    {
        _name = name;
        _score = score;

    }
    public HighScore(string name) : this(name, 0) { }

    public HighScore(int score):this("Empty", score){}
   
    public HighScore() : this("Empty", 0) { }

    
    


}


