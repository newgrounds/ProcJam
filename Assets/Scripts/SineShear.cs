using UnityEngine;
using System.Collections;

public class SineShear : MonoBehaviour {
	
	private int timer = Random.Range(0,100);
	private int timer2 = Random.Range(100, 200);
	private float randScale;
	// Use this for initialization
	void Start () {
		randScale = Random.Range(1f,1.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer++;
		timer2++;
		this.transform.localScale = new Vector3(randScale + Mathf.Sin(timer/50f)/10f, randScale + Mathf.Sin(timer2/50f)/10f, 0);
	}
}
