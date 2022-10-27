using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class Pause : MonoBehaviour
{
    public GameObject Menu;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        
    }

    void OnEnable()
    {
        NotificationCenter.Instance.AddObserver("MenuActive", openMenu);
        NotificationCenter.Instance.AddObserver("MenuDeactivate", closeMenu);
    }

    void OnDisable()
    {
        NotificationCenter.Instance.RemoveObserver("MenuActive", openMenu);
        NotificationCenter.Instance.RemoveObserver("MenuDeactivate", closeMenu);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Menu.activeSelf)
            {
                Time.timeScale = 0f;
                Menu.SetActive(true);
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Menu.SetActive(false);
                Cursor.visible = false;
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
        Cursor.visible = false;
    }
    private void openMenu(Notification no)
    {
        Cursor.visible = true;
    }
    private void closeMenu(Notification no)
    {
        Cursor.visible = false;
    }
}
