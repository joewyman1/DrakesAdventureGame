using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class Pause : MonoBehaviour
{
    public GameObject Menu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Menu.activeSelf)
            {
                Time.timeScale = 0f;
                Menu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                Menu.SetActive(false);
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
    }
    
}
