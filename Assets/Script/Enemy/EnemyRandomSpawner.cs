using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
    Vector2 spawnPos;
    float randY, randX;
    float nextSpawn = 0.0f;
    public float spawnRate = 2f;
    public float top = 0.0f, bottom = 0.0f, left = 0.0f, right = 0.0f;
    // false for vertical, true for horizontal
    public bool axis;


    float top_left = 0.0f;
    float bottom_left = 5.14f;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            randY = Random.Range(top, bottom);
            randX = Random.Range(left, right);
            if (axis) spawnPos = new Vector2(randX, top);
            else spawnPos = new Vector2(left, randY);
            Instantiate(enemy, spawnPos, Quaternion.identity);

        }

	}
}
