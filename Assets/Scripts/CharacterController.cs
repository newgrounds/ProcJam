﻿using UnityEngine;
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
	public bool underRoof = false;
	public float stamina;
	public float MAX_STAMINA = 100f;
	public float health;
	public float MAX_HEALTH = 100f;
	public GameObject gui;
	
	void Start () {
		Time.timeScale = 1f;
		gui.active = false;
		
		stamina = MAX_STAMINA;
		health = MAX_HEALTH;
		animator = this.GetComponent<Animator> ();
		renderer = this.GetComponent<SpriteRenderer> ();
		float randTint = Random.Range(0,230);
		renderer.color = new Color((255 - randTint + 70)/255f, (255 - randTint + 35)/255f, (255 - randTint)/255f);
		speedMod = 3f;
		pants.gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255f)/255f, Random.Range(0, 255f)/255f, Random.Range(0, 255f)/255f);
		shirt.gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255f)/255f, Random.Range(0, 255f)/255f, Random.Range(0, 255f)/255f);
		hair.gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255f)/255f, Random.Range(0, 255f)/255f, Random.Range(0, 255f)/255f);
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
		
		DecreaseStamina(Time.deltaTime / 2f);
	}
	
	public void IncreaseStamina(float amount) {
		stamina += amount;
		
		// stamina can't go above the max
		if (stamina > MAX_STAMINA) {
			stamina = MAX_STAMINA;
		}
	}
	public void DecreaseStamina(float amount) {
		stamina -= amount;
		
		// stamina can't go below 0
		if (stamina < 0) {
			stamina = 0;
		}
		
		// decrease health if stamina is at 0
		if (stamina == 0) {
			DecreaseHealth(amount);
		}
	}
	
	public void IncreaseHealth(float amount) {
		health += amount;
		
		// health can't go above the max
		if (health > MAX_HEALTH) {
			health = MAX_HEALTH;
		}
	}
	public void DecreaseHealth(float amount) {
		health -= amount;
		
		// health below 0 = death
		if (health <= 0) {
			health = 0;
			Time.timeScale = 0;
			gui.active = true;
		}
	}
	
	/*
	 * Collect books
	 */
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("Coin")) {
			collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
			iTween.MoveBy(collider.gameObject, iTween.Hash("time", 0.5f, "easeType", "easeInOutCubic", "loopType", "pingPong", "amount", Vector3.up / 3f));
			iTween.ColorTo(collider.gameObject, iTween.Hash("time", 1.5f, "easeType", "easeInOutCubic", "a", 0f));
			GameObject.Destroy(collider.gameObject, 1f);
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
		// collect the food
		else if (collider.CompareTag("Food")) {
			IncreaseStamina(10f);
			GameObject.Destroy(collider.gameObject);
		}
	}
	
	void OnGUI() {
		GUI.Box(new Rect(Screen.width - 250, 25, 200, 75), "<size=40>Coins: " + numCoins + "</size>");
		GUI.Box(new Rect(0, 10, 300, 70), "<size=40>Health: " + Mathf.Round(health) + "</size>");
		GUI.Box(new Rect(0, 80, 300, 70), "<size=40>Stamina: " + Mathf.Round(stamina) + "</size>");
	}
	
	public void RestartButtonClick() {
		Application.LoadLevel(0);
	}
}
