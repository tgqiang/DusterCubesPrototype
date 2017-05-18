using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public float timeElapsed;
	public float roundDuration;			// note: this is in minutes

	public Tile[] tiles;
	public Tile[,] map;
	public int rowsRemaining = 10;		// set this to change # rows
	public int colsRemaining = 10;		// set this to change # cols

	public GameObject rowWarning;
	public SpriteRenderer rowWarningRenderer;
	public Sprite[] rowWarningFrame;

	public GameObject colWarning;
	public SpriteRenderer colWarningRenderer;
	public Sprite[] colWarningFrame;

	public int numPlayers;

	public int startWarning;
	public int startDestroying;
	public int count;
	public int[] intervals = { 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 };

	public Text winText;
	public Text deathText;
	public Text timeText;

    public GameObject player1, player2;

	int toDestroyRow;
	int rowToDestroy;
	int colToDestroy;
	bool isWarned;

	int destroyTime;
	bool destroyedEarlier;
	bool isGameOver;

	float rowCounter;
	float colCounter;

	int weaponSpawnCounter;
	public int weaponInterval;

	int buttonX;
	int buttonY;

	// Use this for initialization
	void Start () {
		timeElapsed = 0;
		winText.enabled = false;
		startDestroying = 20;
		startWarning = 18;

		rowCounter = 0;
		colCounter = 0;

		// Set-up the game tiles
		GameObject[] tileObjects = GameObject.FindGameObjectsWithTag ("Tile");
		tiles = new Tile[tileObjects.Length];

		for (int i = 0; i < tileObjects.Length; i++) {
			tiles [i] = tileObjects [i].GetComponent<Tile> ();
		}
		Array.Sort (tiles);

		map = new Tile[10, 10];

		for (int m = 0; m < 10; m++) {
			for (int n = 0; n < 10; n++) {
				map [m, n] = tiles [m * 10 + n];
			}
		}

		// Set up warning objects
		rowWarningRenderer = rowWarning.GetComponent<SpriteRenderer>();
		colWarningRenderer = colWarning.GetComponent<SpriteRenderer>();
	}

	void Update() {
		weaponSpawnCounter += 1;

		rowCounter += 0.2f;
		colCounter += 0.2f;
		Vector3 rowWarningPos = rowWarning.transform.position;
		rowWarningPos.y = (4.5f - (Mathf.FloorToInt(rowCounter) % rowsRemaining));
		rowWarning.transform.position = rowWarningPos;

		Vector3 colWarningPos = colWarning.transform.position;
		colWarningPos.x = (-4.5f + (Mathf.FloorToInt(colCounter) % colsRemaining));
		colWarning.transform.position = colWarningPos;

		if (weaponSpawnCounter % weaponInterval == 0) {
			SpawnRandomWeapons ();
		}
	}

	public void TriggerDestruction() {
		toDestroyRow = UnityEngine.Random.Range (0, 1);
		rowToDestroy = Mathf.FloorToInt (rowCounter) % rowsRemaining;
		colToDestroy = Mathf.FloorToInt (colCounter) % colsRemaining;
		StartCoroutine (DestroySetsOfTiles ());
	}

	public void SpawnRandomWeapons() {
		int tile1x = UnityEngine.Random.Range (0, colsRemaining);
		int tile1y = UnityEngine.Random.Range (0, rowsRemaining);

		int tile2x = UnityEngine.Random.Range (0, colsRemaining);
		int tile2y = UnityEngine.Random.Range (0, rowsRemaining);

		int tile3x = UnityEngine.Random.Range (0, colsRemaining);
		int tile3y = UnityEngine.Random.Range (0, rowsRemaining);

		// 0: glue gun
		// 1: gravity gun
		// 2: boxing gun
		int weapon1 = UnityEngine.Random.Range (0, 2);
		int weapon2 = UnityEngine.Random.Range (0, 2);
		switch (weapon1) {
		case 0:
			map [tile1x, tile1y].SpawnGlueGun ();
			break;
		case 1:
			map [tile1x, tile1y].SpawnGravGun ();
			break;
		case 2:
			map [tile1x, tile1y].SpawnBoxingGun ();
			break;
		}
		switch (weapon2) {
		case 0:
			map [tile2x, tile2y].SpawnGlueGun ();
			break;
		case 1:
			map [tile2x, tile2y].SpawnGravGun ();
			break;
		case 2:
			map [tile2x, tile2y].SpawnBoxingGun ();
			break;
		}

		map [buttonX, buttonY].hasButton = false;
		map [tile3x, tile3y].SpawnButton ();
		buttonX = tile3x;
		buttonY = tile3y;

	}

	void FixedUpdate() {
		// print ("[" + startDestroying + ", " + intervals[count] + ", " + startWarning + "]");
		//print ((Mathf.FloorToInt (timeElapsed) % 20) == (startDestroying % 20));

		// if time is up
		if (timeElapsed > roundDuration * 60.0f) {
			isGameOver = true;
			winText.enabled = true;
		}
		// else if warning should begin
		/*
		else if (Mathf.FloorToInt(timeElapsed) % 20 == startWarning && !isWarned) {
			print ("Warning begins.");
			isWarned = true;
			toDestroyRow = UnityEngine.Random.Range (0, 2);
			if (toDestroyRow == 0) {
				colToDestroy = UnityEngine.Random.Range (0, colsRemaining);
				colWarning.transform.position = new Vector2 (-4.5f + colToDestroy, 0);
			} else {
				rowToDestroy = UnityEngine.Random.Range (0, rowsRemaining);
				rowWarning.transform.position = new Vector2 (0, 4.5f - rowToDestroy);
			}
		}
		*/
		// for animating the warning before destruction occurs
		/*
		else if (Mathf.FloorToInt(timeElapsed) % 20 >= startWarning && Mathf.FloorToInt(timeElapsed) % 20 < startDestroying) {
			int index;

			// Alert column destruction
			if (toDestroyRow == 0) {
				colWarningRenderer.enabled = true;
				index = Mathf.FloorToInt ((Time.time * 60)) % colWarningFrame.Length;
				colWarningRenderer.sprite = colWarningFrame [index];
			}
			// Alert row destruction
			else {
				rowWarningRenderer.enabled = true;
				index = Mathf.FloorToInt ((Time.time * 60)) % rowWarningFrame.Length;
				rowWarningRenderer.sprite = rowWarningFrame [index];
			}
		}*/
		// else if it is time to destroy row/column of tiles
		/*
		else if (Mathf.FloorToInt(timeElapsed) > 0 && ((Mathf.FloorToInt(timeElapsed) % 20) == (startDestroying % 20)) && !destroyedEarlier) {
			print ("Destruction begins.");
			rowWarningRenderer.enabled = false;
			colWarningRenderer.enabled = false;
			isWarned = false;
			destroyedEarlier = true;
			startDestroying -= 2;
			count += 1;
			startWarning = startDestroying - intervals[count];
			StartCoroutine (DestroySetsOfTiles ());
		}
		*/
		// else if destruction phase is over
		/*
		else {
			//print ("Destruction ends.");
			destroyedEarlier = false;
		}
		*/
	}

	// LateUpdate is used to permit execution of other critical player/environment actions
	void LateUpdate () {
		if (!isGameOver) {
			timeElapsed += Time.deltaTime;

			timeText.text = "Time: " + Mathf.FloorToInt (timeElapsed / 60) + ":" + Mathf.FloorToInt (timeElapsed % 60);
		}
	}
		
	IEnumerator DestroySetsOfTiles() {
		// Destroy column of tiles
		if (toDestroyRow == 0) {
			colsRemaining -= 1;

			// destroy the column
			for (int r = 0; r < 10; r++) {
				map [r, colToDestroy].isDestroyed = true;
			}

			yield return new WaitForSeconds (2.0f);

			// simulate map fragment fusion
			for (int r = 0; r < rowsRemaining; r++) {
				map [r, colToDestroy].isDestroyed = false;
				map [r, colsRemaining].isDestroyed = true;
			}
            updatePlayerConstrain(false);

        }
		// Else, destroy row of tiles
		else {
			rowsRemaining -= 1;

			// destroy the column
			for (int c = 0; c < 10; c++) {
				map [rowToDestroy, c].isDestroyed = true;
			}

			yield return new WaitForSeconds (2.0f);

			// simulate map fragment fusion
			for (int c = 0; c < colsRemaining; c++) {
				map [rowToDestroy, c].isDestroyed = false;
				map [rowsRemaining, c].isDestroyed = true;
			}
            updatePlayerConstrain(true);

        }
	}

    void updatePlayerConstrain(bool isRow)
    {
        if (!isRow)
        {
            player1.GetComponent<PlayerScript>().removeCol();
            player2.GetComponent<PlayerScript>().removeCol();
            if (Mathf.Abs(player2.GetComponent<PlayerScript>().offsetX + 10) >= colsRemaining)
            {
				//if(player2.transform.position.x > map[0,colToDestroy].transform.position.x)
                player2.GetComponent<PlayerScript>().MoveLeft();
            }
            if (Mathf.Abs(player1.GetComponent<PlayerScript>().offsetX + 10) >= colsRemaining)
            {
                player1.GetComponent<PlayerScript>().MoveLeft();
            }
        }
        else
        {
            player1.GetComponent<PlayerScript>().removeRow();
            player2.GetComponent<PlayerScript>().removeRow();
            if (Mathf.Abs(player2.GetComponent<PlayerScript>().offsetY + 10) >= rowsRemaining)
            {
				//if(player2.transform.position.y > map[rowToDestroy,0].transform.position.y)
                player2.GetComponent<PlayerScript>().MoveUp();
            }
            if (Mathf.Abs(player1.GetComponent<PlayerScript>().offsetY + 10) >= rowsRemaining)
            {
                player1.GetComponent<PlayerScript>().MoveUp();
            }
        }
    }
}
