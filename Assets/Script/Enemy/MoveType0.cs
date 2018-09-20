using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveType0 : MonoBehaviour
{
    public GameObject enemy;
    public float moveSpeed = 2.0f;
    private bool moveLeft = true;
    public float left, right;
    public Animation GoLeft, GoRight;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        if (moveLeft == true)
        {
            
            
            if (transform.position.x < left)
            {
                moveLeft = false;
                transform.eulerAngles = new Vector3(0, -180, 0);
            
            }
        }
        else {
            //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x > right)
            {
                
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveLeft = true;
            }
        }
    }
}

