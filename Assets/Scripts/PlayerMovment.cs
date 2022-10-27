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

    private GameController gc;
    private NotificationCenter nc;
    private float startTime = 0.0f, currentTime = 0.0f, timeToWait = 2.0f;

    private GameObject ballPopup;
    private GameObject ballIcon;
    private GameObject coinIcon;


    // Start is called before the first frame update
    void Awake()
    {
        gc = GameController.Instance;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ///Health and lives setup 
        _health = gc.Lives;
        _hasBall = false;

        


        

        Transform spawn = GameObject.FindGameObjectWithTag("Respawn").transform;

        player.transform.position = new Vector3(spawn.position.x, spawn.position.y, player.transform.position.z);

        ballPopup = GameObject.FindGameObjectWithTag("HUD").transform.Find("BallPopup").gameObject;
        ballIcon = GameObject.FindGameObjectWithTag("ballIcon");

        ballIcon.SetActive(false);
        coinIcon = GameObject.FindGameObjectWithTag("coinIcon");

        
    }

    void OnEnable()
    {
        nc = NotificationCenter.Instance;
        //Observers
        nc.AddObserver("Dead", OnDeath);
        nc.AddObserver("LessLife", lessLife);
        nc.AddObserver("NewLevel", NewLevel);
    }

    void OnDisable()
    {
        nc.RemoveObserver("Dead", OnDeath);
        nc.RemoveObserver("LessLife", lessLife);
        nc.RemoveObserver("NewLevel", NewLevel);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        if(startTime!= 0.0f)
        {
            if (currentTime - startTime > timeToWait)
            {
                ballPopup.SetActive(false);
            }
        }
        
        _move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Speed * _move, rb.velocity.y);


            if (_move != 0)
            {
                anim.Play("Base Layer.Run");
            }
            else
            {
                anim.Play("Base Layer.Idle");
            }
        
           
        

        if(_health!= gc.LivesLeft)
        {
            _health = gc.LivesLeft;

            //check for death
            if(_health == 0)
            {
                nc.PostNotification(new Notification("Dead"));
            }
            else
            {
               
            }
        }


        float mvm = Input.GetAxisRaw("Horizontal");
        if (mvm < 0)
        {

            GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (mvm > 0  )
        {

            GetComponent<SpriteRenderer>().flipX = false;

        }

        if (Input.GetButtonDown("Jump")&& !IsJumping)
        {
            rb.AddForce(new Vector2(rb.velocity.x, Jump));
        }
    }
   
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

       
    }

  
    private void lessLife(Notification notification)
    {
        //
    }
    private void OnDeath(Notification notification)
    {
        //Restart Game
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
   
    private bool checkAnimState(string name)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
        }
        else if (other.gameObject.CompareTag("Pain") || other.gameObject.CompareTag("Enemy"))
        {

            Vector3 direction = player.transform.position - other.gameObject.transform.position;

            if (direction.y > 0)
            {
                //collision is up
                rb.AddForce(new Vector2(rb.velocity.x, 400));
                if (other.gameObject.CompareTag("Pain"))
                {
                    orgColor = GetComponent<SpriteRenderer>().color;
                    gc.LessLife(1);
                    GetComponent<SpriteRenderer>().color = Color.red;
                }
                else if (other.gameObject.CompareTag("Enemy"))
                {
                    nc.PostNotification(new Notification("AddPoint"));
                    other.gameObject.SetActive(false);
                }
            }
            else
            {
                orgColor = GetComponent<SpriteRenderer>().color;
                gc.LessLife(1);
                GetComponent<SpriteRenderer>().color = Color.red;
            }



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

        }else if (other.gameObject.CompareTag("Ball"))
        {
            _hasBall = true;
            other.gameObject.SetActive(false);
            ballIcon.SetActive(true);

        }else if (other.gameObject.CompareTag("Coin")){

            nc.PostNotification(new Notification("Coin"));

            other.gameObject.SetActive(false);

            
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (GetComponent<SpriteRenderer>().color == Color.red)
            {
                GetComponent<SpriteRenderer>().color = orgColor;
            }
            IsJumping = true;
        }
    }
  
}
