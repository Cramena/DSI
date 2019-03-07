using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeMode
{
	OneByOne,
	Unlimited
}

public enum BallMoveMode
{
	Simultaneous,
	Randomized
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public SwipeMode swipe = SwipeMode.Unlimited;
	public BallMoveMode ballSpawn;

	public int timeFreezeDuration = 2;
	int timeFreezeTimer;

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
		swipe = SwipeMode.Unlimited;
		Screen.orientation = ScreenOrientation.Portrait;
		Application.targetFrameRate = 60;
		DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
		{
			if (swipe == SwipeMode.OneByOne)
			{
				swipe = SwipeMode.Unlimited;
			}
			else
			{
				swipe = SwipeMode.OneByOne;
			}
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			if (ballSpawn == BallMoveMode.Randomized)
			{
				ballSpawn = BallMoveMode.Simultaneous;
			}
			else
			{
				ballSpawn = BallMoveMode.Randomized;
			}
		}
		ManageTimeFreeze();
	}

	void ManageTimeFreeze()
	{
		if (timeFreezeTimer > 0)
		{
			timeFreezeTimer --;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	public void TimeFreeze()
	{
		timeFreezeTimer = timeFreezeDuration;
		Time.timeScale = 0;
	}
	
}
