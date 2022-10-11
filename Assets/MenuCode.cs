using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;
public class MenuCode : MonoBehaviour
{
    private NotificationCenter nc;
    // Start is called before the first frame update
    void Start()
    {
        nc = NotificationCenter.Instance;
        nc.PostNotification(new Notification("MenuActive"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickStart()
    {
        NotificationCenter.Instance.PostNotification(new Notification("Start"));
        nc.PostNotification(new Notification("MenuDectivate"));
    }
   public  void onClickQuit()
    {
        Application.Quit();
    }
}
