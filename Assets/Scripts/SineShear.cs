using UnityEngine;
using System.Collections;

public class SineShear : MonoBehaviour {
	
	private int timer = Random.Range(0,100);
	private int timer2 = Random.Range(100, 200);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		timer2++;
		this.transform.localScale = new Vector3(.8f + Mathf.Sin(timer/50f)/10f, .8f + Mathf.Sin(timer2/50f)/10f, 0);
	}
}
