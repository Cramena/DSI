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
	Collider col;

	//public Direction direction;
	public BallState state;
	public BallSize size;
	public Direction myDirection;

	//public Material

	public float fallSpeed = 5;
	public float swipeSpeed = 20;

	private Vector3 dirVector;

	public float swipeDuration = 0.5f;
	private float swipeTimer;

	public bool obstacle;

	public AnimationCurve speedCurve;

	public GameObject explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
		PlayerController.instance.SwipeEnd += StartSwipe;
		ModifySize(size);
		col = GetComponent<Collider>();
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
		StartCoroutine(SpawnEffect());
	}

	IEnumerator SpawnEffect()
	{
		float counter = 0.01f;
		while (counter < 1)
		{
			self.GetChild(0).localScale = new Vector3(counter, counter, counter);
			yield return new WaitForFixedUpdate();
			counter += Time.fixedDeltaTime * 2;
		}
		self.GetChild(0).localScale = Vector3.one;
	}

    // Update is called once per frame
    void Update()
    {

		if (GameManager.instance.swipe == SwipeMode.OneByOne)
		{
			if (swipeTimer > 0)
			{
				swipeTimer -= Time.deltaTime;
				body.velocity = dirVector * swipeSpeed * speedCurve.Evaluate(swipeTimer/swipeDuration);
				print(body.velocity.magnitude);
			}
			else if (state == BallState.Swiping)
			{
				state = BallState.Falling;
			}
		}
		else
		{
			if (swipeTimer > 0)
			{
				swipeTimer -= Time.deltaTime;
			}
			body.velocity = dirVector * swipeSpeed;
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

		//ClampToTop();


	}

	void Fall()
	{
		if (body.isKinematic) body.isKinematic = false;
		body.velocity = new Vector3(0, -fallSpeed, 0);
	}

	void Swipe()
	{
		//body.velocity = dirVector * swipeSpeed;
	}

	void StartSwipe(Direction _direction)
	{
		if (state != BallState.Falling) return;
		if (GameManager.instance.ballSpawn == BallMoveMode.Simultaneous)
		{
			state = BallState.Swiping;
			swipeTimer = swipeDuration;
			obstacle = false;

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
		else
		{
			float randTime = Random.Range(0, 0.5f);
			StartCoroutine(EnableSwipe(_direction, randTime));
		}
	}

	IEnumerator EnableSwipe(Direction _direction, float timeBefore)
	{
		yield return timeBefore;
		if (this != null)
		{
			obstacle = false;

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
	}

	private void OnCollisionStay(Collision collision)
	{
		if (state != BallState.Swiping) return;
		if (collision.gameObject.CompareTag("Ball"))
		{
			BallCollide ball = collision.gameObject.GetComponent<BallCollide>();
			if (ball.size == size)
			{
				MergeManager.instance.GetBallCollision(this, ball);
			}
		}
		//else if (state == BallState.Swiping)
		//{
		//	Vector3 _swipeDirection = (self.position - collision.contacts[0].point ).normalized;
		//	switch (PlayerController.instance.direction)
		//	{
		//		case Direction.Left:
		//			if (_swipeDirection.x > _swipeDirection.y && _swipeDirection.x < 0)
		//			{
		//				state = BallState.Falling;
		//			}
		//			break;
		//		case Direction.Right:
		//			if (_swipeDirection.x > _swipeDirection.y && _swipeDirection.x > 0)
		//			{
		//				state = BallState.Falling;
		//			}
		//			break;
		//		case Direction.Up:
		//			if (_swipeDirection.y > _swipeDirection.x && _swipeDirection.y > 0)
		//			{
		//				state = BallState.Falling;
		//			}
		//			break;
		//		case Direction.Down:
		//			if (_swipeDirection.y > _swipeDirection.x && _swipeDirection.y < 0)
		//			{
		//				state = BallState.Falling;
		//			}
		//			break;
		//		default:
		//			break;
		//	}
			
		//}

	}

	private void OnCollisionEnter(Collision collision)
	{
		if ((state == BallState.Swiping && swipeTimer <= 0 &&
			(!collision.gameObject.CompareTag("Ball") || collision.gameObject.GetComponent<BallCollide>().obstacle)))
		{
			Vector3 _swipeDirection = (collision.contacts[0].point - self.position).normalized;
			print("Swipe direction: "+ _swipeDirection + " and direction is " + PlayerController.instance.direction);
			switch (PlayerController.instance.direction)
			{
				case Direction.Left:
					if (Mathf.Abs(_swipeDirection.x) > Mathf.Abs(_swipeDirection.y) && _swipeDirection.x < 0)
					{
			print("Processing collision");
						state = BallState.Falling;
						obstacle = true;
					}
					break;
				case Direction.Right:
					if (Mathf.Abs(_swipeDirection.x) > Mathf.Abs(_swipeDirection.y) && _swipeDirection.x > 0)
					{
						print("Processing collision");
						state = BallState.Falling;
						obstacle = true;
					}
					break;
				case Direction.Up:
					if (Mathf.Abs(_swipeDirection.y) > Mathf.Abs(_swipeDirection.x) && _swipeDirection.y > 0)
					{
						state = BallState.Falling;
						print("Processing collision");
						obstacle = true;
					}
					break;
				case Direction.Down:
					if (Mathf.Abs(_swipeDirection.y) > Mathf.Abs(_swipeDirection.x) && _swipeDirection.y < 0)
					{
						state = BallState.Falling;
						print("Processing collision");
						obstacle = true;
					}
					break;
				default:
					break;
			}
		}
	}

	public void ModifySize(BallSize _newSize)
	{
		size = _newSize;
		switch (size)
		{
			case BallSize.Small:
				self.localScale = new Vector3(CONSTANTS.instance.smallBallSize, CONSTANTS.instance.smallBallSize, CONSTANTS.instance.smallBallSize);
				break;
			case BallSize.Medium:
				self.localScale = new Vector3(CONSTANTS.instance.mediumBallSize, CONSTANTS.instance.mediumBallSize, CONSTANTS.instance.mediumBallSize);
				break;
			case BallSize.Big:
				self.localScale = new Vector3(CONSTANTS.instance.bigBallSize, CONSTANTS.instance.bigBallSize, CONSTANTS.instance.bigBallSize);
				break;
		}
	}

	public void Ghost()
	{
		body.isKinematic = true;
		col.enabled = false;
	}

	public void UnGhost()
	{
		body.isKinematic = false;
		col.enabled = true;
	}

	public void Die()
	{
		Instantiate(explosionParticle, self.position, Quaternion.Euler(-90, 0, 0));
		Destroy(gameObject);
	}
}
