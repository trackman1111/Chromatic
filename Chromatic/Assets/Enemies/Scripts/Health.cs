using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] string identifier = "enemy";

    void OnCollisionEnter2D(Collision2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if(!damageDealer || identifier == damageDealer.getIdentifier())
        {
            return;
        }
        
        ProcessHit(damageDealer.GetDamage());
    }

    public void ProcessHit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        else
        {
            return;
        }
    }

    public string getIdentifier()
    {
        return identifier;
    }
}
