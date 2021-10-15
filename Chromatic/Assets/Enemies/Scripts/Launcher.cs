using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float timeBetweenShots = 2f;
    float shotCounter;
    private GameObject mainCharacter;

    void Start()
    {
        mainCharacter = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            if (!mainCharacter)
            {
                return;
            }
            Fire();
            shotCounter = timeBetweenShots;
        }
    }

    private void Fire()
    {
        print("test1");
        GameObject launchedProjectile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        print("test2");
        // Shoot Right
        if (gameObject.transform.position.x > mainCharacter.transform.position.x)
        {
            print("test3");
            launchedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
            print("test4");
        }
        // Shoot Left
        if (this.transform.position.x < mainCharacter.transform.position.x)
        {
            launchedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        }
    }
}
