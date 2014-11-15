using UnityEngine;
using System.Collections.Generic;

public class Shopkeep : MonoBehaviour {
	bool playerInRange;
	int numPurchased;
	List<Purchasable> itemsForSale = new List<Purchasable>();
	CharacterController controller;
	
	public GameObject mapObj;
	public GameObject bootsObj;
	public GameObject swordObj;
	public GameObject keyObj;
	public GameObject betterBootsObj;
	
	void Start() {
		playerInRange = false;
		numPurchased = 0;
		// maybe make the coin counter free
		itemsForSale.Add(new Purchasable("map", 10, mapObj));
		itemsForSale.Add(new Purchasable("boots", 50, bootsObj));
		//itemsForSale.Add(new Purchasable("sword", 100, swordObj));
		//itemsForSale.Add(new Purchasable("skeleton key", 200, keyObj));
		itemsForSale.Add(new Purchasable("better boots", 300, betterBootsObj));
		//itemsForSale.Add(new Purchasable("", 400, ));
		// fast travel
		//itemsForSale.Add(new Purchasable("", 500, ));
		
		controller = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<CharacterController>();
	}
	
	void Purchase() {
		// charge player
		controller.numCoins -= itemsForSale[numPurchased].cost;
		
		// spawn purchased item
		Instantiate(itemsForSale[numPurchased].obj, transform.position, Quaternion.identity);
		
		// increment number of purchased items
		numPurchased++;
		
		// don't respawn
		if (numPurchased >= itemsForSale.Count) {
			Destroy(gameObject);
		}
		// spawn somewhere random
		else {
			Tile t = TerrainGenerator.tilesWithoutDecals[Random.Range(0, TerrainGenerator.tilesWithoutDecals.Count)];
			transform.position = t.transform.position;
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			playerInRange = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			playerInRange = false;
		}
	}
	
	void OnGUI() {
		if (playerInRange) {
			GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300));
			GUI.Box(new Rect(0, 0, 300, 200), "");
			GUI.Label(
				new Rect(50, 20, 200, 100),
				"<size=20>Hey there! Buy the " + itemsForSale[numPurchased].name
				+ " for " + itemsForSale[numPurchased].cost + " coins?</size>"
			);
			if (controller.numCoins >= itemsForSale[numPurchased].cost) {
				if (GUI.Button(new Rect(100, 120, 100, 50), "Buy")) {
					Purchase();
				}
			} else {
				GUI.Button(new Rect(75, 120, 150, 50), "Need more coins");
			}
			GUI.EndGroup();
		}
	}
}

public class Purchasable {
	public string name;
	public int cost;
	public GameObject obj;
	
	public Purchasable(string n, int c, GameObject o) {
		name = n;
		cost = c;
		obj = o;
	}
}