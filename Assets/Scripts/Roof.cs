using UnityEngine;
using System.Collections;

public class Roof : MonoBehaviour {

	public bool underRoof = false;
	private int timer = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(!underRoof && timer > 10){
			underRoof = false;
			iTween.ColorTo(this.gameObject, iTween.Hash("time", .5f, "easeType", "easeInOutCubic", "a", 1f));	
		}
	}
}
