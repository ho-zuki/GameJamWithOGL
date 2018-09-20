using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveType0 : MonoBehaviour
{
    public GameObject enemy;
    public float moveSpeed = 2.0f;
    private bool moveLeft = true;
    //public float left, right;
    private int cnt = 0;
    private float left = 0, right = 0;

    public LayerMask playerLayer;
    public float enemyVision;
    private bool moveStart = false;


    // Use this for initialization
    void Start()
    {
        Utilities uti = new Utilities();
        left = 0 - (uti.getScreenWidth() / 2);
        right = uti.getScreenWidth() / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInRange()) moveStart = true;
        if (moveStart)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (moveLeft == true)
            {
                if (transform.position.x < left)
                {
                    moveLeft = false;
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    cnt++;
                }
            }
            else {
                //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                if (transform.position.x > right && cnt < 5)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    moveLeft = true;
                    cnt++;
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, enemyVision);
    }
    private bool PlayerInRange()
    {
        return Physics2D.OverlapCircle(transform.position, enemyVision, playerLayer);
    }
}

