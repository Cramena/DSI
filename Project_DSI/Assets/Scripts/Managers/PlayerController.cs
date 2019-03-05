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

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	public Direction direction;

	public Ball[] balls;

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
		GetInput();

	}

	void GetInput()
	{
		print("Getting input");
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Swipe(Direction.Left);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Swipe(Direction.Right);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Swipe(Direction.Up);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Swipe(Direction.Down);
		}
	}

	void Swipe(Direction _direction)
	{
		print("Swiping");

		direction = _direction;
		while (CheckForObstacles() > 0)
		{
			print("Checking for obstacles");

			//Do nothing, this code is reeeeeaaaaallyyyyy dirty
		}

		for (int i = 0; i < balls.Length; i++)
		{
			if (balls[i].currentCell != null)
			{
				if (balls[i].currentCell.obstacle)
				{
					print("Same cell");
					balls[i].StartSwipeTowards(balls[i].currentCell);
				}
				else
				{
					//print("New cell: " + direction); //HERE IS THE BUG
					Vector2Int targetPosition = Vector2Int.zero;
					switch (direction)
					{
						case Direction.Left:
							targetPosition = new Vector2Int(balls[i].currentCell.position.x - 1, balls[i].currentCell.position.y);
							break;
						case Direction.Right:
							targetPosition = new Vector2Int(balls[i].currentCell.position.x + 1, balls[i].currentCell.position.y);
							break;
						case Direction.Up:
							targetPosition = new Vector2Int(balls[i].currentCell.position.x, balls[i].currentCell.position.y + 1);
							break;
						case Direction.Down:
							targetPosition = new Vector2Int(balls[i].currentCell.position.x, balls[i].currentCell.position.y - 1);
							break;
						default:
							break;
					}
					Cell newCell = GridManager.instance.grid[targetPosition.x, targetPosition.y];
					balls[i].StartSwipeTowards(newCell);
				}
			}
		}
		print("Swipe end");

	}

	int CheckForObstacles()
	{
		int nbOfObstacles = 0;
		for (int i = 0; i < balls.Length; i++)
		{
			if (balls[i].currentCell != null)
			{
				if (balls[i].CheckMoveTo() && !balls[i].currentCell.obstacle)
				{
					balls[i].currentCell.obstacle = true;
					nbOfObstacles++;
				}
			}	
		}

		return nbOfObstacles;
	}
}
