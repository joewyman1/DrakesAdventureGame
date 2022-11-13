using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Notifications;
using UnityEngine.SceneManagement;

public class PlayerMovment : MonoBehaviour

{
    public float Speed;
    public float Jump;
    public bool IsJumping;

    //private bool _layStarted;
    private float _move;
    private int _health;
    private Rigidbody2D rb;
    private Animator anim;

    public TMP_Text lives;
    public TMP_Text level;

    private bool _hasBall;
    private Color orgColor;

    public bool HasBall { get { return _hasBall; } }
    public GameObject player;
    private bool notPlayingAnimation;
    private bool sleep, invincible;

    private bool canBark, barking, sitting;

    private GameController gc;
    private NotificationCenter nc;
    private float enemyPopupTimer = 0.0f, startTime = 0.0f, currentTime = 0.0f, timeToWait = 2.0f, startIdle = 0.0f, startBark = 0.0f, startPain = 0.0f;

    private GameObject ballPopup;
    private GameObject ballIcon;
    private GameObject coinIcon;
    private GameObject enemyPopup;



    // Start is called before the first frame update
    void Awake()
    {
        gc = GameController.Instance;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ///Health and lives setup 
        _health = gc.Lives;
        _hasBall = false;

        invincible = false;
        canBark = true;
        barking = false;

        notPlayingAnimation = true;


        nc = NotificationCenter.Instance;


        Transform spawn = GameObject.FindGameObjectWithTag("Respawn").transform;

        player.transform.position = new Vector3(spawn.position.x, spawn.position.y, player.transform.position.z);

        ballPopup = GameObject.FindGameObjectWithTag("HUD").transform.Find("BallPopup").gameObject;
        ballIcon = GameObject.FindGameObjectWithTag("ballIcon");

        ballIcon.SetActive(false);
        coinIcon = GameObject.FindGameObjectWithTag("coinIcon");

        enemyPopup = GameObject.FindGameObjectWithTag("HUD").transform.Find("EnemyKilled").gameObject;


    }

    void OnEnable()
    {
        nc = NotificationCenter.Instance;
        nc.AddObserver("NewLevel", NewLevel);
        nc.AddObserver("EnemyKilled", onEnemyKilled);
    }

    void OnDisable()
    {
        nc = NotificationCenter.Instance;
        nc.RemoveObserver("NewLevel", NewLevel);
        nc.RemoveObserver("EnemyKilled", onEnemyKilled);

    }

