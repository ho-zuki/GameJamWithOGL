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
    private float nextShot = 0.0f;

    public float shootRate = 0.3f;

    public LayerMask targetLayer;
    
    public float bulletSpeed;

    bool playerInRange;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, atkRange, targetLayer);
        if (playerInRange && Time.time>nextShot)
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                if (Time.time > nextShot)
                {
                    Instantiate(bullet, shooter.transform.position, Quaternion.identity);
                    nextShot += shootRate;
                }
            }
            nextShot += cooldown;
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, atkRange);
    }



}
