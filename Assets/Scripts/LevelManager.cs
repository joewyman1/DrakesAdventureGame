using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    private GameController gc;
    private NotificationCenter nc;

    private GameObject[] hearts;
    private GameObject ballIcon;

    private GameObject coinIcon;
    private Text coinCount;
    private Text enemyText;


    private AudioSource _bark;
    private AudioSource _wimper;
    private AudioSource _breath;

    private List<HighScore> highScores;
    private Text scoreboard;
    void OnEnable()
    {
        gc = GameController.Instance;
        highScores = XMLManager.instance.loadScores();
        loadScoreboard();

        DontDestroyOnLoad(GameObject.FindWithTag("Manager"));

        nc = NotificationCenter.Instance;


        nc.AddObserver("Start", onStart);
        nc.AddObserver("Dead", onDeath);
        nc.AddObserver("Win", onWin);
        nc.AddObserver("PlayerExit", onExit);
        nc.AddObserver("Menu", goMenu);
        nc.AddObserver("LessLife", lessLife);
        nc.AddObserver("Coin", coinHandler);
        nc.AddObserver("NameBtnPressed", onNameBtnPressed);
        nc.AddObserver("EnemyKilled", enemyKilled);
        nc.AddObserver("RestartLevel", onRestartLevel);
        nc.AddObserver("Bark", onBark);
        nc.AddObserver("Instructions", instructions);



    }

    void OnDisable()
    {
        nc.RemoveObserver("Start", onStart);
        nc.RemoveObserver("Dead", onDeath);
        nc.RemoveObserver("Win", onWin);
        nc.RemoveObserver("PlayerExit", onExit);
        nc.RemoveObserver("Menu", goMenu);
        nc.RemoveObserver("LessLife", lessLife);
        nc.RemoveObserver("Coin", coinHandler);
        nc.RemoveObserver("NameBtnPressed", onNameBtnPressed);
        nc.RemoveObserver("EnemyKilled", enemyKilled);
        nc.RemoveObserver("RestartLevel", onRestartLevel);
        nc.RemoveObserver("Bark", onBark);
        nc.RemoveObserver("Instructions", instructions);


    }
    // Start is called before the first frame update
    void Start()
    {

        

        ballIcon = GameObject.FindGameObjectWithTag("ballIcon");

        ballIcon.SetActive(false);

        

        hearts = new GameObject[3];
        for (int i = 1; i < 4; i++)
        {
            GameObject temp = GameObject.Find("Heart" + i);


            hearts[i - 1] = temp;

            temp.SetActive(false);
        }

        SceneManager.sceneLoaded += onLevelLoaded;


        nc = NotificationCenter.Instance;




    }
    private void onRestartLevel(Notification n)
    {
        gc.RestartLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }
    private void loadScoreboard()
    {
        scoreboard = GameObject.Find("/MenuCanvas/ScorePanel/ScoreBoard").GetComponent<Text>();
        HighScore[] scores = highScores.ToArray();
        scores = sortArray(scores);
        string temp = "High Scores:$";
        for (int i = 0; i < highScores.Count; i++)
        {
            HighScore curr = scores[i];
            temp += curr.Name + "- Kills:" + curr.Kills + ", Score:" + curr.Score + '$';
        }
        temp = temp.Replace('$', '\n');
        scoreboard.text = temp;
    }
    // Update is called once per frame
    void Update()
    {

    }
    void coinHandler(Notification nc)
    {
        coinIcon = GameObject.FindGameObjectWithTag("coinIcon");
        coinCount = coinIcon.transform.GetChild(0).gameObject.GetComponent<Text>();
        gc.AddCoin();


        coinCount.text = "x" + gc.Coins;
    }
    void onLevelLoaded(Scene scene, LoadSceneMode mode)
    {


        if (scene.name.StartsWith("Level"))
        {
            enemyText = GameObject.FindGameObjectWithTag("KillCount").GetComponent<Text>();

            _bark = GameObject.FindGameObjectWithTag("Bark").GetComponent<AudioSource>();
            _wimper = GameObject.FindGameObjectWithTag("Wimper").GetComponent<AudioSource>();
            _breath = GameObject.FindGameObjectWithTag("Breath").GetComponent<AudioSource>();

            _breath.loop = true;
            _breath.Play();

            coinIcon = GameObject.FindGameObjectWithTag("coinIcon");
            coinCount = coinIcon.transform.GetChild(0).gameObject.GetComponent<Text>();
            coinCount.text = "x" + gc.Coins;

            hearts = new GameObject[3];
            for (int i = 1; i < 4; i++)
            {
                GameObject temp = GameObject.Find("Heart" + i);


                hearts[i - 1] = temp;

                temp.SetActive(true);
            }
        }else if(scene.name == "Menu")
        {
            if (highScores != null)
            {
                loadScoreboard();
            }
        }
    }
    void enemyKilled(Notification noti)
    {
        enemyText = GameObject.FindGameObjectWithTag("KillCount").GetComponent<Text>();
        enemyText.text = "Enemies Slain: " + gc.Kills;
    }
    void onBark(Notification notification)
    {
        _breath.Pause();
        _bark.Play();
        _breath.Play();
    }

    void onExit(Notification noti)
    {
       
        if (SceneManager.sceneCountInBuildSettings - 5 > gc.Level)
        {
            SceneManager.LoadScene("Level " + (gc.NewLevel));
            coinIcon = GameObject.FindGameObjectWithTag("coinIcon");
            coinCount = coinIcon.transform.GetChild(0).gameObject.GetComponent<Text>();
        }
        else if (SceneManager.sceneCountInBuildSettings - 5 == gc.Level)
        {
            nc.PostNotification(new Notification("Win"));
        }
    }
    void instructions(Notification n)
    {
        gc = GameController.Instance;
        SceneManager.LoadScene("Instructions");
    }
    void onStart(Notification noti)
    {
        gc = GameController.Instance;
        SceneManager.LoadScene("Level 1");
        
        
    }
    void onDeath(Notification noti)
    {
        nc.PostNotification(new Notification("MenuActive"));
        SceneManager.LoadScene("Death Screen");
        gc.Destroy();
    }
    void onWin(Notification noti)
    {
        SceneManager.LoadScene("AddHighScore");
        string temp = "YOUR SCORE$$" + gc.Kills + " ENEMIES ELIMINATED$" + gc.Coins + " COINS COLLECTED";
        temp = temp.Replace('$', '\n');
        GameObject.Find("/MenuCanvas/Score").GetComponent<TMPro.TextMeshProUGUI>().text = temp;
    }
    void onNameBtnPressed(Notification Noti)
    {
        string name = GameObject.FindGameObjectWithTag("Input").GetComponent<InputField>().text;
        if (name.Length > 0)
        {
            highScores.Add(new HighScore(name, gc.Coins, gc.Kills));
            XMLManager.instance.saveScores(highScores);
        }
        else
        {
            highScores.Add(new HighScore(gc.Coins, gc.Kills));
            XMLManager.instance.saveScores(highScores);
        }
        nc.PostNotification(new Notification("NameAdded"));
        nc.PostNotification(new Notification("Menu"));
    }
    void goMenu(Notification noti)
    {     
        nc.PostNotification(new Notification("MenuActive"));
        SceneManager.LoadScene("Menu");     
        gc.Destroy();
    }
    private HighScore[] sortArray(HighScore [] arr)
    {
        HighScore[] oldArray = arr;
        int length = oldArray.Length;
        HighScore[] newArray = new HighScore[length];
        
        for(int i = 0; i < length; i++) 
        {
            int best = 0;
            HighScore bestScore = null;

            foreach (HighScore hs in oldArray)
            {
                if (hs.Score+hs.Kills > best)
                {
                    bestScore = hs;
                    best = hs.Score+hs.Kills;                    
                }
            }
            List<HighScore> tempo = new List<HighScore>(oldArray);
            tempo.Remove(bestScore);
            oldArray = tempo.ToArray();
            newArray[i] = bestScore;
        }
        return newArray;
        
    }
    void lessLife(Notification noti)
    {
        hearts = new GameObject[3];
        for (int i = 1; i < 4; i++)
        {
            GameObject temp = GameObject.Find("Heart" + i);
            hearts[i - 1] = temp;
        }
        switch (gc.LivesLeft)
        {
            case 2:
                hearts[2].SetActive(false);
                break;
            case 1:
                hearts[1].SetActive(false);
                break;
        }
        _breath.Pause();
        _wimper.Play();
        _breath.Play();
    }

}