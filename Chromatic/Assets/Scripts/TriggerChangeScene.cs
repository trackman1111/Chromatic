using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChangeScene : MonoBehaviour
{
    //Apply this script to the Detection box
    //The player must have the Player tag for this to work
    //SceneName is the scene you want to change to, and CurrentScene simply stores the current scene you are in (It's unnecessary, but may be useful later).
    public string SceneName;
    string CurrentScene;


    // Start is called before the first frame update
    void Start()
    {

        CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //NOTE TO SELF: Box CANNOT be have Trigger checked.
        if (collision.gameObject.tag == "Player")
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {


        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);


    }



}
