using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
	
	private Animator animator;
	public Animator pants;
	private float direction = 0;
	private Vector2 speed;
	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.W)) {
			if (Input.GetKey (KeyCode.D)) {
				speed = new Vector2 (.035f, .035f);
			} 
			else if (Input.GetKey (KeyCode.A)) {
				speed = new Vector2 (-.035f, .035f);
			} else {
				direction = 2f;
				speed = new Vector2 (0, .05f);
			}
		}  else if (Input.GetKey (KeyCode.S)) {
			if (Input.GetKey (KeyCode.D)) {
				speed = new Vector2 (.035f, -.035f);
			} 
			else if (Input.GetKey (KeyCode.A)) {
				speed = new Vector2 (-.035f, -.035f);
			} else {
				direction = 0f;
				speed = new Vector2 (0, -.05f);
			}
		} else if (Input.GetKey (KeyCode.A)) {
			direction = 3f;
			speed = new Vector2 (-.05f, 0);
		} else if (Input.GetKey (KeyCode.D)) {
			direction = 1f;
			speed = new Vector2 (.05f, 0);
		}
		
		transform.Translate (new Vector3 (speed.x/2f, speed.y/2f, 0));
		//rigidbody2D.AddForce(speed);// (new Vector3 (speed.x, speed.y, 0));
		speed *= .8f;
		animator.SetFloat ("Direction", direction);
		animator.SetFloat ("Speed", speed.magnitude);
		pants.SetFloat ("Direction", direction);
		pants.SetFloat ("Speed", speed.magnitude);
		//Debug.Log(speed.magnitude);
	}
}
