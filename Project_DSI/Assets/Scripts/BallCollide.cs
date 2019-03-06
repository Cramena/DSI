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
	Swiping
}

public enum BallColor
{
	Red,
	Green,
	Yellow
}

public class BallCollide : MonoBehaviour
{
	public Transform self;
	public Rigidbody body;

	//public Direction direction;
	public BallState state;
	public BallColor color;

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
    }

    // Update is called once per frame
    void Update()
    {
		if (swipeTimer > 0)
		{
			swipeTimer -= Time.deltaTime;
		}
		else
		{
			state = BallState.Falling;
		}

        if (state == BallState.Falling)
		{
			Fall();
		}
		else
		{
			Swipe();
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

	//void GetInput()
	//{
	//	if (Input.GetKeyDown(KeyCode.LeftArrow))
	//	{
	//		StartSwipe(Direction.Left);
	//	}
	//	if (Input.GetKeyDown(KeyCode.RightArrow))
	//	{
	//		StartSwipe(Direction.Right);
	//	}
	//	if (Input.GetKeyDown(KeyCode.UpArrow))
	//	{
	//		StartSwipe(Direction.Up);
	//	}
	//	if (Input.GetKeyDown(KeyCode.DownArrow))
	//	{
	//		StartSwipe(Direction.Down);
	//	}
	//}

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

	}
}
