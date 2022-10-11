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



    private Color orgColor;

    public GameObject player;

    private GameController gc;

    // Start is called before the first frame update
    void Awake()
    {
        gc = GameController.Instance;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ///Health and lives setup 
        _health = gc.Lives;


        //Observers
        NotificationCenter.Instance.AddObserver("Dead",  OnDeath);
        NotificationCenter.Instance.AddObserver("LessLife", lessLife);
        NotificationCenter.Instance.AddObserver("NewLevel", NewLevel);

        Transform spawn = GameObject.FindGameObjectWithTag("Respawn").transform;

        player.transform.position = new Vector3(spawn.position.x, spawn.position.y, player.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        
        _move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Speed * _move, rb.velocity.y);


        if(_move!= 0) 
        {
            anim.Play("Walk");
        }
        else
        {
            anim.Play("Idle");
            /**
            if (!checkAnimState("Idle")&&!checkAnimState("Lay"))
            {
                anim.Play("Idle");
            }
            else if(!checkAnimState("Lay")&&!_layStarted)
            {
                startLay();
            }
            **/
            
        }
        

        if(_health!= gc.LivesLeft)
        {
            _health = gc.LivesLeft;

            //check for death
            if(_health == 0)
            {
                NotificationCenter.Instance.PostNotification(new Notification("Dead"));
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
 
    /**
    private void startLay()
    {
        _layStarted = true;
        float start = Time.time;
        while (Time.time < start + 2f)
        {
            if(_move!= 0)
            {
                return;
            }
        }
        if(_move == 0 && checkAnimState("Idle")) 
        {
            anim.Play("Lay");
        }
        else
        {
            return;
        }
        
    } 
    **/
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
        }else if (other.gameObject.CompareTag("Pain")|| other.gameObject.CompareTag("Enemy"))
        {
            orgColor = GetComponent<SpriteRenderer>().color;
            gc.LessLife(1);
            GetComponent<SpriteRenderer>().color = Color.red;

            rb.AddForce(new Vector2(rb.velocity.x, 400));

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
