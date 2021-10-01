using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.GetComponent<Health>().getIdentifier() 
        //== gameObject.GetComponent<DamageDealer>().getIdentifier())
       // {
        //    return;
        //}
        Destroy(gameObject);
    }
}
