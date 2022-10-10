using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Notifications;


public class GameController : ScriptableObject
{
   
    public GameObject game;
    public int Level { get { return _level; } }
    public int NewLevel {
        get {
            nc = NotificationCenter.Instance;
            nc.PostNotification(new Notification("NewLevel"));
            _level += 1;
            _livesLeft = 5;
            return _level;
        } }
    public int Lives { get { return _lives; } }
    public int LivesLeft { get { return _livesLeft; } }
    private int _level;
    private int _lives;
    private int _livesLeft;
    private static GameController _instance = null;
    private NotificationCenter nc;

    public static GameController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<GameController>();

            }
            return _instance;
        }
    }
    

    public GameController()
    {
        _level = 1;
        _lives = 5;
        _livesLeft = 5;
    }

    
    public void LessLife(int num)
    {
        _livesLeft = _livesLeft - num;
        if(_livesLeft == 0)
        {
            nc = NotificationCenter.Instance;
            nc.PostNotification(new Notification("Dead"));
        }
    }

    
    
}
