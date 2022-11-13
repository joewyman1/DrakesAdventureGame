using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class ProjectileScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject player;

    private GameController gc;
    private NotificationCenter nc;
    private Vector3 org;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        org = projectile.transform.position;
        gc = GameController.Instance;
        nc = NotificationCenter.Instance;

        nc = NotificationCenter.Instance;
        nc.AddObserver("Bark", onBark);
        rb = projectile.GetComponent<Rigidbody2D>();
        rb.Sleep();
    }
    void onEnable()
    {
        nc = NotificationCenter.Instance;
        nc.AddObserver("Bark", onBark);
        nc.AddObserver("EnemyHit", onHit);
    }

    void onDisable()
    {
        nc.RemoveObserver("Bark", onBark);
        nc.RemoveObserver("EnemyHit", onHit);
    }
    // Update is called once per frame
    void Update()
    {
        if(!projectile.activeSelf)
        {
            rb.Sleep();
        } 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            
        }
    }
    private void onBark(Notification noti)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - player.transform.position;
        mouseDir.z = 0.0f;
        mouseDir = mouseDir.normalized;

        projectile.transform.position = player.transform.position;

        rb.WakeUp();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0.0f;

        rb.AddForce(mouseDir * 500);
    }
    private void onHit(Notification noti)
    {
        GameObject enemy = (GameObject) noti.Object;
        enemy.SetActive(false);
        gc.addKill();
        nc.PostNotification(new Notification("EnemyKilled"));
        projectile.transform.position = org;
        rb.Sleep();
    }
}
