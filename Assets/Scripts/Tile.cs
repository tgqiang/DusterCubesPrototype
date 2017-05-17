using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour, IComparable<Tile> {
	
	public bool isStepped;		// control flag for player stepping on tile
	public bool isDestroyed;	// control flag for destroyed tile
	public string id;			// for debugging purposes

	public bool hasGlueGun;
	public bool hasGravGun;
	public bool hasBoxingGun;

	public Sprite initial;
	public Sprite active;
	public Sprite destroyed;
	public Sprite glueGunSpawned;
	public Sprite gravGunSpawned;
	public Sprite boxingGunSpawned;
	public SpriteRenderer tileRenderer;

	// Use this for initialization
	void Awake ()
	{
		isStepped = false;
		isDestroyed = false;
		tileRenderer = GetComponent<SpriteRenderer> ();
		tileRenderer.sprite = initial;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isDestroyed) {
			// TODO: set tile to red (unsteppable)
			tileRenderer.sprite = destroyed;
		} else if (isStepped) {
			// TODO: set tile to white color
			tileRenderer.sprite = active;
		} else if (hasGlueGun) {
			tileRenderer.sprite = glueGunSpawned;
		} else if (hasGravGun) {
			tileRenderer.sprite = gravGunSpawned;
		} else if (hasBoxingGun) {
			// TODO: add placeholder sprites
			tileRenderer.sprite = boxingGunSpawned;
		} else {
			tileRenderer.sprite = initial;
		}
	}

	public void PickUpWeapon() {
		if (hasGlueGun) {
			hasGlueGun = false;
		} else if (hasGravGun) {
			hasGravGun = false;
		} else if (hasBoxingGun) {
			hasBoxingGun = false;
		}
	}

	public void SpawnGlueGun() {
		hasGravGun = false;
		hasBoxingGun = false;
		hasGlueGun = true;
	}

	public void SpawnGravGun() {
		hasBoxingGun = false;
		hasGlueGun = false;
		hasGravGun = true;
	}

	public void SpawnBoxingGun() {
		hasGlueGun = false;
		hasGravGun = false;
		hasBoxingGun = true;
	}

	public int CompareTo(Tile other) {
		return id.CompareTo(other.id);
	}
}
