using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] string identifier = "enemy";

    public int GetDamage()
    {
        return damage;
    }

    public string GetIdentifier()
    {
        return identifier;
    }
}
