using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

	// public GameObject[] players;		// gets access to every player GameObject instance in game
	public float timeElapsed;
	public Tile[] tiles;
	public int symbolToDestroy;

	// Use this for initialization
	void Start () {
		timeElapsed = 0;
		symbolToDestroy = -1;

		GameObject[] tileObjects = GameObject.FindGameObjectsWithTag ("Tile");
		tiles = new Tile[tileObjects.Length];
		for (int i = 0; i < tileObjects.Length; i++) {
			tiles [i] = tileObjects [i].GetComponent<Tile> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		UpdateTiles ();

		// if 3 minutes for a round is up
		if (timeElapsed >= 180) {
			print ("[END-GAME] Game Over.");
		} else if (timeElapsed > 0 && Mathf.FloorToInt(timeElapsed) % 30 == 0) {
			symbolToDestroy = Random.Range (0, 4); // 5 different symbols for now: {0, 1, 2, 3, 4}
		}
	}

	// TODO: update tiles as player moves about in the map
	void UpdateTiles() {
		/*
		for (int i = 0; i < tiles.Length; i++) {
			for (int p = 0; p < players.Length; p++) {
				float pX = players[p].transform.position.x;
				float pY = players[p].transform.position.y;
				
				tiles[i].UpdateTileFromPlayerPosition(pX, pY);
			}
		}
		*/
	}

	void DestroyTiles(int symbolChosenBySystem) {
		print ("[DESTROY TILE] Destroying tiles with symbol " + symbolChosenBySystem);
		/*
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].DestroyTileWithSymbol (symbolChosenBySystem);
		}
		*/
	}
}
