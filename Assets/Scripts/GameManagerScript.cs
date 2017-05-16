using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public float timeElapsed;
	public Tile[] tiles;
	public int symbolToDestroy;
	public float roundDuration;			// note: this is in minutes

	public int numPlayers;

	public Text winText;
	public Text deathText;

	bool[] hasBeenAssignedId;
	int symbolToAssign;

	int triggerTime;
	bool destroyedEarlier;

	public bool[] gameOvers;

	int[] symbols = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					  2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					  3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
					  4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4
				    };

	int[] indices = { 16, 20, 9, 65, 85, 14, 47, 33, 10, 58, 7, 80, 89, 94, 13, 79, 86, 21, 8, 38,
					  53, 1, 74, 32, 49, 15, 48, 78, 18, 67, 28, 5, 30, 92, 31, 63, 73, 42, 6, 2,
					  83, 44, 36, 50, 64, 88, 22, 81, 0, 17, 99, 60, 55, 52, 72, 91, 96, 35, 26, 75,
					  56, 87, 82, 39, 40, 27, 57, 84, 76, 43, 95, 93, 37, 66, 25, 11, 19, 62, 29, 54,
					  61, 77, 24, 34, 45, 97, 46, 41, 70, 4, 59, 3, 12, 90, 68, 71, 69, 51, 23, 98
					};

	// Use this for initialization
	void Start () {
		timeElapsed = 0;
		symbolToDestroy = -1;

		GameObject[] tileObjects = GameObject.FindGameObjectsWithTag ("Tile");
		tiles = new Tile[tileObjects.Length];

		for (int i = 0; i < tileObjects.Length; i++) {
			tiles [i] = tileObjects [i].GetComponent<Tile> ();
		}

		for (int i = 0; i < tileObjects.Length; i++) {
			tiles [indices[i]].symbol = symbols[i];
		}

		winText.enabled = false;

		gameOvers = new bool[numPlayers];
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;

		if (IsGameOver ()) {
			winText.text = "Game Over.";
			winText.enabled = true;
		}
		// if 3 minutes for a round is up
		else if (timeElapsed >= (roundDuration * 60f)) {
			if (!deathText.enabled) {
				winText.text = "You win!";
				winText.enabled = true;
			} else {
				winText.text = "Game Over.";
				winText.enabled = true;
			}
		} else if (Mathf.FloorToInt (timeElapsed) > 0 && Mathf.FloorToInt (timeElapsed) % 30 == 0 && !destroyedEarlier) {
			symbolToDestroy = Random.Range (0, 4); // 5 different symbols for now: {0, 1, 2, 3, 4}
			DestroyTiles (symbolToDestroy);
			triggerTime = Mathf.FloorToInt (timeElapsed);
			destroyedEarlier = true;
		} else if (Mathf.FloorToInt (timeElapsed) >= (triggerTime + 1)) {
			destroyedEarlier = false;
		}
	}

	void DestroyTiles(int symbolChosenBySystem) {
		print ("[DESTROY TILE] Destroying tiles with symbol " + symbolChosenBySystem);
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].DestroyTileWithSymbol (symbolChosenBySystem);
		}
	}

	bool IsGameOver() {
		for (int i = 0; i < gameOvers.Length; i++) {
			if (gameOvers [i] == false) {
				return false;
			}
		}
		return true;
	}
}
