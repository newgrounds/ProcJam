using UnityEngine;
using System.Collections;

public class RoofTile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay2D(Collider2D collider) {
		Roof parentRoof = this.transform.parent.GetComponent<Roof>();
		if (collider.CompareTag("Player") && !parentRoof.underRoof) {
			parentRoof.underRoof = true;
			iTween.ColorTo(this.transform.parent.gameObject, iTween.Hash("time", .5f, "easeType", "easeInOutCubic", "a", 0f));
		}
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		Roof parentRoof = this.transform.parent.GetComponent<Roof>();
		if (collider.CompareTag("Player") && parentRoof.underRoof) {
			parentRoof.underRoof = false;
			//iTween.ColorTo(this.transform.parent.gameObject, iTween.Hash("time", .5f, "easeType", "easeInOutCubic", "a", 1f));
		}
	}	
	
}
