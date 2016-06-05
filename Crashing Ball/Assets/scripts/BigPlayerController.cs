using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BigPlayerController : MonoBehaviour {

	public Rigidbody2D player;
	float velocity;
    float force;

    void Start () {
		player = GetComponent<Rigidbody2D> ();
        //force = 400;
        velocity = 8;
    }
	

	void Update () {
		if (Input.GetKeyDown ("d")) {
            //player.AddForce(new Vector2(force,0));
            player.velocity += new Vector2(4,0);
        }
        else if (Input.GetKeyDown ("a")) {
            //player.AddForce(new Vector2(-force, 0));
            player.velocity += new Vector2(-4,0);
        }

        else if (Input.GetKeyDown ("w") && this.GetComponent<CollisionController>().isTouchingFloor == 1) {
            player.velocity += new Vector2(0, velocity);
        }

        
	}

    
}
