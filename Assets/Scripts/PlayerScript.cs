using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public bool isDead = false;
	public int id;
    public Transform pb;
    public List<Transform> Tiles;
	public Text deathText;

	public GameManagerScript gameManager;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
		if (!isDead)
        {
			checkTileOn ();
			if (id == 1) {
				if (Input.GetKeyUp (KeyCode.A)) {
					checkTileOff ();
					MoveLeft ();
					checkTileOn ();
				} else if (Input.GetKeyUp (KeyCode.D)) {
					checkTileOff ();
					MoveRight ();
					checkTileOn ();
				} else if (Input.GetKeyUp (KeyCode.W)) {
					checkTileOff ();
					MoveUp ();
					checkTileOn ();
				} else if (Input.GetKeyUp (KeyCode.S)) {
					checkTileOff ();
					MoveDown ();
					checkTileOn ();
				}
			} else if (id == 2) {
				if (Input.GetKeyUp(KeyCode.LeftArrow)) {
					checkTileOff ();
					MoveLeft();
					checkTileOn();
				}
				else if (Input.GetKeyUp(KeyCode.RightArrow)) {
					checkTileOff ();
					MoveRight();
					checkTileOn();
				} else if (Input.GetKeyUp(KeyCode.UpArrow)) {
					checkTileOff ();
					MoveUp();
					checkTileOn();
				} else if (Input.GetKeyUp(KeyCode.DownArrow)) {
					checkTileOff ();
					MoveDown();
					checkTileOn();
				}
			}
        }
    }

    void MoveLeft()
    {
        if (!((pb.position + Vector3.left).x < -4.5))
            pb.position = (pb.position + Vector3.left);
    }

    void MoveRight()
    {
        if (!((pb.position + Vector3.right).x > 4.5))
            pb.position = (pb.position + Vector3.right);
    }

    void MoveUp()
    {
        if ((pb.position + Vector3.up).y < 5.5)
        //if (!((pb.position + Vector3.up).y < 4.5))
            pb.position = (pb.position + Vector3.up);
    }

    void MoveDown()
    {
        if ((pb.position + Vector3.down).y > -5.5)
        //if (!((pb.position + Vector3.down).y > -4.5))
            pb.position = (pb.position + Vector3.down);
    }

    void checkTileOn()
    {
        for (int i = 0; i < Tiles.Count;i++)
        {
            if (pb.position.Equals(Tiles[i].position))
            {
				if (Tiles [i].GetComponent<Tile> ().isDestroyed) {
					isDead = true;
					deathText.text = deathText.text + "\nPlayer " + id + " died.";
				} else {
					Tiles [i].GetComponent<Tile> ().isStepped = true;
				}
            }
        }
    }

	void checkTileOff() {
		for (int i = 0; i < Tiles.Count;i++)
		{
			if (pb.position.Equals(Tiles[i].position))
			{
				if (Tiles [i].GetComponent<Tile> ().isDestroyed) {
					isDead = true;
					deathText.text = deathText.text + "\nPlayer " + id + " died.";
				} else {
					Tiles [i].GetComponent<Tile> ().isStepped = false;
				}
			}
		}
	}

}
