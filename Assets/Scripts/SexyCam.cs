using UnityEngine;

public class SexyCam : MonoBehaviour {
	public Transform target;
	public float smooth;
	
	void Update() {
		Vector3 newPos = Vector3.Lerp(transform.position, target.position, Time.deltaTime * smooth);
		transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
	}
}