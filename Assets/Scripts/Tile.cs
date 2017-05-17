using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour, IComparable<Tile> {
	
	public bool isStepped;		// control flag for player stepping on tile
	public bool isDestroyed;	// control flag for destroyed tile
	public string id;			// for debugging purposes

	public Sprite initial;
	public Sprite active;
	public Sprite destroyed;
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
		} else {
			tileRenderer.sprite = initial;
		}
	}

	public int CompareTo(Tile other) {
		return id.CompareTo(other.id);
	}
}
