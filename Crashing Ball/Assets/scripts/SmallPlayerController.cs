using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class SmallPlayerController : MonoBehaviour {

    public Rigidbody2D player;
    float velocity;


    void Start()
    {
        player = GetComponent<Rigidbody2D>();

        velocity = 6;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            player.velocity += new Vector2(2, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            player.velocity += new Vector2(-2, 0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && this.GetComponent<CollisionController>().isTouchingFloor == 1)
        {
            player.velocity += new Vector2(0, velocity);
        }
        
    }

    

}
