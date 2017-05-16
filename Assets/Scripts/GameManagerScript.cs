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
	public int numPlayersSurviving;

	public Text winText;
	public Text deathText;
	public Text timeText;
	public Text warningText;

	bool[] hasBeenAssignedId;

	int triggerTime;
	bool destroyedEarlier;
	bool isGenerated;
	int prevGeneratedSymbol;

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
		warningText.enabled = false;

		numPlayersSurviving = numPlayers;
		prevGeneratedSymbol = -1;

		gameOvers = new bool[numPlayers];
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (numPlayersSurviving > 1) {
			timeElapsed += Time.deltaTime;

			timeText.text = "Time: " + Mathf.FloorToInt (timeElapsed / 60) + ":" + Mathf.FloorToInt (timeElapsed % 60);

			// if 3 minutes for a round is up
			if (timeElapsed >= (roundDuration * 60f)) {
				if (numPlayersSurviving == 1) {
					winText.text = "Game Won!";
					winText.enabled = true;
				} else if (numPlayersSurviving <= 0) {
					winText.text = "Game Over.";
					winText.enabled = true;
				} else {
					winText.text = "Game Draw~";
					winText.enabled = true;
				}
			} else if (Mathf.FloorToInt (timeElapsed) > 0 && Mathf.FloorToInt (timeElapsed) % 30 == 27 && !isGenerated) {
				while (symbolToDestroy == prevGeneratedSymbol) {
					symbolToDestroy = Random.Range (0, 4); // 5 different symbols for now: {0, 1, 2, 3, 4}
				}
				isGenerated = true;
				warningText.text = "Destroying tiles with symbol " + symbolToDestroy + " in 3...";
				warningText.enabled = true;
			} else if (Mathf.FloorToInt (timeElapsed) > 0 && Mathf.FloorToInt (timeElapsed) % 30 == 28) {
				warningText.text = "Destroying tiles with symbol " + symbolToDestroy + " in 2...";
			} else if (Mathf.FloorToInt (timeElapsed) > 0 && Mathf.FloorToInt (timeElapsed) % 30 == 29) {
				warningText.text = "Destroying tiles with symbol " + symbolToDestroy + " in 1...";
				warningText.enabled = true;
			} else if (Mathf.FloorToInt (timeElapsed) > 0 && Mathf.FloorToInt (timeElapsed) % 30 == 0 && !destroyedEarlier) {
				warningText.enabled = false;
				isGenerated = false;
				DestroyTiles (symbolToDestroy);
				triggerTime = Mathf.FloorToInt (timeElapsed);
				destroyedEarlier = true;
				prevGeneratedSymbol = symbolToDestroy;
			} else if (Mathf.FloorToInt (timeElapsed) >= (triggerTime + 1)) {
				destroyedEarlier = false;
			}
		}
	}

	void DestroyTiles(int symbolChosenBySystem) {
		print ("[DESTROY TILE] Destroying tiles with symbol " + symbolChosenBySystem);
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].DestroyTileWithSymbol (symbolChosenBySystem);
		}
	}

	bool IsGameOver() {
		return numPlayersSurviving == 1;
	}
}
