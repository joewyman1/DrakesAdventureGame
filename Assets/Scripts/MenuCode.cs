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
        GameObject.Find("/GameMusic/Audio Source").GetComponent<AudioSource>().Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickStart()
    {
        NotificationCenter.Instance.PostNotification(new Notification("Start"));
        GameObject.Find("/GameMusic/Audio Source").GetComponent<AudioSource>().UnPause();
        nc.PostNotification(new Notification("MenuDectivate"));
    }
   public  void onClickQuit()
    {
        Application.Quit();
    }
}
