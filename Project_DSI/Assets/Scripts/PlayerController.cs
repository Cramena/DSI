using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	public Direction direction;

	//public UICharInfo
	public delegate void SwipeEvent(Direction dir);
	public SwipeEvent SwipeEnd;

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
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//StartSwipe(Direction.Left);
			direction = Direction.Left;
			if (SwipeEnd != null) SwipeEnd(direction);
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
