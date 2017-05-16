using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public bool isStepped;		// control flag for player stepping on tile
	public bool isDestroyed;	// control flag for destroyed tile
	public int symbol;			// tile symbol

	public Sprite initial;
	public Sprite active;
	public Sprite destroyed;

	private SpriteRenderer tileRenderer;

	// Use this for initialization
	void Start ()
	{
		isStepped = false;
		isDestroyed = false;
		tileRenderer = GetComponent<SpriteRenderer> ();
		tileRenderer.sprite = initial;
		symbol = Random.Range (0, 5);	// 5 different possible symbols for now: {0, 1, 2, 3, 4}
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
		}
		// Uncomment this if you want to allow resetting the tile.
		/*else {
			tileRenderer.sprite = initial;
		}*/
	}
}

