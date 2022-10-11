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
    public void Quit()
    {

        Application.Quit();
    }
    public void deathContinue()
    {
        NotificationCenter.Instance.PostNotification(new Notification("Menu"));
    }
}