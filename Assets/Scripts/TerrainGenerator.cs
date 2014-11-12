using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int mapWidth = 50;
		for(int x = 0; x < mapWidth; x++){
			for(int y = 0; y < mapWidth; y++){
				GameObject tile = Instantiate(Resources.Load("tile")) as GameObject;
				float height = SimplexNoise.Noise.Generate(x/12f,y/12f)/2f;
			 	tile.transform.position = new Vector3(transform.position.x + x *.5f,transform.position.y - y *.5f + height, 0);
				tile.GetComponent<SpriteRenderer>().sortingOrder = y;
				tile.GetComponent<Tile>().sortingOrder = y;
				tile.GetComponent<Tile>().height = height;
			}
			 
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
