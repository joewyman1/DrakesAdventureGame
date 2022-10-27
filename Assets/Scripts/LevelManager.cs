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

    void OnEnable()
    {
        DontDestroyOnLoad(GameObject.FindWithTag("Manager"));
        nc = NotificationCenter.Instance;


        nc.AddObserver("Start", onStart);
        nc.AddObserver("Dead", onDeath);
        nc.AddObserver("Win", onWin);
        nc.AddObserver("PlayerExit", onExit);
        nc.AddObserver("Menu", goMenu);
        nc.AddObserver("LessLife", lessLife);
        nc.AddObserver("Coin", coinHandler);
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
            coinIcon = GameObject.FindGameObjectWithTag("coinIcon");
            coinCount = coinIcon.transform.GetChild(0).gameObject.GetComponent<Text>();
            coinCount.text = "x" + gc.Coins;

            hearts = new GameObject[3];
            for (int i = 1; i < 4; i++)
            {
                GameObject temp = GameObject.Find("Heart" + i);


                hearts[i - 1] = temp;

                temp.SetActive(false);
            }
        }
    }
    void onExit(Notification noti)
    {
        if (SceneManager.sceneCountInBuildSettings - 2 > gc.Level)
        {
     
            SceneManager.LoadScene("Level " + (gc.NewLevel));
            coinIcon = GameObject.FindGameObjectWithTag("coinIcon");
            coinCount =coinIcon.transform.GetChild(0).gameObject.GetComponent<Text>();
            

        }
        else if (SceneManager.sceneCountInBuildSettings -2  == gc.Level)
        {
            //Win

            nc.PostNotification(new Notification("Win"));
        }
    }
    void onStart(Notification noti)
    {
        gc = GameController.Instance;
        SceneManager.LoadScene("Level 1");

        
        foreach (GameObject heart in hearts)
        {

            heart.SetActive(true);

        }

       

    }
    void onDeath(Notification noti)
    {
        nc.PostNotification(new Notification("MenuActive"));
        SceneManager.LoadScene("Death Screen");
        gc.Destroy();
    }
    void onWin(Notification noti)
    {
        nc.PostNotification(new Notification("MenuActive"));
        SceneManager.LoadScene("Win Screen");
        gc.Destroy();
    }
    
    void goMenu(Notification noti)
    {
        nc.PostNotification(new Notification("MenuActive"));
        SceneManager.LoadScene("Menu");
        gc.Destroy();
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
    }
}
