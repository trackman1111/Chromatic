using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("case sensitive left or right")]
    [SerializeField] string direction = "left";
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float timeBetweenShots = 2f;
    float shotCounter;

    // Start is called before the first frame update
    void Start()
    {
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = timeBetweenShots;
        }
    }

    private void Fire()
    {
        GameObject launchedProjectile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        if (direction == "left")
        {
            launchedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
        }
        if (direction == "right")
        {
            launchedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        }
        // AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserVolume);
    }
}
