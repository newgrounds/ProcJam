using UnityEngine;

public class Minimap : MonoBehaviour {
	Camera cam;
	
	public float targetHeight = 0.4f;
	
	void Start() {
		cam = GetComponent<Camera>();
		float screenAspect = (float)Screen.width / (float)Screen.height;
		cam.orthographicSize = (TerrainChunk.mapWidth * TerrainChunk.tileSize * 1.5f) / (screenAspect);
		float viewX = targetHeight / screenAspect;
		cam.rect = new Rect(1f - viewX, 0, viewX, targetHeight);
	}
	
	public static Bounds OrthographicBounds(Camera camera) {
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		return new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0)
		);
	}
}
