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

    void Start()
    {
        moveDistanceRight = new Vector3(moveXRight, 0, 0);
        moveDistanceLeft = new Vector3(moveXLeft, 0, 0);
        waypointRight = this.transform.position + moveDistanceRight;
        waypointLeft = this.transform.position - moveDistanceLeft;
    }

    // Update is called once per frame
    void Update()
    {
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
            transform.position = Vector2.MoveTowards(transform.position, waypointRight, step);

    }

    private void MoveLeft()
    {
            var step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, waypointLeft, step);
    }
}
