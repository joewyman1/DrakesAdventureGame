using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class Pause : MonoBehaviour
{
    public GameObject Menu;
    private NotificationCenter nc;
    void Start()
    {
        nc = NotificationCenter.Instance;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Menu.activeSelf)
            {
                
                nc.PostNotification(new Notification("MenuActive"));
                GameObject.Find("/GameMusic/Audio Source").GetComponent<AudioSource>().Pause();
                GameObject.Find("/SFX/breath").GetComponent<AudioSource>().Pause();
                Time.timeScale = 0f;
                Menu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                Menu.SetActive(false);
                nc.PostNotification(new Notification("Resumed"));
                GameObject.Find("/GameMusic/Audio Source").GetComponent<AudioSource>().UnPause();
                GameObject.Find("/SFX/breath").GetComponent<AudioSource>().UnPause();
                nc.PostNotification(new Notification("MenuDectivate"));
            }
        }
    }

    public void quit()
    {
        Application.Quit();
    }

    public void resume()
    {
        Time.timeScale = 1f;
        Menu.SetActive(false);
        nc.PostNotification(new Notification("Start"));
        GameObject.Find("/GameMusic/Audio Source").GetComponent<AudioSource>().UnPause();
        nc.PostNotification(new Notification("MenuDectivate"));
    }
    
}
