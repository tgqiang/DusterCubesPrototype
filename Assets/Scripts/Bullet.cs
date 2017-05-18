using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public enum bulletType {
		Glue,		//range 2, "stuck" status, 15 button mash to escape
		Grav,		//range full, 
		Boxing		//range 1, push-back 1
	};

	[SerializeField]
	float speed = 0.2f; // the bullet speed
	Vector2 _direction; // the direction of the bullet
	bool isReady; //when bullet direction is set
    [SerializeField]
    int damage;

	public int Range = 4;
	Vector3 startPos;
	int bulletTypeI = 0;


    void Awake()
	{
		//speed = 0.2f;
		isReady = false;
		startPos = transform.position;
		//Debug.Log (startPos);
	}

	// Use this for initialization
	void Start () {
	
	}

	public void setBulletSpeed(float spd)
	{
		speed = spd;
	}

	public void setRange(int r){
		Range = r;	
	}

	public void setType(int t){
		bulletTypeI = t;	
	}

	public int getType(){
		return bulletTypeI;	
	}

	public void setDirection(Vector2 direction)
	{
		_direction = direction.normalized;

		isReady = true;
	}
	public void setPosition(Vector3 pos)
	{
		startPos = pos;
	}

	// Update is called once per frame
	void Update () {
		if (isReady) {
			// get bullet's current position
			Vector2 position = transform.position;

			position += _direction * speed;

			transform.position = position;
			//Debug.Log (position);
			//transform.LookAt(new Vector3(_direction.x,_direction.y,0));
			// bottome-left of screen
			Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

			//top-right of screen
			Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

			if ((transform.position.x < startPos.x - Range) || (transform.position.x > startPos.x + Range) ||
				(transform.position.y < startPos.y - Range) || (transform.position.y > startPos.y + Range)) {

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
