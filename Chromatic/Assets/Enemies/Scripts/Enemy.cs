using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveXRight = 5f;
    [SerializeField] float moveXLeft = 5f;
    [SerializeField] float moveSpeed = 2f;
    [Header("case sensitive left or right")]
    [SerializeField] string direction = "left";

    Vector3 waypointRight;
    Vector3 waypointLeft;
    Vector3 moveDistanceRight; 
    Vector3 moveDistanceLeft; 
    private Rigidbody2D rb;

    // flying variables
    private bool flying = false;
    [Header("Adjust Flap Speed and Rigidbody gravity scale")]
    [SerializeField] float flapSpeed = 2.1f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        moveDistanceRight = new Vector3(moveXRight, 0, 0);
        moveDistanceLeft = new Vector3(moveXLeft, 0, 0);
        waypointRight = this.transform.position + moveDistanceRight;
        waypointLeft = this.transform.position - moveDistanceLeft;


        if (gameObject.tag == "Flying Enemy")
        {
            flying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < waypointRight.y - .5f && flying)
        {
            rb.velocity = new Vector2(0, flapSpeed);
        }

        if (transform.position.x < waypointRight.x && direction == "right")
        {
            MoveRight();

            if (transform.position.x >= waypointRight.x)
            {
                direction = "left";
            }
        }

        if (transform.position.x > waypointLeft.x && direction == "left")
        {
            MoveLeft();

            if (transform.position.x <= waypointLeft.x)
            {
                direction = "right";
            }
        }
    }

    private void MoveRight()
    {       
            var step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(waypointRight.x, transform.position.y, transform.position.z), step);

    }

    private void MoveLeft()
    {
            var step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(waypointLeft.x, transform.position.y, transform.position.z), step);
    }
}
