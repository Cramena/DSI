using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
	public static MergeManager instance;

	public List<BallCollide> ballsColliding = new List<BallCollide>();
	public List<BallCollide[]> ballsCollisions = new List<BallCollide[]>();

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
		print("Get ball collision");
		//if (!ballsColliding.Contains(firstBall) || !ballsColliding.Contains(secondBall))
		//{
			if (DirectionCorrect(PlayerController.instance.direction, firstBall.self, secondBall.self))
			{
				AddBallsCollision(firstBall, secondBall);
				ProcessCollisions();
			CheckMergeDirection(firstBall, secondBall);
			}
		//}
	}

	void AddBallsCollision(BallCollide firstBall, BallCollide secondBall)
	{
		print("AddBallsCollision");
		//if (!ballsColliding.Contains(firstBall)) ballsColliding.Add(firstBall);
		//if (!ballsColliding.Contains(secondBall)) ballsColliding.Add(secondBall);
		ballsCollisions.Add(new BallCollide[] { firstBall, secondBall });
	}

	void ProcessCollisions()
	{
		print("ProcessCollisions");
		for (int i = 0; i < ballsCollisions.Count; i++)
		{
			//CheckMergeDirection(ballsCollisions[i]);
		}
		
	}

	void CheckMergeDirection(/*BallCollide[] _balls*/  BallCollide first, BallCollide second)
	{
		print("CheckMergeDirection");
		if (DirectionCorrect(PlayerController.instance.direction, first.self, second.self))
		{
			StartCoroutine(DoMerge(first, second));
		}
		else
		{
			StartCoroutine(DoMerge(second, first));
		}
	}

	IEnumerator DoMerge(BallCollide firstBall, BallCollide secondBall)
	{
		print("DoMerge");
		firstBall.state = BallState.Merging;
		secondBall.state = BallState.Merging;
		firstBall.Ghost();
		secondBall.Ghost();
		Vector3 secondInitPos = secondBall.self.position;
		float counter = 0;
		while(Vector3.Distance(firstBall.self.position, secondBall.self.position) > mergeEndDistance)
		{
			secondBall.self.position = Vector3.Lerp(secondBall.self.position, firstBall.self.position, mergeSpeed);
			yield return new WaitForFixedUpdate();
			//counter += mergeSpeed * Time.fixedDeltaTime;
			print("Merging");
		}
		Destroy(secondBall.gameObject);
		if (firstBall.size == BallSize.Big)
		{
			Destroy(firstBall.gameObject);
		}
		else
		{
			firstBall.ModifySize((BallSize)((int)firstBall.size + 1));
			//ballsColliding.Remove(secondBall);
			//ballsColliding.Remove(firstBall);
			ballsCollisions.Clear();
			firstBall.state = BallState.Falling;
			secondBall.state = BallState.Falling;
			firstBall.UnGhost();
			secondBall.UnGhost();
		}
		print("Merge DONE");
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
