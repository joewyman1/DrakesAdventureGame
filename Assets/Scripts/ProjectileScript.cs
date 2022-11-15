using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notifications;

public class ProjectileScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject player;
    public GameObject explosion;

    private GameController gc;
    private NotificationCenter nc;
    private Vector3 org;
    private Rigidbody2D rb;
    private float deadTime = 0.0f, expTime = 0.0f;
    private GameObject tempEnemy;



    // Start is called before the first frame update
    void Start()
    {
        org = projectile.transform.position;
        gc = GameController.Instance;
        nc = NotificationCenter.Instance;
        DontDestroyOnLoad(projectile);
        DontDestroyOnLoad(explosion);
        tempEnemy = null;
        nc = NotificationCenter.Instance;
        nc.AddObserver("Bark", onBark);
        rb = projectile.GetComponent<Rigidbody2D>();
        rb.Sleep();
    }
    void onEnable()
    {
        nc = NotificationCenter.Instance;
        nc.AddObserver("Bark", onBark);

    }

    void onDisable()
    {
        nc.RemoveObserver("Bark", onBark);

    }
    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        if (!projectile.activeSelf)
        {
            rb.Sleep();
        }
        if (player == null)
        {
            player = GameObject.Find("/Player");
        }

        if (deadTime != 0.0f && currentTime - deadTime > 0.7f && tempEnemy != null)
        {
            nc.PostNotification(new Notification("isDead"));
            tempEnemy.SetActive(false);
            deadTime = 0.0f;
            tempEnemy = null;
        }
        if(expTime != 0.0f && currentTime - expTime > 0.4f)
        {
            explosion.transform.position = org;
            expTime = 0.0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            onHit(other.gameObject);
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
    private void onHit(GameObject enemy)
    {
        tempEnemy = enemy;
        Animator an = enemy.GetComponent<Animator>();
        an.Play("Die");
        deadTime = Time.time;
        gc.addKill();
        nc.PostNotification(new Notification("EnemyKilled"));
        nc.PostNotification(new Notification("isDying", enemy));
        expTime = Time.time;
        explosion.transform.position = enemy.transform.position;
        projectile.transform.position = org;
        rb.Sleep();
    }
}
