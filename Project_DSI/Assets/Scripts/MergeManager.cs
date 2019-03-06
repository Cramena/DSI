using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
	public static MergeManager instance;

	public List<BallCollide> ballsColliding = new List<BallCollide>();
	public List<List<BallCollide>> ballsCollisions = new List<List<BallCollide>>();

	public float mergeEndDistance = 0.1f;
	[Range(0, 1)] public float mergeSpeed = 0.2f;

	// Start is called before the first frame update
	void Awake	()
    {
		#region Singleton
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		#endregion
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void GetBallCollision(BallCollide firstBall, BallCollide secondBall)
	{
		if (!ballsColliding.Contains(firstBall) || !ballsColliding.Contains(secondBall))
		{
			if (DirectionCorrect(PlayerController.instance.direction, firstBall.self, secondBall.self))
			{
				AddBallsCollision(firstBall, secondBall);
				ProcessCollisions();
			}
		}
	}

	void AddBallsCollision(BallCollide firstBall, BallCollide secondBall)
	{
		if (!ballsColliding.Contains(firstBall)) ballsColliding.Add(firstBall);
		if (!ballsColliding.Contains(secondBall)) ballsColliding.Add(secondBall);
	}

	void ProcessCollisions()
	{
		for (int i = 0; i < ballsCollisions.Count; i++)
		{
			CheckMergeDirection(ballsCollisions[i]);
		}
	}

	void CheckMergeDirection(List<BallCollide> _balls)
	{
		if (DirectionCorrect(PlayerController.instance.direction, _balls[0].self, _balls[1].self))
		{
			StartCoroutine(DoMerge(_balls[0], _balls[1]));
		}
		else
		{
			StartCoroutine(DoMerge(_balls[1], _balls[0]));
		}
	}

	IEnumerator DoMerge(BallCollide firstBall, BallCollide secondBall)
	{
		Vector3 secondInitPos = secondBall.self.position;
		while(Vector3.Distance(firstBall.self.position, secondBall.self.position) > mergeEndDistance)
		{
			secondBall.self.position = Vector3.Lerp(secondInitPos, firstBall.self.position, mergeSpeed);
			yield return null;
		}
		Destroy(secondBall.gameObject);
		firstBall.ModifySize((BallSize)((int)firstBall.size + 1));
		ballsColliding.Remove(secondBall);
		ballsColliding.Remove(firstBall);
	}

	bool DirectionCorrect(Direction _dir, Transform firstTransform, Transform secondTransform)
	{
		Vector3 collisionDirection = (secondTransform.position - firstTransform.position).normalized;
		switch (_dir)
		{
			case Direction.Left:
				if (Mathf.Abs(collisionDirection.x) > Mathf.Abs(collisionDirection.y) && collisionDirection.x < 0)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			case Direction.Right:
				if (Mathf.Abs(collisionDirection.x) > Mathf.Abs(collisionDirection.y) && collisionDirection.x > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			case Direction.Up:
				if (Mathf.Abs(collisionDirection.y) > Mathf.Abs(collisionDirection.x) && collisionDirection.y > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			case Direction.Down:
				if (Mathf.Abs(collisionDirection.y) > Mathf.Abs(collisionDirection.x) && collisionDirection.y < 0)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			default:
				return false;
				break;
		}
	}
}
