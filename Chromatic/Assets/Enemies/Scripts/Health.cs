using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] string identifier = "enemy";
    [Header("Only Effects Player")]
    [SerializeField] float immunityTimer = 2f;

    bool playerHit = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if(!damageDealer || identifier == damageDealer.GetIdentifier())
        {
            return;
        }
        ProcessHit(damageDealer.GetDamage());
    }

        void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if(!damageDealer || identifier == damageDealer.GetIdentifier())
        {
            return;
        }
        
        if (identifier == "enemy")
        {
            ProcessHit(damageDealer.GetDamage());
        }

        if(identifier == "player" && !playerHit)
        {
            playerHit = true;
            ProcessHit(damageDealer.GetDamage());
            StartCoroutine(PlayerImmunity());
            return;
        }

        if(identifier == "player" && playerHit)
        {
            print("playerCollision");
            return;
        }
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

    public string GetIdentifier()
    {
        return identifier;
    }

    IEnumerator PlayerImmunity()
    {
        yield return new WaitForSeconds(immunityTimer);
        playerHit = false;
    }
}
