using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackpos;
    public float attackRange;
    public LayerMask whatisEnemies;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.F))
            {
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackpos.position, attackRange, whatisEnemies);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Health>().ProcessHit(damage);

                    }

                    timeBtwAttack = startTimeBtwAttack;
                    Debug.Log("MAIN CHAR ATTACKED");
                }
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpos.position, attackRange);
    }
}
