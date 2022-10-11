using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameController gc;
    private NotificationCenter nc;
    private GameObject[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(GameObject.FindWithTag("Manager"));

        hearts = new GameObject[3];
        for (int i = 1; i < 4; i++)
        {
            GameObject temp = GameObject.Find("Heart" + i);
      
            
            hearts[i - 1] = temp;

            temp.SetActive(false);
        }
        
       
        nc = NotificationCenter.Instance;
       

        nc.AddObserver("Start", onStart);
        nc.AddObserver("NewLevel", newLevel);
        nc.AddObserver("Dead", onDeath);
        nc.AddObserver("Win", onWin);
        nc.AddObserver("PlayerExit", onExit);
        nc.AddObserver("Menu", goMenu);
        nc.AddObserver("LessLife", lessLife);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void onExit(Notification noti)
    {
        if (SceneManager.sceneCountInBuildSettings - 2 > gc.Level)
        {

            SceneManager.LoadScene("Level " + (gc.NewLevel));
            

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
    }
    void newLevel(Notification noti)
    {
        
        //New Level Action

    }
    void goMenu(Notification noti)
    {
        nc.PostNotification(new Notification("MenuActive"));
        SceneManager.LoadScene("Menu");
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
