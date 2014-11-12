using UnityEngine;

public class Book : MonoBehaviour {
	public char bookChar = ' ';
	
	void OnGUI() {
		Vector3 p = Camera.main.WorldToScreenPoint(transform.position);
		GUI.color = Color.red;
		GUI.Label(
			new Rect(p.x - 15, Screen.height - p.y - 30, 50, 30),
			"<size=40>" + bookChar + "</size>"
		);
	}
}