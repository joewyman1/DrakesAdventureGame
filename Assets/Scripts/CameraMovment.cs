using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Notifications;

public class CameraMovment : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private Transform target;
    private NotificationCenter nc;
    

    void Awake()
    {
        nc = NotificationCenter.Instance;
        SceneManager.sceneLoaded += onLevelLoaded;
        nc.AddObserver("RestartLevel", onRestartLevel);
    }
    void onDisable()
    {
        nc.RemoveObserver("RestartLevel", onRestartLevel);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        }
    }
    void onLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    void onRestartLevel(Notification n) 
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
