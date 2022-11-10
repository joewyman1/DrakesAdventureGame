using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Notifications;


public class GameController 
{
   
    public GameObject game;
    private int _coinCount;
    public int Coins { get { return _coinCount; }  }
    public int Level { get { return _level; } }
    public int NewLevel {
        get {
            nc.PostNotification(new Notification("NewLevel"));
            _level += 1;
            _livesLeft = 3;
            return _level;
        } }
    public int Lives { get { return _lives; } }
    public int LivesLeft { get { return _livesLeft; } }
    public int Kills { get { return _kills; }}
    private int _level;
    private int _lives;
    private int _livesLeft;
    private int _kills;
    private static GameController _instance = null;
    private NotificationCenter nc;

    public static GameController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameController();
            }
            return _instance;
        }
    }
    
    public GameController()
    {
        _level = 1;
        _lives = 3;
        _livesLeft = 3;
        _kills = 0;
        nc = NotificationCenter.Instance;
        _coinCount = 0;
        nc.AddObserver("EnemyKilled", onKill);
    }
    public void AddCoin()
    {
        _coinCount += 1;
    }
    public void Destroy()
    {
        nc = NotificationCenter.Instance;

        nc.RemoveObserver("EnemyKilled", onKill);
        _instance = null;
    }
    
    public void LessLife(int num)
    {
        
        _livesLeft = _livesLeft - num;
        nc.PostNotification(new Notification("LessLife"));
        if (_livesLeft == 0)
        {
            nc = NotificationCenter.Instance;
            nc.PostNotification(new Notification("Dead"));
        }
    }

    private void onKill(Notification noti)
    {
        _kills += 1;
        nc.PostNotification(new Notification("NewKill"));
    }
    
}
