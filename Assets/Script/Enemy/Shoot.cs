using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Enemy Shoot action
If target get in shooter range 
-> shot at an interval of shootRate seconds 
    with shootAmount bullets
*/
public class Shoot : MonoBehaviour
{
    public GameObject target;
    public GameObject shooter;
    public GameObject bullet;

    public float atkRange;
    public int bulletAmount;
    public float cooldown;
   
    public float shootRate = 0.3f;

    public LayerMask targetLayer;
    
    public float bulletSpeed;

    bool playerInRange;
    private float nextShot = 0.0f;
    bool takeShot;
    int bulletCount;
    Vector2 targetPos;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, atkRange, targetLayer);
        if (playerInRange && Time.time > nextShot)
        {
            if (Time.time > nextShot)
            {
                takeShot = true;
                targetPos = target.transform.position;
            }
            if (takeShot && bulletCount < bulletAmount)
            {
                Instantiate(bullet, shooter.transform.position, Quaternion.identity);
                nextShot += shootRate;
                bulletCount++;
                takeShot = false;
            }
            if (bulletCount >= bulletAmount)
            {
                nextShot += cooldown;
            }
            
        }
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, atkRange);
    }



}
