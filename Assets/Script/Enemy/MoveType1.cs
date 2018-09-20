using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveType1 : MonoBehaviour {

    public GameObject player;
    public GameObject enemy;
 
    public float enemyVision;

    public LayerMask playerLayer;
    public float moveSpeed;

    bool playerInRange = false;
    public float chaseTime;
    private bool chaseStart = false;
    private float chaseBegin;
    
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(chaseStart == false) playerInRange = Physics2D.OverlapCircle(transform.position, enemyVision, playerLayer);
        if (playerInRange)
        {
            chaseStart = true;
            chaseBegin = Time.time;
            playerInRange = false;
        }
        if (chaseStart) {
            if (Time.time < (chaseBegin + chaseTime) /*!hit player*/){
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(-20,10), moveSpeed * Time.deltaTime);
            }
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position,enemyVision);
    }
    

    
}
