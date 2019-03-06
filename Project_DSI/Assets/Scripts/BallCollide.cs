using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Left,
	Right,
	Up,
	Down
}

public enum BallState
{
	Falling,
	Swiping,
	Merging
}

public enum BallSize
{
	Small,
	Medium,
	Big
}

public class BallCollide : MonoBehaviour
{
	public Transform self;
	public Rigidbody body;

	//public Direction direction;
	public BallState state;
	public BallSize size;

	//public Material

	public float fallSpeed = 5;
	public float swipeSpeed = 20;

	private Vector3 dirVector;

	public float swipeDuration = 0.5f;
	private float swipeTimer;

    // Start is called before the first frame update
    void Start()
    {
		PlayerController.instance.SwipeEnd += StartSwipe;
		ModifySize(size);
		//switch (size)
		//{
		//	case BallSize.Small:
		//		self.GetChild(0).localScale = new Vector3(.5f, .5f, .5f);
		//		break;
		//	case BallSize.Medium:
		//		self.GetChild(0).localScale = new Vector3(.75f, .75f, .75f);
		//		break;
		//	case BallSize.Big:
		//		self.GetChild(0).localScale = Vector3.one;
		//		break;
		//}
	}

    // Update is called once per frame
    void Update()
    {
		if (swipeTimer > 0)
		{
			swipeTimer -= Time.deltaTime;
		}
		else if (state == BallState.Swiping)
		{
			state = BallState.Falling;
		}

		switch (state)
		{
			case BallState.Falling:
				Fall();
				break;
			case BallState.Swiping:
				Swipe();
				break;
			default:
				break;
		}
		
    }

	void Fall()
	{
		body.velocity = new Vector3(0, -fallSpeed, 0);
	}

	void Swipe()
	{
		body.velocity = dirVector * swipeSpeed;
	}

	void StartSwipe(Direction _direction)
	{
		state = BallState.Swiping;
		swipeTimer = swipeDuration;
		switch (_direction)
		{
			case Direction.Left:
				dirVector = new Vector3(-1, 0, 0);
				break;
			case Direction.Right:
				dirVector = new Vector3(1, 0, 0);
				break;
			case Direction.Up:
				dirVector = new Vector3(0, 1, 0);
				break;
			case Direction.Down:
				dirVector = new Vector3(0, -1, 0);
				break;
			default:
				break;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (state == BallState.Swiping)
		{
			state = BallState.Falling;
		}
		if (collision.gameObject.CompareTag("Ball"))
		{
			BallCollide ball = collision.gameObject.GetComponent<BallCollide>();
			if (state == BallState.Falling && ball.size == size)
			{
				switch (size)
				{
					case BallSize.Small:
						ModifySize(BallSize.Medium);
						//size = BallSize.Medium;
						//self.GetChild(0).localScale = new Vector3(.75f, .75f, .75f);
						break;
					case BallSize.Medium:
						ModifySize(BallSize.Big);
						//size = BallSize.Big;
						//self.GetChild(0).localScale = Vector3.one;
						break;
					case BallSize.Big:
						Destroy(gameObject);
						break;
				}
			}
		}

	}

	public void ModifySize(BallSize _newSize)
	{
		size = _newSize;
		switch (size)
		{
			case BallSize.Small:
				self.GetChild(0).localScale = new Vector3(CONSTANTS.instance.smallBallSize, CONSTANTS.instance.smallBallSize, CONSTANTS.instance.smallBallSize);
				break;
			case BallSize.Medium:
				self.GetChild(0).localScale = new Vector3(CONSTANTS.instance.mediumBallSize, CONSTANTS.instance.mediumBallSize, CONSTANTS.instance.mediumBallSize);
				break;
			case BallSize.Big:
				self.GetChild(0).localScale = new Vector3(CONSTANTS.instance.bigBallSize, CONSTANTS.instance.bigBallSize, CONSTANTS.instance.bigBallSize);
				break;
		}
	}
}
