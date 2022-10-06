using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

using UnityEngine.SceneManagement;

public class FinshLine : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.sceneCountInBuildSettings+1 > GameController.Instance.Level)
            {
                
                SceneManager.LoadScene("Level " + (GameController.Instance.NewLevel));
            }
            else if (SceneManager.sceneCountInBuildSettings + 1 == GameController.Instance.Level)
            {
                //Win

                NotificationCenter.Instance.PostNotification(new Notification("Win"));
            }
            
            
            
        }
    }

}

