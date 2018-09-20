using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveType1 : MonoBehaviour {

    public GameObject player;
    public GameObject enemy;
 
    public float enemyVision;

    public LayerMask playerLayer;
    public float moveSpeed;

    bool playerInRange;
    
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        playerInRange = Physics2D.OverlapCircle(transform.position, enemyVision, playerLayer);
        if (playerInRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
          	
	}
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position,enemyVision);
    }
    

    
}
