using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore
{
    private string _name;
    private int _score;
    private int _kills;

    public string Name { get { return _name; } set { _name = value; } }
    public int Score { get { return _score; } set { _score = value; } }
    public int Kills { get { return _kills; } set { _kills = value; } }

    public HighScore(string name, int score, int kills)
    {
        _name = name;
        _score = score;
        _kills = kills;
    }
    public HighScore(string name) : this(name, 0, 0) { }
    public HighScore(int score, int kills) :this("Empty", score , kills){}
    public HighScore() : this("Empty", 0, 0) { }

}