    void Update()
    {
        checkTimer();
        moveChar();
        checkHealth();
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;


    }
    private void checkTimer()
    {
        currentTime = Time.time;

        if (startTime != 0.0f || startBark != 0.0f || startIdle != 0.0f || startPain != 0.0f || enemyPopup!= 0.0f)
        {

            if (_move != 0)
            {
                notPlayingAnimation = true;

            }
            if (startTime != 0.0f && currentTime - startTime > timeToWait)
            {
                ballPopup.SetActive(false);
            }
            if (startBark != 0.0f && currentTime - startBark > 1.3f)
            {

                notPlayingAnimation = true;
                startBark = 0.0f;
                
                barking = false;
                canBark = true;
            }
            if (currentTime - enemyPopup > 2.0f)
            {
                enemyPopup.SetActive(false);
            }
            if (startIdle != 0.0f && currentTime - startIdle > 6.0f)
            {

                anim.Play("Base Layer.Sleep");
                sleep = true;

            }
            else if (startIdle != 0.0f && currentTime - startIdle > 3.0f)
            {
                anim.Play("Base Layer.Sit");
                sitting = true;
            }
            if (startPain != 0)
            {
                invincible = true;
            }
            if (startPain != 0 && currentTime - startPain > 5.0f && currentTime - startPain < 1.0f)
            {
                GetComponent<SpriteRenderer>().color = orgColor;
            }
            else if (startPain != 0 && currentTime - startPain > 1.0f && currentTime - startPain < 1.5f)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (startPain != 0 && currentTime - startPain > 1.5f)
            {
                GetComponent<SpriteRenderer>().color = orgColor;

                startPain = 0.0f;
                invincible = false;
            }
        }
        
    }
    private void checkHealth()
    {
        if (_health != gc.LivesLeft)
        {
            _health = gc.LivesLeft;
            if (_health == 0)
            {
                nc.PostNotification(new Notification("Dead"));
            }
        }
    }
    private void moveChar()
    {
        if (!IsJumping)
        {
            _move = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(Speed * _move, rb.velocity.y);
        }

        if (notPlayingAnimation)
        {
            if (_move != 0)
            {


                sleep = false;
                sitting = false;
                startIdle = 0.0f;
                if (barking)
                {
                    anim.Play("Base Layer.Bark_Stand");
                }
                else
                {
                    anim.Play("Base Layer.Run");
                }

            }
            else
            {
                if (!sleep && !sitting && startIdle == 0.0f)
                {
                    anim.Play("Base Layer.Idle");
                    startIdle = Time.time;
                }
            }
        }

        float mvm = Input.GetAxisRaw("Horizontal");
        if (mvm < 0)
        {

            GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (mvm > 0)
        {

            GetComponent<SpriteRenderer>().flipX = false;

        }

        if (Input.GetButtonDown("Jump") && !IsJumping && !sleep)
        {
            rb.AddForce(new Vector2(rb.velocity.x, Jump));
        }
        else if (Input.GetMouseButtonDown(0) && canBark )
        {
            barking = true;
            anim.Play("Base Layer.Bark_Stand");
            nc.PostNotification(new Notification("Bark"));
            notPlayingAnimation = false;
            canBark = false;
            startBark = Time.time;

        }

    }

    private void NewLevel(Notification nc)
    {

        ballIcon = GameObject.FindGameObjectWithTag("ballIcon");
        coinIcon = GameObject.FindGameObjectWithTag("coinIcon");



        ballIcon.SetActive(false);
        ballPopup = GameObject.FindGameObjectWithTag("HUD").transform.Find("BallPopup").gameObject;
        _hasBall = false;
        player = GameObject.FindGameObjectWithTag("Player");

        GameObject rPoint = GameObject.FindGameObjectWithTag("Respawn");
        Transform spawn = rPoint.transform;

        player.transform.position = new Vector3(spawn.position.x, spawn.position.y, player.transform.position.z);


    }
    private void onEnemyKilled(Notification noti)
    {
        enemyPopup.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;


        }
        else if (other.gameObject.CompareTag("Pain") && !invincible)
        {

            Vector3 direction = player.transform.position - other.gameObject.transform.position;

            if (direction.y > 0)
            {
                //collision is up
                rb.AddForce(new Vector2(rb.velocity.x, 300));
                gc.LessLife(1);
                GetComponent<SpriteRenderer>().color = Color.red;
                startPain = Time.time;

            }
            else
            {

                orgColor = GetComponent<SpriteRenderer>().color;
                gc.LessLife(1);
                GetComponent<SpriteRenderer>().color = Color.red;
            }



        }
        else if (other.gameObject.CompareTag("Enemy") && !invincible && !other.otherRigidbody.gameObject.CompareTag("Projectile"))
        {
            gc.LessLife(1);

            orgColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = Color.red;

            startPain = Time.time;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            if (_hasBall)
            {
                nc.PostNotification(new Notification("PlayerExit"));

            }
            else
            {

                nc.PostNotification(new Notification("PlayerCannotExit"));
                ballPopup.SetActive(true);
                startTime = Time.time;

            }

        }
        else if (other.gameObject.CompareTag("Ball"))
        {
            _hasBall = true;
            other.gameObject.SetActive(false);
            ballIcon.SetActive(true);

        }
        else if (other.gameObject.CompareTag("Coin"))
        {

            nc.PostNotification(new Notification("Coin"));

            other.gameObject.SetActive(false);


        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsJumping = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile")){
            float cur = Time.time;
            nc.PostNotification(new Notification("EnemyHit", other.gameObject));
            enemyPopup.SetActive(true);

            enemyPopupTimer = Time.time;

        }
    }


}
