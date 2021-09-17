using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int enemyValue = 150;
    [SerializeField] float movementSpeed = 5f;
    [Header("Enemy Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [Header("Enemy Sounds")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float explosionDuration = 1;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathVolume = .75f;
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0,1)] float laserVolume = .75f;
    
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots); // creates randomness in shots
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
        Move();
    }

    // Shoot on an interval
    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    // Spawn a projectile, give it a velocity, and play an instantiation sound
    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        
        if(!damageDealer)
        {
            movementSpeed = movementSpeed * -1;
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // FindObjectOfType<GameSession>().AddToScore(enemyValue); just used to add to score
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume); // play death sound
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation); // play explosion vfx
        Destroy(explosion, explosionDuration); // delete explosion vfx
    }

    private void Move()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, 0);
    }
}
