using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyScript : MonoBehaviour
{
    public GameObject enemy;
    private float leftPoint;
    private float rightPoint;

    public float mMovementSpeed = 3.0f;
    private bool bIsGoingRight = true;
    private Animator anim;
    private float hitTime = 0.0f;

    private SpriteRenderer _mSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        leftPoint = enemy.transform.GetChild(0).position.x;
        rightPoint = enemy.transform.GetChild(1).position.x;
        anim = GetComponent<Animator>();
        _mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mSpriteRenderer.flipX = bIsGoingRight;
    }

    // Update is called once per frame
    void Update()
    {
        float current = Time.time;

        if (! anim.GetCurrentAnimatorStateInfo(0).IsName("Die") && hitTime == 0.0f)
        {
            anim.Play("Walk");

            Vector3 directionTranslation = (bIsGoingRight) ? enemy.transform.right : -enemy.transform.right;
            directionTranslation *= Time.deltaTime * mMovementSpeed;

            enemy.transform.Translate(directionTranslation);
            if (enemy.transform.position.x >= rightPoint)
            {
                bIsGoingRight = false;
                _mSpriteRenderer.flipX = bIsGoingRight;
            }
            else if (enemy.transform.position.x <= leftPoint)
            {
                bIsGoingRight = true;
                _mSpriteRenderer.flipX = bIsGoingRight;
            }
        }else if(hitTime != 0.0f && current - hitTime>0.5f && !anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            hitTime = 0.0f;
        }


        
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")&&! anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            anim.Play("Hit");
            hitTime = Time.time;
        }
    }
}
