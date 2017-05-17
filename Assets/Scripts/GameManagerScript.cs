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

	// Use this for initialization
	void Start () {
		timeElapsed = 0;
		winText.enabled = false;
		startDestroying = 20;
		startWarning = 18;

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

	void FixedUpdate() {
		print ("[" + startDestroying + ", " + intervals[count] + ", " + startWarning + "]");
		//print ((Mathf.FloorToInt (timeElapsed) % 20) == (startDestroying % 20));

		// if time is up
		if (timeElapsed > roundDuration * 60.0f) {
			isGameOver = true;
			winText.enabled = true;
		}
		// else if warning should begin
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
		// for animating the warning before destruction occurs
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
		}
		// else if it is time to destroy row/column of tiles
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
		// else if destruction phase is over
		else {
			//print ("Destruction ends.");
			destroyedEarlier = false;
		}
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
                player2.GetComponent<PlayerScript>().MoveUp();
            }
            if (Mathf.Abs(player1.GetComponent<PlayerScript>().offsetY + 10) >= rowsRemaining)
            {
                player1.GetComponent<PlayerScript>().MoveUp();
            }
        }
    }
}
