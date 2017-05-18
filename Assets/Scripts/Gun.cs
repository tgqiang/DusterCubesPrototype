using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {

	public enum gunType {
		Glue,		//range 2, "stuck" status, 15 button mash to escape
		Grav,		//range full, 
		Boxing		//range 1, push-back 1
	};

	public int playerId;

	public bool toPull;
	public bool triggerGravityRow;
	public bool triggerGravityCol;

	public GameManagerScript gameManager;
	public Sprite[] gravityRows;
	public Sprite[] gravityCols;

	public SpriteRenderer gravityRowRenderer;
	public SpriteRenderer gravityColRenderer;

	int counter;

	gunType currentHoldingGun;

	// Use this for initialization
	void Start () {
		gravityRowRenderer.enabled = false;
		gravityColRenderer.enabled = false;
		currentHoldingGun = gunType.Glue;
	}

	// Update is called once per frame
	void Update () {
		if (counter > 0) {
			counter -= 1;
		}
		if (counter == 0) {
			gravityRowRenderer.enabled = false;
			gravityColRenderer.enabled = false;
		}
    }

	public void setGunType(gunType type){
		currentHoldingGun = type;
	}

	public void fireBullet(int direction)
	{
		/*if (playership != null) {
			GameObject bullet = (GameObject)Instantiate (enemyBullet);

			bullet.transform.position = transform.position;

			Vector2 direction = playership.transform.position - bullet.transform.position + new Vector3(2f,0,0);
			bullet.GetComponent<enemyBullet> ().damageValue = (int)(bullet.GetComponent<enemyBullet> ().damageValue * damageMultiplier);
			bullet.GetComponent<enemyBullet> ().setDirection (direction);
		}
		else {
			return;
		}*/
		switch (direction) {
		case 0: //up
			Debug.Log ("Top-face");
			gravityColRenderer.sprite = gravityCols [0];
			break;
		case 1: //down
			Debug.Log ("Down-face");
			gravityColRenderer.sprite = gravityCols [1];
			break;
		case 2: //left
			Debug.Log ("Left-face");
			gravityRowRenderer.sprite = gravityRows [0];
			break;
		case 3: //right
			Debug.Log ("Right-face");
			gravityRowRenderer.sprite = gravityRows [1];
			break;
			
		}

		switch (currentHoldingGun) {
		case gunType.Glue:
			Debug.Log ("PewPewPew.........Glue gun");
			break;
		case gunType.Grav:
			Debug.Log ("Zugoooooo.........Gravity gun");
			TriggerGravityGun (direction);
			break;
		case gunType.Boxing:
			Debug.Log ("BongBongBong.........Boxing gun");
			break;
		}
	}

	public void TriggerGravityGun(int direction) {
		print (transform.position);
		Vector2 pos = transform.position;

		switch (direction) {
		case 0: //up
			//Debug.Log ("Grav Gun Effect Top-face");
			if (toPull) {
				gravityColRenderer.sprite = gravityCols [1];
			}

			pos.y = 0;
			gravityColRenderer.transform.position = pos;
			triggerGravityCol = true;
			triggerGravityRow = false;
			break;
		case 1: //down
			//Debug.Log ("Grav Gun Effect Down-face");
			if (toPull) {
				gravityColRenderer.sprite = gravityCols [0];
			}

			pos.y = 0;
			gravityColRenderer.transform.position = pos;
			triggerGravityCol = true;
			triggerGravityRow = false;
			break;
		case 2: //left
			//Debug.Log ("Grav Gun Effect Left-face");
			if (toPull) {
				gravityRowRenderer.sprite = gravityRows [1];
			}

			pos.x = 0;
			gravityRowRenderer.transform.position = pos;
			triggerGravityRow = true;
			triggerGravityCol = false;
			break;
		case 3: //right
			//Debug.Log ("Grav Gun Effect Right-face");
			if (toPull) {
				gravityRowRenderer.sprite = gravityRows [0];
			}

			pos.x = 0;
			gravityRowRenderer.transform.position = pos;
			triggerGravityRow = true;
			triggerGravityCol = false;
			break;
		}

		if (triggerGravityRow) {
			gravityRowRenderer.enabled = true;
		} else if (triggerGravityCol) {
			gravityColRenderer.enabled = true;
		} else {
			gravityRowRenderer.enabled = false;
			gravityColRenderer.enabled = false;
		}

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		GameObject self = GameObject.Find ("Player " + playerId);

		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetComponent<PlayerScript> ().id != playerId) {
				if (players [i].GetComponent<Transform> ().position.y == gravityRowRenderer.transform.position.y) { // ROW
					if (direction == 2 && players [i].GetComponent<Transform> ().position.x <= self.transform.position.x) { // self facing left
						// move his ass along x axis
						if (toPull) {
							players [i].GetComponent<Transform> ().position += Vector3.right * 3;
						} else {
							players [i].GetComponent<Transform> ().position += Vector3.left * 3;
						}
						if (Mathf.Abs (players [i].GetComponent<Transform> ().position.x) > 4.5 ||
							Mathf.Abs (players [i].GetComponent<Transform> ().position.y) > 4.5) {
							players [i].GetComponent<PlayerScript> ().KillPlayer ();
						}
					} else if (direction == 3 && players [i].GetComponent<Transform> ().position.x >= self.transform.position.x) { // self facing right
						// move his ass along x axis
						if (toPull) {
							players [i].GetComponent<Transform> ().position += Vector3.left * 3;
						} else {
							players [i].GetComponent<Transform> ().position += Vector3.right * 3;
						}
						if (Mathf.Abs (players [i].GetComponent<Transform> ().position.x) > 4.5 ||
							Mathf.Abs (players [i].GetComponent<Transform> ().position.y) > 4.5) {
							players [i].GetComponent<PlayerScript> ().KillPlayer ();
						}
					}
				}
				else if (players [i].GetComponent<Transform> ().position.x == gravityColRenderer.transform.position.x) { // COLUMN
					if (direction == 0 && players [i].GetComponent<Transform> ().position.y >= self.transform.position.y) { // self facing up
						// move his ass along y axis
						if (toPull) {
							players [i].GetComponent<Transform> ().position += Vector3.down * 3;
						} else {
							players [i].GetComponent<Transform> ().position += Vector3.up * 3;
						}
						if (Mathf.Abs (players [i].GetComponent<Transform> ().position.x) > 4.5 ||
							Mathf.Abs (players [i].GetComponent<Transform> ().position.y) > 4.5) {
							players [i].GetComponent<PlayerScript> ().KillPlayer ();
						}
					} else if (direction == 1 && players [i].GetComponent<Transform> ().position.y <= self.transform.position.y) { // self facing down
						// move his ass along y axis
						if (toPull) {
							players [i].GetComponent<Transform> ().position += Vector3.up * 3;
						} else {
							players [i].GetComponent<Transform> ().position += Vector3.down * 3;
						}
						if (Mathf.Abs (players [i].GetComponent<Transform> ().position.x) > 4.5 ||
							Mathf.Abs (players [i].GetComponent<Transform> ().position.y) > 4.5) {
							players [i].GetComponent<PlayerScript> ().KillPlayer ();
						}
					}
				}
			}
		}

		counter = 20;
	}
}