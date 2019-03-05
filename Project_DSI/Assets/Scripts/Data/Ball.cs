using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
	Default,
	Swipe
}

public class Ball : MonoBehaviour
{
	[Space]
	[Header("Parameters:")]
	public float fallSpeed;
	public float swipeSpeed;
	public Cell currentCell;


	//PRIVATE 
	//REFERENCES
	private Transform self;
	private List<Collider> collidersInRange = new List<Collider>();

	//PARAMETERS
	private Vector3 lerpStartPosition;
	private Vector3 lerpTargetPosition;
	private BallState state = BallState.Default;
	

    // Start is called before the first frame update
    void Start()
    {
		self = transform;
    }

    // Update is called once per frame
    void Update()
    {
		if (state == BallState.Default)
		{
		CheckCanMoveDown();

		}
		else
		{
			SwipeTowards();
		}
	}

	private void FixedUpdate()
	{
		CheckCurrentCell();
	}

	void CheckCanMoveDown()
	{

		if (currentCell == null)
		{
			MoveDown();
			//print("No current cell");
		}
		else if (currentCell.position.y > 0)
		{
			//print("My cell is: " + currentCell);
			Cell checkedCell = GridManager.instance.grid[currentCell.position.x, currentCell.position.y - 1];
			if (checkedCell.containedObject == null ||
				(checkedCell.containedObject.self.position.y < checkedCell.transform.position.y))
			{
				MoveDown();
				//print("Free space below");
			}
			else
			{
			//print("No space below");
			MoveToCurrentCell();

			}

		}
		else
		{
			//print("At the bottom");
			MoveToCurrentCell();
		}

		//else if (currentCell.position.y > 0 && 
		//	GridManager.instance.grid[currentCell.position.x, currentCell.position.y -1].containedObject != null)
		//{
		//print("Not moving");

		//}
		//else if (currentCell.position.y > 0)
		//{
		//	MoveDown();
		//}

	}

	void MoveDown()
	{
		self.position += new Vector3(0, -fallSpeed * Time.deltaTime, 0);

	}

	void MoveToCurrentCell()
	{
		if (Vector3.Distance(self.position, currentCell.transform.position) > 0.1f)
		{
			self.position += (currentCell.transform.position - self.position).normalized * fallSpeed * Time.deltaTime;
		}
		else
		{
			self.position = currentCell.transform.position;
		}
	}

	void CheckCurrentCell()
	{
		//Collider[] nearColliders = Physics.OverlapSphere(self.position, 20, 0, QueryTriggerInteraction.Collide);
		if (collidersInRange.Count > 0)
		{
			if (currentCell != null) currentCell.containedObject = null;
			currentCell = GetNearestCell(collidersInRange);
			if (currentCell != null) currentCell.containedObject = this;
		}
		//Debug.Log(currentCell, currentCell);
	}

	Cell GetNearestCell(List<Collider> _colliders)
	{
		float shortestDistance = Mathf.Infinity;
		Collider closestCollider = null;

		float currentDistance;

		for (int i = 0; i < _colliders.Count; i++)
		{
			currentDistance = Vector3.Distance(self.position, _colliders[i].transform.position);
			if (currentDistance < shortestDistance)
			{
				shortestDistance = currentDistance;
				closestCollider = _colliders[i];
			}
		}

		if (closestCollider != null)
		{
			return closestCollider.GetComponent<Cell>();
		}
		else
		{
			return null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		print("Colliding with something");
		if (!collidersInRange.Contains(other))
		{
			collidersInRange.Add(other);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (collidersInRange.Contains(other))
		{
			collidersInRange.Remove(other);
		}
	}

	#region Move Behavior
	public bool CheckMoveTo()
	{
		Vector2Int targetPosition = Vector2Int.zero;
		switch (PlayerController.instance.direction)
		{
			case Direction.Left:
				targetPosition = new Vector2Int(currentCell.position.x -1, currentCell.position.y);
				break;
			case Direction.Right:
				targetPosition = new Vector2Int(currentCell.position.x + 1, currentCell.position.y);
				break;
			case Direction.Up:
				targetPosition = new Vector2Int(currentCell.position.x, currentCell.position.y + 1);
				break;
			case Direction.Down:
				targetPosition = new Vector2Int(currentCell.position.x, currentCell.position.y - 1);
				break;
			default:
				break;
		}
		if ((currentCell.position.x <= 0 || currentCell.position.x >= GridManager.instance.gridWidth ||
			currentCell.position.y <= 0 || currentCell.position.y >= GridManager.instance.gridHeight) ||
			GridManager.instance.grid[targetPosition.x, targetPosition.y].obstacle)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void StartSwipeTowards(Cell newCell)
	{
		print("Target is: " + (currentCell.position - newCell.position));
		state = BallState.Swipe;
		currentCell = newCell;
	}

	void SwipeTowards()
	{
		if (Vector3.Distance(self.position, currentCell.transform.position) > 0.1f)
		{
			self.position += (currentCell.transform.position - self.position).normalized * fallSpeed * Time.deltaTime;
		}
		else
		{
			self.position = currentCell.transform.position;
			state = BallState.Default;
		}
	}
	#endregion

}

