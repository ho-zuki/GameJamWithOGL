using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum Type{
        E_LEFT, E_RIGHT, E_TAG_LEFT, E_TAG_RIGHT,
    };

    private Vector2 pos;
    private int state;
    int timer;
    float speed;

    const float movespeed = 0.01f;


    void Start()
    {
        state = 0;
        pos = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        timer = 0;
    }


    void Update()
    {
        // 移動
        if (timer >= 60)
        {
            ++state;
            if (state > 3) state = 0;
            timer = 0;
        }
        else timer++;

        switch (state)
        {
            case (int)(Type.E_LEFT):
                speed = -movespeed;
                break;

            case (int)(Type.E_RIGHT):
                speed = movespeed;
                break;

            case (int)(Type.E_TAG_LEFT):
                speed = -movespeed;
                break;

            case (int)(Type.E_TAG_RIGHT):
                speed = movespeed;
                break;
        }

        pos.x = speed;
        transform.Translate(pos.x, 0.0f, 0.0f);
    }

    /*----- 移動 -----*/
    void Move()
    {
       
    }
}

