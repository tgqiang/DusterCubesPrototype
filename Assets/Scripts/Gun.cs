using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {

	public enum gunType {
		Glue,		//range 2, "stuck" status, 15 button mash to escape
		Grav,		//range full, 
		Boxing		//range 1, push-back 1
	};

	gunType currentHoldingGun;
	public GameObject BulletT;
	public GameObject player;

	// Use this for initialization
	void Start () {
		currentHoldingGun = gunType.Glue;
	}

	// Update is called once per frame
	void Update () {
        
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
		GameObject bullet = (GameObject)Instantiate (BulletT);
		bullet.transform.position = transform.position;
		//bullet.GetComponent<Bullet> ().setDirection (direction)/;
		Vector2 fireTo;
		switch (currentHoldingGun) {
		case gunType.Glue:
			Debug.Log ("PewPewPew.........Glue gun");
			bullet.GetComponent<Bullet> ().setRange (2);
			break;
		case gunType.Grav:
			Debug.Log ("Zugoooooo.........Gravity gun");
			//bullet.GetComponent<Bullet> ().setRange (2);
			break;
		case gunType.Boxing:
			Debug.Log ("BongBongBong.........Boxing gun");
			bullet.GetComponent<Bullet> ().setRange (2);
			break;
		}

		switch (direction) {
		case 0: //up
			Debug.Log ("Top-face");
			bullet.GetComponent<Bullet> ().setPosition (transform.position);
			fireTo = transform.position - bullet.transform.position + new Vector3(0f,1f,0);
			bullet.GetComponent<Bullet> ().setDirection (fireTo);
			break;
		case 1: //down
			Debug.Log ("Down-face");
			bullet.GetComponent<Bullet> ().setPosition (transform.position);
			fireTo = transform.position - bullet.transform.position + new Vector3(0f,-1f,0);
			bullet.GetComponent<Bullet> ().setDirection (fireTo);
			break;
		case 2: //left
			Debug.Log ("Left-face");
			bullet.GetComponent<Bullet> ().setPosition (transform.position);
			fireTo = transform.position - bullet.transform.position + new Vector3(-1f,0f,0);
			bullet.GetComponent<Bullet> ().setDirection (fireTo);
			break;
		case 3: //right
			Debug.Log ("Right-face");
			bullet.GetComponent<Bullet> ().setPosition (transform.position);
			fireTo = transform.position - bullet.transform.position + new Vector3(1f,0f,0);
			bullet.GetComponent<Bullet> ().setDirection (fireTo);
			break;
			
		}

	}

}