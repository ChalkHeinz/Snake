using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//https://noobtuts.com/unity/2d-snake-game used this to create the skeleton of the game http://theflyingkeyboard.net/unity/unity-ui-c-simple-main-menu/
public class Snake : MonoBehaviour
{
    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = new Vector2(0, 1);

    // Keep Track of Tail
    List<Transform> body = new List<Transform>();
    

    // Did the snake eat something?
    bool ate = false;

    //Prefabs
    public GameObject TailPrefab;
    public GameObject BodyPrefab;
    public GameObject tailTest;

    //Gameover prefab
    public GameObject GameOverPrefab;


    // Use this for initialization
    void Start()
    {
        // Move the Snake every 300ms
        InvokeRepeating("Move", 0.1f, 0.1f);

        Vector2 v = transform.position;

        tailTest = (GameObject)Instantiate(TailPrefab,
                                                  v,
                                                  Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Move in a new Direction?
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Go Right
            transform.Rotate(
                    0, 0, -2);
            }
            
        else if (Input.GetKey(KeyCode.LeftArrow))
            //Go Left
            transform.Rotate(
               0, 0, 2);
    }

    void Move()
    {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);


        //Gets head's rotation
        Transform tf = GetComponent<Transform>();

        // Ate something? Then insert new Element into gap
        if (ate)
        {
            // Load Prefab into the world
            GameObject g = (GameObject)Instantiate(BodyPrefab,
                                                  v,
                                                  Quaternion.identity);

            // Keep track of it in our tail list
            body.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }

        // Do we have a Tail?
        else if (body.Count > 0)
        {
            // Move last Tail Element to where the Head was
            body.Last().position = v;

            // Add to front of list, remove from the back
            body.Insert(0, body.Last());
            body.RemoveAt(body.Count - 1);

            tailTest.transform.position = body.Last().position;

            tailTest.transform.rotation = body.Last().rotation;
            body.Last().rotation = tf.rotation;

        }


    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("FoodPrefab"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the Food
            Destroy(coll.gameObject);
        }
        // Collided with Tail or Border
        if (coll.name.StartsWith("Border")){
            // GameOver Screen
            Instantiate(GameOverPrefab);
        }
    }
}