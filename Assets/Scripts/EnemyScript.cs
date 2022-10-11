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

    private SpriteRenderer _mSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        leftPoint = enemy.transform.GetChild(0).position.x;
        rightPoint = enemy.transform.GetChild(1).position.x;

        _mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mSpriteRenderer.flipX = bIsGoingRight;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionTranslation = (bIsGoingRight) ? enemy.transform.right : -enemy.transform.right;
        directionTranslation *= Time.deltaTime * mMovementSpeed;

        enemy.transform.Translate(directionTranslation);

        if (enemy.transform.position.x >= rightPoint)
        {
            bIsGoingRight = false;
            _mSpriteRenderer.flipX = bIsGoingRight;
        }else if (enemy.transform.position.x <= leftPoint)
        {
            bIsGoingRight = true;
            _mSpriteRenderer.flipX = bIsGoingRight;
        }
    }
}
