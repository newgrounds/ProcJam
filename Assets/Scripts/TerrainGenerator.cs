using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {
	
	public static List<Tile> tiles = new List<Tile>();

	// Use this for initialization
	void Start () {
		int mapWidth = 50;
		for(int x = 0; x < mapWidth; x++){
			for(int y = 0; y < mapWidth; y++){
				float height = SimplexNoise.Noise.Generate(x/12f,y/12f)/2f;
				float height2 = SimplexNoise.Noise.Generate(x+1000/8f,y+1000/8f)/2f;
				float height3 = SimplexNoise.Noise.Generate(x+400/15f,y+400/15f)/2f;
				
				
				GameObject tile = Instantiate(Resources.Load("tile")) as GameObject;

			 	tile.transform.position = new Vector3(transform.position.x + x *.5f,transform.position.y - y *.5f + height, 0);
				tile.GetComponent<SpriteRenderer>().sortingOrder = y;
				tile.GetComponent<Tile>().sortingOrder = y;
				tile.GetComponent<Tile>().height = height;
				tile.GetComponent<SpriteRenderer>().color = new Color(1f + height,1f + height,1f + height, 1);
				
				tiles.Add(tile.GetComponent<Tile>());
				
				/*
				if(height3 > .01f){
				tile = Instantiate(Resources.Load("tile")) as GameObject;
				tile.GetComponent<SpriteRenderer>().color = new Color(1f + height,1f + height,1f + height, 1);
				tile.GetComponent<Tile>().height = height;
			 	tile.transform.position = new Vector3(transform.position.x + x *.5f,transform.position.y - y *.5f + height, 0);
					
					
				}
				else{
				tile = Instantiate(Resources.Load("water")) as GameObject;
			 	tile.transform.position = new Vector3(transform.position.x + x *.5f,transform.position.y - y *.5f, 0);
					
					
				}
				
				
				tile.GetComponent<SpriteRenderer>().sortingOrder = y;
				tile.GetComponent<Tile>().sortingOrder = y;
				*/
				
				if(height2 > .3f){
					float xOffset = 0;//Random.Range(-.5f, .5f) ;
					GameObject tree = Instantiate(Resources.Load("pine")) as GameObject;
					tree.transform.localScale = new Vector3(1f+ Random.Range(-.5f, .5f),1f + Random.Range(-.5f, .5f),1f);
				 	tree.transform.position = new Vector3(xOffset + transform.position.x + x *.5f,.7f + transform.position.y - y *.5f + height, 0);
					tree.GetComponent<SpriteRenderer>().sortingOrder = y + 1;	
					tree.GetComponent<SpriteRenderer>().color = new Color(1 + Random.Range(-.25f,0),1 + Random.Range(-.25f,0),Random.Range(0f,.3f),1);	
					
				}
				else if(height > .25f){
					GameObject tree;
					if(Random.Range(0,10) > 8){
					    tree = Instantiate(Resources.Load("deadTree")) as GameObject;
					}
					else{
					    tree = Instantiate(Resources.Load("tree")) as GameObject;
						tree.GetComponent<SpriteRenderer>().color = new Color(1 + Random.Range(-.25f,0),1 + Random.Range(-.25f,0),Random.Range(0f,.3f),1);	
						
					}
					tree.transform.localScale = new Vector3(1f,1f + Random.Range(-.5f, .5f),1f);
				 	tree.transform.position = new Vector3(Random.Range(-.5f, .5f) + transform.position.x + x *.5f,.7f + transform.position.y - y *.5f + height, 0);
					tree.GetComponent<SpriteRenderer>().sortingOrder = y + 1;	
					
				}
				else if(height > .2f){
					GameObject grass = Instantiate(Resources.Load("grass")) as GameObject;
				 	grass.transform.position = new Vector3(transform.position.x + x *.5f,.7f + transform.position.y - y *.5f + height, 0);
					grass.GetComponent<SpriteRenderer>().sortingOrder = y + 1;	
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
