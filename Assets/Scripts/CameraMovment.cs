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
    private bool scr;
    private float start = 0.0f;
    private NotificationCenter nc;
 
    void Awake() {
        nc = NotificationCenter.Instance;
        SceneManager.sceneLoaded += onLevelLoaded;
        scr = false;
    }

    // Update is called once per frame
    void Update()
    {
        float cur = Time.time;
        if (target)
        {
            if (scr && cur-start >dampTime*50)
            {
                scr = false;
                start = 0.0f;
                nc.PostNotification(new Notification("CanMove"));
            }
            if (scr)
            {
                Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
                Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime * 20);

            }
            else
            {
                Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
                Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }
    }
    void onLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            scr = true;
            start = Time.time; 
        }
    }

}
