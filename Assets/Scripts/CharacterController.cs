using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	public int numCoins = 0;
	private Animator animator;
	private SpriteRenderer renderer;
	public Animator pants;
	public Animator shirt;
	public Animator hair;
	private float direction = 0;
	private Vector2 speed;
	private Tile currentTile;
	private float lastHeight = 0;
	private bool attacking = false;
	public bool isAttacking = false;
	public float speedMod;
	public bool canAttack;
	
	void Start () {
		animator = this.GetComponent<Animator> ();
		renderer = this.GetComponent<SpriteRenderer> ();
		float randTint = Random.Range(0,230);
		renderer.color = new Color((255 - randTint + 70)/255f, (255 - randTint + 35)/255f, (255 - randTint)/255f);
		speedMod = 3f;
		
	}
	
	void FixedUpdate () {
		if (canAttack && Input.GetKeyDown(KeyCode.Space) && speed.magnitude == 0 && !isAttacking) {
			attacking = true;
		}

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
		
		if(!attacking){
			transform.Translate (new Vector3 (speed.x/speedMod, speed.y/speedMod, 0));
		}
		//rigidbody2D.AddForce(speed);// (new Vector3 (speed.x, speed.y, 0));
		speed *= .8f;
		if (speed.magnitude <= .001f) {
			speed = Vector2.zero;
		}
		if (speed.magnitude <= .01f && !attacking) {
			animator.Play("Idle");
			pants.Play("Idle");
			shirt.Play("Idle");
			hair.Play("Idle");
		}
		if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
		   // Avoid any reload.
		   isAttacking = true;
		} else if (isAttacking) {
		    isAttacking = false;
			attacking = false;
		    // You have just leaved your state!
		}
		
		// fix character sorting order
		foreach(TerrainChunk c in TerrainGenerator.spawnedChunks) {
			foreach(Tile t in c.tiles){
				if(Vector3.Distance(t.transform.position + (Vector3.up * 0.5f), transform.position) < .25f){
					hair.GetComponent<SpriteRenderer>().sortingOrder = t.sortingOrder + 2;
					pants.GetComponent<SpriteRenderer>().sortingOrder = t.sortingOrder + 2;
					shirt.GetComponent<SpriteRenderer>().sortingOrder = t.sortingOrder + 2;
					renderer.sortingOrder = t.sortingOrder + 2;
				}
			}
		}
		animator.SetFloat ("Direction", direction);
		animator.SetFloat ("Speed", speed.magnitude* 100f);
		//Debug.Log(speed.magnitude);
		animator.SetBool ("Attacking", attacking);
		
		pants.SetFloat ("Direction", direction);
		pants.SetFloat ("Speed", speed.magnitude* 100f);
		pants.SetBool ("Attacking", attacking);
		
		shirt.SetFloat ("Direction", direction);
		shirt.SetFloat ("Speed", speed.magnitude* 100f);
		shirt.SetBool ("Attacking", attacking);
		
		hair.SetFloat ("Direction", direction);
		hair.SetFloat ("Speed", speed.magnitude* 100f);
		hair.SetBool ("Attacking", attacking);
		
		//pants.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
		//Debug.Log(speed.magnitude);
	}
	
	void OnTriggerStay2D(Collider2D collider) {

	}
	
	/*
	 * Collect books
	 */
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("Coin")) {
			GameObject.Destroy(collider.gameObject);
			numCoins++;
		}
		/*
		if (collider.CompareTag("Tile")) {
			
			//Debug.Log("tile");
			//Debug.Log(t.height);
			if(t.height < lastHeight){
				//iTween.MoveBy(gameObject, iTween.Hash("time", .3f,"y", t.height - lastHeight, "easeType", "easeOutBounce"));
			}
			else if(t.height > lastHeight){
				//iTween.MoveBy(gameObject, iTween.Hash("time", .3f,"y", t.height - lastHeight, "easeType", "easeInOutCubic"));
			}
				lastHeight = t.height;
				//iTween.MoveTo(this.gameObject, t.transform.position, .1f);
			Tile t = collider.gameObject.GetComponent<Tile>();
			
		}
		*/
		CheckCollectables(collider);
	}
	
	void CheckCollectables(Collider2D collider) {
		if (collider.CompareTag("Book")) {
			Book book = collider.gameObject.GetComponent<Book>();
			char bookChar = book.bookChar;
			//Debug.Log("collided with book with char: " + bookChar);
			
			Cryptogram crypto = GameObject.Find("Cryptogram").GetComponent<Cryptogram>();
			crypto.UnscrambleValue(bookChar);
			crypto.Scramble(crypto.message);
			GameObject.Destroy(book.gameObject);
		} else if (collider.CompareTag("Chest")) {
			Chest chest = collider.gameObject.GetComponent<Chest>();
			char chestChar = chest.storedChar;
			//Debug.Log("opened chest with char: " + chestChar);
			
			Cryptogram crypto = GameObject.Find("Cryptogram").GetComponent<Cryptogram>();
			
			// if this one is already open
			if (chest.gameObject.GetComponent<Animator>().GetBool("open")) {
				// do nothing
			}
			// open in correct order
			else if (crypto.message[crypto.openChests].Equals(chestChar)) {
				chest.gameObject.GetComponent<Animator>().SetBool("open", true);
				crypto.openChests++;
			} else {
				crypto.CloseAllChests();
			}
		}
		// collect the map
		else if (collider.CompareTag("Map")) {
			GameObject.Find("Minimap").GetComponent<Camera>().enabled = true;
			GameObject.Destroy(collider.gameObject);
		}
		// collect the boots
		else if (collider.CompareTag("Boots")) {
			speedMod -= 0.5f;
			GameObject.Destroy(collider.gameObject);
		}
		// collect the better boots
		else if (collider.CompareTag("BetterBoots")) {
			speedMod -= 0.5f;
			GameObject.Destroy(collider.gameObject);
		}
		// collect the attack
		else if (collider.CompareTag("AttackEnabler")) {
			canAttack = true;
			GameObject.Destroy(collider.gameObject);
		}
	}
	
	void OnGUI() {
		GUI.Box(new Rect(Screen.width - 250, 25, 200, 75), "<size=40>Coins: " + numCoins + "</size>");
	}
}
