using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void onMouseDown()
    {
        Debug.Log("Working...");
        NotificationCenter.Instance.PostNotification(new Notification("Start"));
    }
}
