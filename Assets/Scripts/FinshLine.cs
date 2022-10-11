using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

using UnityEngine.SceneManagement;

public class FinshLine : MonoBehaviour
{
    private GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NotificationCenter.Instance.PostNotification(new Notification("PlayerExit"));
            
            
            
        }
    }

}

