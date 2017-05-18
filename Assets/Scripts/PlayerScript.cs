using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{

    public bool isDead = false;
	public bool isGlued = false;
	public int mashCount = 0;
	public int id;
    public Transform pb;
    public List<Transform> Tiles;
	public Text deathText;
    public float offsetX = 0, offsetY = 0;
	public int direction = -1;


	public bool isGlueGunEquipped;
	public bool isGravGunEquipped;
	public bool isBoxingGunEquipped;
	public SpriteRenderer playerRenderer;
	public Sprite unequipped;
	public Sprite[] glueGunEquippedSprites;
	public Sprite[] gravGunEquippedSprites;
	public Sprite[] boxingGunEquippedSprites;

	public GameManagerScript gameManager;

    // Use this for initialization
    void Start()
    {
		playerRenderer = GetComponent<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {

    }


	/*
	 *			0 
	 * 			|
	 * 	  2 --------- 3
	 * 			|
	 * 			4
	 * 
	*/
    void FixedUpdate()
    {
		if (!isDead && !isGlued) {
			checkTileOn ();
			if (id == 1) {
				if (Input.GetKeyDown (KeyCode.A)) {
					direction = 2;
					checkTileOff ();
					MoveLeft ();
					checkTileOn ();
				} else if (Input.GetKeyDown (KeyCode.D)) {
					direction = 3;
					checkTileOff ();
					MoveRight ();
					checkTileOn ();
				} else if (Input.GetKeyDown (KeyCode.W)) {
					direction = 0;
					checkTileOff ();
					MoveUp ();
					checkTileOn ();
				} else if (Input.GetKeyDown (KeyCode.S)) {
					direction = 1;
					checkTileOff ();
					MoveDown ();
					checkTileOn ();
				}
				if (isBoxingGunEquipped || isGlueGunEquipped || isGravGunEquipped) {
					if (Input.GetKeyDown (KeyCode.Space)) {
						pb.GetComponent<Gun> ().fireBullet (direction);
					}
				}
			} else if (id == 2) {
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					direction = 2;
					checkTileOff ();
					MoveLeft ();
					checkTileOn ();
				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
					direction = 3;
					checkTileOff ();
					MoveRight ();
					checkTileOn ();
				} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
					direction = 0;
					checkTileOff ();
					MoveUp ();
					checkTileOn ();
				} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
					direction = 1;
					checkTileOff ();
					MoveDown ();
					checkTileOn ();
				}
				if (isBoxingGunEquipped || isGlueGunEquipped || isGravGunEquipped) {
					if (Input.GetKeyDown (KeyCode.RightControl)) {
						pb.GetComponent<Gun> ().fireBullet (direction);
					}
				}
			}
		} else if (isGlued) {
			if (id == 1) {
				if (mashCount > 15) {
					mashCount = 0;
					isGlued = false;
				} else
					if (Input.GetKeyDown (KeyCode.Space)) {
						mashCount += 1;
					}
			} else if (id == 2) {
				if (mashCount >= 15) {
					setUnglued ();
				} else
					if (Input.GetKeyDown (KeyCode.RightControl)) {
						mashCount += 1;
					}
			}
				
		}
    }

    public void MoveLeft()
    {
		if (isGlueGunEquipped) {
			playerRenderer.sprite = glueGunEquippedSprites [2];
		} else if (isGravGunEquipped) {
			playerRenderer.sprite = gravGunEquippedSprites [2];
		} else if (isBoxingGunEquipped) {
			// TODO: add placeholder sprites
			playerRenderer.sprite = boxingGunEquippedSprites[2];
		} else {
			playerRenderer.sprite = unequipped;
		}
        if (!((pb.position + Vector3.left).x < -4.5)) //always be left most, aka will not be truncted
			pb.position = (pb.position + Vector3.left );
    }

    void MoveRight()
    {
		if (isGlueGunEquipped) {
			playerRenderer.sprite = glueGunEquippedSprites [3];
		} else if (isGravGunEquipped) {
			playerRenderer.sprite = gravGunEquippedSprites [3];
		} else if (isBoxingGunEquipped) {
			// TODO: add placeholder sprites
			playerRenderer.sprite = boxingGunEquippedSprites[3];
		} else {
			playerRenderer.sprite = unequipped;
		}
        if (!((pb.position + Vector3.right).x > 4.5 - offsetX))
			pb.position = (pb.position + Vector3.right );
    }

    public void MoveUp()
    {
		if (isGlueGunEquipped) {
			playerRenderer.sprite = glueGunEquippedSprites [0];
		} else if (isGravGunEquipped) {
			playerRenderer.sprite = gravGunEquippedSprites [0];
		} else if (isBoxingGunEquipped) {
			// TODO: add placeholder sprites
			playerRenderer.sprite = boxingGunEquippedSprites[0];
		} else {
			playerRenderer.sprite = unequipped;
		}
        if ((pb.position + Vector3.up).y < 5.5) //always be top most, aka will not be truncted
        //if (!((pb.position + Vector3.up).y < 4.5))
			pb.position = (pb.position + Vector3.up );
    }

    void MoveDown()
    {
		if (isGlueGunEquipped) {
			playerRenderer.sprite = glueGunEquippedSprites [1];
		} else if (isGravGunEquipped) {
			playerRenderer.sprite = gravGunEquippedSprites [1];
		} else if (isBoxingGunEquipped) {
			// TODO: add placeholder sprites
			playerRenderer.sprite = boxingGunEquippedSprites[1];
		} else {
			playerRenderer.sprite = unequipped;
		}
        if ((pb.position + Vector3.down).y > -5.5 + offsetY)
        //if (!((pb.position + Vector3.down).y > -4.5))
			pb.position = (pb.position + Vector3.down );
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
					if (Tiles [i].GetComponent<Tile> ().hasGlueGun) {
						EquipGlueGun ();
					} else if (Tiles [i].GetComponent<Tile> ().hasGravGun) {
						EquipGravGun ();
					} else if (Tiles [i].GetComponent<Tile> ().hasBoxingGun) {
						EquipBoxingGun ();
					}
					Tiles [i].GetComponent<Tile> ().PickUpWeapon ();
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

	public void EquipGlueGun() {
		isGravGunEquipped = false;
		isBoxingGunEquipped = false;
		isGlueGunEquipped = true;
		playerRenderer.sprite = glueGunEquippedSprites [direction];
		pb.GetComponent<Gun> ().setGunType (Gun.gunType.Glue);
	}

	public void EquipGravGun() {
		isBoxingGunEquipped = false;
		isGlueGunEquipped = false;
		isGravGunEquipped = true;
		playerRenderer.sprite = gravGunEquippedSprites [direction];
		pb.GetComponent<Gun> ().setGunType (Gun.gunType.Grav);
	}

	public void EquipBoxingGun() {
		isGlueGunEquipped = false;
		isGravGunEquipped = false;
		isBoxingGunEquipped = true;
		playerRenderer.sprite = boxingGunEquippedSprites [direction];
		pb.GetComponent<Gun> ().setGunType (Gun.gunType.Boxing);
	}

	public void setGlued() {
		isGlued = true;
	}

	public void setUnglued() {
		isGlued = false;
		mashCount = 0;
	}

    public void removeRow() //for border constrain
    {
        offsetY += 1;
    }
    public void removeCol()//for border constrain
    {
        offsetX += 1;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (id == 1) {
			if (other.gameObject.layer != LayerMask.NameToLayer ("Player1")) {
				Debug.Log ("Player 1 stuck");
				isGlued = true;
				Destroy (other.gameObject);
			}
		} else if (id == 2) {
			if (other.gameObject.layer != LayerMask.NameToLayer ("Player2")) {
				Destroy (other.gameObject);
				isGlued = true;
				Debug.Log ("Player 2 stuck");
			}
		} else if (id == 3) {
			if (other.gameObject.layer != LayerMask.NameToLayer ("Player3")) {
				Destroy (other.gameObject);
				isGlued = true;
				Debug.Log ("Player 3 stuck");
			}
		} else if (id == 4) {
			if (other.gameObject.layer != LayerMask.NameToLayer ("Player4")) {
				Destroy (other.gameObject);
				isGlued = true;
				Debug.Log ("Player 4 stuck");
			}
		}

	}
}
