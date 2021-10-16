using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorin : MonoBehaviour
{
    //Press E to fill in the color
    //collision.attachedRigidbody.velocity = new Vector2(0, 0);

    private Vector2 pos;
    private Quaternion rot;
    private float BoxSize;
    private float BoxOffset;
    private Vector2 playerpos;
    private bool triggerarea;
    private bool done;
    private bool filling;
    private Vector2 movepos;
    private Rigidbody2D player;

    public GameObject coloredobject;
    public float movespeed;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;
        BoxSize = gameObject.GetComponent<BoxCollider2D>().size.x;
        BoxOffset = gameObject.GetComponent<BoxCollider2D>().offset.x;

        //Because will later divide by movespeed
        movespeed = movespeed / 10;
        movepos = gameObject.GetComponentInChildren<Transform>().position;

        filling = false;
        done = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (triggerarea == true && Input.GetKeyDown(KeyCode.E))
        {
            while (playerpos != movepos)
            {

                ColorFillIn(player);

            }
        }

    }



    //OnTriggerStay2D is checked randomly so using bool for Enter and exit

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<Rigidbody2D>();
            triggerarea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerarea = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {



        if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {

            print("test");
            coloredobject.transform.position = pos;
            coloredobject.transform.rotation = rot;
            //ColorFillIn(collision);

        }

        if (playerpos == movepos && filling == true && done == false)
        {

            //Moves the colored object to the dotted-line object
            coloredobject.transform.position = pos;
            coloredobject.transform.rotation = rot;

            //So that OnTriggerStay2D won't test for pressing E anymore
            done = true;

        }

    }

    private void ColorFillIn(Rigidbody2D player)
    {
        Vector2 playerpos;
        float distance_x;
        float distance_y;

        playerpos = player.GetComponent<Transform>().position;

        //distances
        distance_x = movepos.x - playerpos.x;
        distance_y = movepos.y - playerpos.y;

        player.transform.position = playerpos + (new Vector2 (distance_x*movespeed,distance_y*movespeed));
        filling = true;

    }

}
 
