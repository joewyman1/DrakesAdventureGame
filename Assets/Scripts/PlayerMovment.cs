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
    private bool sleep;

    private bool canBark;

    private GameController gc;
    private NotificationCenter nc;
    private float startTime = 0.0f, currentTime = 0.0f, timeToWait = 2.0f, startIdle = 0.0f, startBark = 0.0f;

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

        canBark = true;

        notPlayingAnimation = true;

<<<<<<< Updated upstream
        
=======
        nc = NotificationCenter.Instance;
        //Observers
       
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
    void OnEnable()
    {
        nc = NotificationCenter.Instance;
        //Observers
=======
>>>>>>> Stashed changes
        nc.AddObserver("Dead", OnDeath);
        nc.AddObserver("LessLife", lessLife);
        nc.AddObserver("NewLevel", NewLevel);
    }
<<<<<<< Updated upstream

    void OnDisable()
    {
=======
    void OnDisable()
    {
        nc = NotificationCenter.Instance;

>>>>>>> Stashed changes
        nc.RemoveObserver("Dead", OnDeath);
        nc.RemoveObserver("LessLife", lessLife);
        nc.RemoveObserver("NewLevel", NewLevel);
    }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        
        if(startTime!= 0.0f|| startBark != 0.0f || startIdle != 0.0f)
        {
            if(_move != 0)
            {
                notPlayingAnimation = true;

            }
            if (startTime != 0.0f && currentTime - startTime > timeToWait)
            {
                ballPopup.SetActive(false);
            }
            if (startBark!= 0.0f &&currentTime - startBark > 2.0f)
            {

                notPlayingAnimation = true;
                startBark = 0.0f;
                enemyPopup.SetActive(false);
                canBark = true;
            }
            if (startIdle != 0.0f && currentTime-startIdle > 6.0f)
            {
                
                anim.Play("Base Layer.Sleep");
                sleep = true;
                
            }
        }
        
        if (!IsJumping)
        {
            _move = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(Speed * _move, rb.velocity.y);

        }
        

        if (notPlayingAnimation)
        {
            if (_move != 0)
            {
                if (sleep)
                {
                    sleep = false;
                    startIdle = 0.0f;
                }



                anim.Play("Base Layer.Run");
            }
            else
            {
                if (!sleep  && startIdle == 0.0f)
                {
                    anim.Play("Base Layer.Idle");
                    startIdle = Time.time;
                }
            }
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

        if (Input.GetButtonDown("Jump")&& !IsJumping&& !sleep)
        {
            rb.AddForce(new Vector2(rb.velocity.x, Jump));
        }else if (Input.GetMouseButtonDown(0)&& canBark)
        {
            //Shoot Button
            anim.Play("Base Layer.Bark_Stand");
            nc.PostNotification(new Notification("Bark"));
            notPlayingAnimation = false;
            canBark = false;
            startBark = Time.time;
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){

                if(Vector2.Distance(enemy.transform.position, player.transform.position) <=  2.0f)
                {
                    enemy.SetActive(false);
                    enemyPopup.SetActive(true);
                    nc.PostNotification(new Notification("EnemyKilled"));
                }
            }

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
        else if (other.gameObject.CompareTag("Pain") )
        {

            Vector3 direction = player.transform.position - other.gameObject.transform.position;

            if (direction.y > 0)
            {
                //collision is up
                rb.AddForce(new Vector2(rb.velocity.x, 300));
                if (other.gameObject.CompareTag("Pain"))
                {
                    orgColor = GetComponent<SpriteRenderer>().color;
                    gc.LessLife(1);
                    GetComponent<SpriteRenderer>().color = Color.red;
                }
                
            }
            else
            {
              
                orgColor = GetComponent<SpriteRenderer>().color;
                gc.LessLife(1);
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            


        }else if (other.gameObject.CompareTag("Enemy"))
        {
            orgColor = GetComponent<SpriteRenderer>().color;
            gc.LessLife(1);
            GetComponent<SpriteRenderer>().color = Color.red;
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
