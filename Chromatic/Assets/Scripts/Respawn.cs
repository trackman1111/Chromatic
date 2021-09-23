using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    Vector2 position;
    Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {

        position = transform.position;
        rotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y <= -30)
        {

            transform.position = position;
            transform.rotation = rotation;
        
        }



    }





}
