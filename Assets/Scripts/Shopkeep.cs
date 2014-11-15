using UnityEngine;
using System.Collections.Generic;

public class Shopkeep : MonoBehaviour {
	bool playerInRange;
	int numPurchased;
	List<Purchasable> itemsForSale = new List<Purchasable>();
	
	void Start() {
		playerInRange = false;
		numPurchased = 0;
		itemsForSale.Add(new Purchasable("map", 10));
	}
	
	bool AttemptPurchase() {
		numPurchased++;
		return false;
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
			GUI.EndGroup();
		}
	}
}

public class Purchasable {
	public string name;
	public int cost;
	
	public Purchasable(string n, int c) {
		name = n;
		cost = c;
	}
}