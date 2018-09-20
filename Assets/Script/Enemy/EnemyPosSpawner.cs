using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosSpawner : MonoBehaviour {
    public GameObject enemy;
    public Vector2 spawnPos;
    public int number;
    public float spawnTime;
    public float timeInterval;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < number; i++)
        {
            if (Time.time > spawnTime)
            {
                spawnTime = Time.time + timeInterval;
                Instantiate(enemy, spawnPos, Quaternion.identity);
            }
        }
	}
}
