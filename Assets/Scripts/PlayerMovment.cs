using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Notifications;



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
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ///Health and lives setup 
        _health = GameController.Instance.Lives;

        level.text = "Level " + GameController.Instance.Level;
        lives.text = GameController.Instance.LivesLeft + " Lives Left...";
        _health = GameController.Instance.Lives;

        //Observers
        NotificationCenter.Instance.AddObserver("Dead", (Action<Notifications.Notification>) OnDeath);
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
        

        if(_health!= GameController.Instance.LivesLeft)
        {
            _health = GameController.Instance.LivesLeft;

            //check for death
            if(_health == 0)
            {
                NotificationCenter.Instance.PostNotification(new Notification("Dead"));
            }
            else
            {
                lives.text = GameController.Instance.LivesLeft + " Lives Left...";
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
    private void OnDeath()
    {

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
        }else if (other.gameObject.CompareTag("Pain"))
        {
            GameController.Instance.LessLife(1);

            rb.AddForce(new Vector2(rb.velocity.x, 400));

        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsJumping = true;
        }
    }
}
