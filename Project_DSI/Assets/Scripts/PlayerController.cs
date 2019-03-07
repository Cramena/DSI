using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	public Direction direction = Direction.Down;

	//public UICharInfo
	public delegate void SwipeEvent(Direction dir);
	public SwipeEvent SwipeEnd;

	Vector3 swipePosBegin;
	Vector3 swipePosEnd;
	Vector3 swipeDir;

    // Start is called before the first frame update
    void Awake()
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
		if (Time.timeScale <= 0) return;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			swipePosBegin = Input.mousePosition;
		}
		if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)) 
		{
			swipePosEnd = Input.mousePosition;
			swipeDir = (swipePosEnd - swipePosBegin);
			if (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y))
			{
				if (swipeDir.x > 0)
				{
					direction = Direction.Right;
					SwipeEnd(direction);
				}
				else
				{
					direction = Direction.Left;
					SwipeEnd(direction);
				}
			}
			else if (Mathf.Abs(swipeDir.y) > Mathf.Abs(swipeDir.x))
			{
				if (swipeDir.y > 0)
				{
					direction = Direction.Up;
					SwipeEnd(direction);
				}
				else
				{
					direction = Direction.Down;
					SwipeEnd(direction);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) )
		{
			//StartSwipe(Direction.Left);
			direction = Direction.Left;
			SwipeEnd(direction);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			//StartSwipe(Direction.Right);
			direction = Direction.Right;
			SwipeEnd(direction);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			//StartSwipe(Direction.Up);
			direction = Direction.Up;
			SwipeEnd(direction);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			//StartSwipe(Direction.Down);
			direction = Direction.Down;
			SwipeEnd(direction);
		}
		//return direction;
	}
}
