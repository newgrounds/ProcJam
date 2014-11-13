using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
	
	private Animator animator;
	private SpriteRenderer renderer;
	public Animator pants;
	private float direction = 0;
	private Vector2 speed;
	private Tile currentTile;
	private float lastHeight = 0;
	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		renderer = this.GetComponent<SpriteRenderer> ();
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				speed = new Vector2 (.035f, .035f);
			} 
			else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
				speed = new Vector2 (-.035f, .035f);
			} else {
				direction = 2f;
				speed = new Vector2 (0, .05f);
			}
		}  else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				speed = new Vector2 (.035f, -.035f);
			} 
			else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
				speed = new Vector2 (-.035f, -.035f);
			} else {
				direction = 0f;
				speed = new Vector2 (0, -.05f);
			}
		} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			direction = 3f;
			speed = new Vector2 (-.05f, 0);
		} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			direction = 1f;
			speed = new Vector2 (.05f, 0);
		}
		
		transform.Translate (new Vector3 (speed.x/2f, speed.y/2f, 0));
		//rigidbody2D.AddForce(speed);// (new Vector3 (speed.x, speed.y, 0));
		speed *= .8f;
		animator.SetFloat ("Direction", direction);
		animator.SetFloat ("Speed", speed.magnitude);
		
		//pants.SetFloat ("Direction", direction);
		//pants.SetFloat ("Speed", speed.magnitude);
		//Debug.Log(speed.magnitude);
	}
	
	void OnTriggerStay2D(Collider2D collider) {

	}
	
	/*
	 * Collect books
	 */
	void OnTriggerEnter2D(Collider2D collider) {
				if (collider.CompareTag("Tile")) {
			
			//Debug.Log("tile");
			Tile t = collider.gameObject.GetComponent<Tile>();
			//Debug.Log(t.height);
			if(t.height < lastHeight){
				iTween.MoveBy(gameObject, iTween.Hash("time", .3f,"y", t.height - lastHeight, "easeType", "easeOutBounce"));
			}
			else if(t.height > lastHeight){
				iTween.MoveBy(gameObject, iTween.Hash("time", .3f,"y", t.height - lastHeight, "easeType", "easeInOutCubic"));
			}
				lastHeight = t.height;
				//iTween.MoveTo(this.gameObject, t.transform.position, .1f);
			
			//renderer.sortingOrder = t.sortingOrder + 1;
		}
		CheckBooks(collider);
	}
	
	void CheckBooks(Collider2D collider){
		if (collider.CompareTag("Book")) {
			Book book = collider.gameObject.GetComponent<Book>();
			char bookChar = book.bookChar;
			Debug.Log("collided with book with char: " + bookChar);
			Cryptogram crypto = GameObject.Find("Cryptogram").GetComponent<Cryptogram>();
			crypto.UnscrambleValue(bookChar);
			crypto.Scramble(crypto.message);
			GameObject.Destroy(book.gameObject);
		} else if (collider.CompareTag("Chest")) {
			Chest chest = collider.gameObject.GetComponent<Chest>();
			char chestChar = chest.storedChar;
			Debug.Log("opened chest with char: " + chestChar);
			
			Cryptogram crypto = GameObject.Find("Cryptogram").GetComponent<Cryptogram>();
			
			// open in correct order
			if (crypto.message[crypto.openChests].Equals(chestChar)) {
				chest.gameObject.GetComponent<Animator>().SetBool("open", true);
				crypto.openChests++;
			} else {
				crypto.CloseAllChests();
			}
		} else if (collider.CompareTag("Map")) {
			GameObject.Find("Minimap").GetComponent<Camera>().enabled = true;
			GameObject.Destroy(collider.gameObject);
			Debug.Log("entered map");
		}	
	}
}
