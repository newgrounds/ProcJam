using UnityEngine;

public class Chest : MonoBehaviour {
	public char storedChar = ' ';
	
	void OnGUI() {
		Vector3 p = Camera.main.WorldToScreenPoint(transform.position);
		GUI.color = Color.red;
		GUI.Label(
			new Rect(p.x - 25, Screen.height - p.y - 15, 50, 30),
			"<size=40>" + storedChar + "</size>"
		);
	}
}