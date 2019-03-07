using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Down,
	Up,
	Left,
	Right
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
	public BallState state = BallState.Falling;
	public BallSize size;
	public Direction myDirection;

	//public Material

	public float fallSpeed = 5;
	public float swipeSpeed = 20;

	private Vector3 dirVector;

	public float swipeDuration = 0.5f;
	private float swipeTimer;

	public bool obstacle;
	public float explosionRadius = 1;
	public float timeBeforeExplosion = 0.2f;
	public float score = 15;

	public float growSpeed = 3;

	public AnimationCurve speedCurve;

	public GameObject explosionParticle;

	float targetScale;

    // Start is called before the first frame update
    void Start()
    {
		PlayerController.instance.SwipeEnd += StartSwipe;
		ModifySize(size);
		col = GetComponent<Collider>();
		StartCoroutine(SpawnEffect());
		switch (PlayerController.instance.direction)
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

	IEnumerator SpawnEffect()
	{
		float counter = 0.01f;
		float endScale = 0;
		switch (size)
		{
			case BallSize.Small:
				endScale = CONSTANTS.instance.smallBallSize;
				break;
			case BallSize.Medium:
				endScale = CONSTANTS.instance.mediumBallSize;
				break;
			case BallSize.Big:
				endScale = CONSTANTS.instance.bigBallSize;
				break;
			default:
				break;
		}
		while (counter < endScale)
		{
			self.localScale = new Vector3(counter, counter, counter);
			yield return new WaitForFixedUpdate();
			counter += Time.fixedDeltaTime * endScale * growSpeed;
		}
		self.localScale = new Vector3(endScale, endScale, endScale);
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
		body.velocity = dirVector * fallSpeed;//new Vector3(0, -fallSpeed, 0);
	}

	void Swipe()
	{
		//body.velocity = dirVector * swipeSpeed;
	}

	void StartSwipe(Direction _direction)
	{
		//if (state != BallState.Falling) return;
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
				targetScale = CONSTANTS.instance.smallBallSize;
				StartCoroutine(Grow());
				//self.localScale = new Vector3(CONSTANTS.instance.smallBallSize, CONSTANTS.instance.smallBallSize, CONSTANTS.instance.smallBallSize);
				break;
			case BallSize.Medium:
				targetScale = CONSTANTS.instance.mediumBallSize;
				StartCoroutine(Grow());
				//self.localScale = new Vector3(CONSTANTS.instance.mediumBallSize, CONSTANTS.instance.mediumBallSize, CONSTANTS.instance.mediumBallSize);
				break;
			case BallSize.Big:
				targetScale = CONSTANTS.instance.bigBallSize;
				StartCoroutine(Grow());
				//self.localScale = new Vector3(CONSTANTS.instance.bigBallSize, CONSTANTS.instance.bigBallSize, CONSTANTS.instance.bigBallSize);
				break;
		}
	}

	IEnumerator Grow()
	{
		float currentScale = self.localScale.x;
		while (targetScale - currentScale > 0.1f)
		{
			currentScale = Mathf.Lerp(currentScale, targetScale, 0.2f);
			self.localScale = new Vector3(currentScale, currentScale, currentScale);
			yield return null;
		}
		self.localScale = new Vector3(targetScale, targetScale, targetScale);
		//for (int i = 0; i < length; i++)
		//{
		//	yield return null;
		//}
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
		StopAllCoroutines();
		if (size == BallSize.Big)
		{
			Instantiate(explosionParticle, self.position, Quaternion.Euler(-90, 0, 0));
			Collider[] toDie = Physics.OverlapSphere(self.position, explosionRadius);
			if (toDie.Length > 0)
			{
				for (int i = 0; i < toDie.Length; i++)
				{
					if (toDie[i].gameObject.CompareTag("Ball"))
					{
						//toDie[i].GetComponent<BallCollide>().Die();
						Destroy(toDie[i].gameObject);
					}
				}
			}
		}
		Destroy(gameObject);
		ScoreManager.instance.AddScore(score);
		//GameManager.instance.TimeFreeze();

		//StartCoroutine(Explode());
	}

	IEnumerator Explode()
	{
		gameObject.SetActive(false);
		yield return new WaitForSeconds(timeBeforeExplosion);
		if (size == BallSize.Big)
		{
			Instantiate(explosionParticle, self.position, Quaternion.Euler(-90, 0, 0));
			//StartCoroutine(SpawnExplosion());
			Collider[] toDie = Physics.OverlapSphere(self.position, explosionRadius);
			if (toDie.Length > 0)
			{
				for (int i = 0; i < toDie.Length; i++)
				{
					if (toDie[i].gameObject.CompareTag("Ball"))
					{
						//toDie[i].GetComponent<BallCollide>().Die();
						Destroy(toDie[i].gameObject);
					}
				}
			}
		}
		Destroy(gameObject);
		ScoreManager.instance.AddScore(score);
		GameManager.instance.TimeFreeze();
		//if (Death != null) Death(this);
		print("About to explode");
		print("Exploded");
	}
}
