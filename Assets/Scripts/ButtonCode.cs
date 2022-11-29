using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class ButtonCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {

        NotificationCenter.Instance.PostNotification(new Notification("Start"));
    }
    public void GoToInstructions()
    {
        NotificationCenter.Instance.PostNotification(new Notification("Instructions"));

    }
    public void Quit()
    {

        Application.Quit();
    }
    public void restart()
    {
        NotificationCenter.Instance.PostNotification(new Notification("RestartLevel"));
    }
    public void deathContinue()
    {
        NotificationCenter.Instance.PostNotification(new Notification("Menu"));
    }
    public void nameAdded()
    {
        NotificationCenter.Instance.PostNotification(new Notification("NameBtnPressed"));
        
        
    }
}
