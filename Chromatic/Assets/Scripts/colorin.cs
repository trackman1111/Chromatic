using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorin : MonoBehaviour
{
    //Press E to fill in the color
    //The player will be moved to a position (the trigger box of the public GameObject 'moveto'), and then the shape will be filled in.
    //Components needed: An object with a trigger box (this object). A child object with a trigger box (the 'moveto' object. Will also have the 'detectcolorin' script). A child object (the object that is colored in, and the 'colored object' object.)

    private Vector2 pos;
    private Quaternion rot;
    private Vector2 playerpos;
    //triggerarea tests if the player is in the trigger area
    private bool triggerarea;
    //done tests if the shape has been filled in (so that the script can only run once)
    private bool done;
    //filling tests if player is in the act of filling and has pressed E
    private bool filling;
    private Vector2 movepos;
    private Rigidbody2D player;
    private float gravscale;
    private float distance_x;
    private Vector2 lastplayerpos;
    private float counting;

    public GameObject coloredobject;
    public float movespeed = 1;
    public GameObject moveto;
    //moved will be inputted from the 'detectcolorin' script placed in a seperate object with a trigger. moved tests if the player is in the trigger area of where it is moving to.
    public bool moved = false;

    //To disable Player movement (requires changes to main character script to implement)
    public bool playercontrol;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;

        //Because will later divide by movespeed
        movespeed = movespeed / 10;
        movepos = moveto.GetComponent<Transform>().position;

        filling = false;
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (done == false)
        {
            //This if statement fixes annoying error messages
            if (player != null)
            {
                playerpos = player.position;
            }


            if (triggerarea == true && Input.GetKeyDown(KeyCode.E))
            {
                filling = true;
            }

            if (filling == true && moved == false)
            {
                
                playercontrol = false;
                //using 0.9f*gravscale to help with friction issues
                player.velocity = (new Vector2(movespeed * distance_x, 0.2f*gravscale));

                //There was a weird glitch where sometimes the circle will just stop and the velocity won't move it or anything.
                //Because of this, I added the timer below. It basically just raises the position by 0.01f/s to update the position (you can't even tell because of gravity.)
                if (counting < (Time.time - 1))
                {
                    player.position = new Vector2(playerpos.x, playerpos.y + 0.01f);
                    counting = 0;
                }

                if (counting == 0)
                {
                    counting = Time.time;
                }


            }

            if (filling == true && moved == true)
            {
                //
                player.velocity = Vector2.zero;
                coloredobject.transform.position = pos;
                coloredobject.transform.rotation = rot;
                playercontrol = true;
                done = true;

            }


        }

    }



    //OnTriggerStay2D is checked randomly so using bool for Enter and exit

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<Rigidbody2D>();
            gravscale = player.gravityScale;
            triggerarea = true;
            distance_x = movepos.x - playerpos.x;
            distance_x = AddSpeed(distance_x);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerarea = false;
        }
    }

    private float AddSpeed(float distance)
    {
        if (distance < 0)
        {
            distance = distance - 1;
        }
        if (distance > 0)
        {
            distance = distance + 1;
        }

        return distance;
    }

}
 
