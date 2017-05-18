﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField]
	float speed = 0.2f; // the bullet speed
	Vector2 _direction; // the direction of the bullet
	bool isReady; //when bullet direction is set
    [SerializeField]
    int damage;
	Vector2 Range;


    void Awake()
	{
		//speed = 0.2f;
		isReady = false;
		//transform.position = 
	}

	// Use this for initialization
	void Start () {
	
	}

	public void setBulletSpeed(float spd)
	{
		speed = spd;
	}

	public void setDirection(Vector2 direction)
	{
		_direction = direction.normalized;

		isReady = true;
	}

	// Update is called once per frame
	void Update () {
		if (isReady) {
			// get bullet's current position
			Vector2 position = transform.position;

			position += _direction * speed;

			transform.position = position;
			//transform.LookAt(new Vector3(_direction.x,_direction.y,0));
			// bottome-left of screen
			Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

			//top-right of screen
			Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

			if ((transform.position.x < min.x) || (transform.position.x > max.x) ||
				(transform.position.y < min.y) || (transform.position.y > max.y)) {

				Destroy (gameObject);
			}
		}
	}

    public int damageValue
    {
         get { return damage; }
        set { damage = value; }
    }
}
