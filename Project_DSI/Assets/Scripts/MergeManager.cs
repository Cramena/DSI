using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
	public static MergeManager instance;

	//public List<BallCollide> balls

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

	public void Merge(BallCollide firstBall, BallCollide secondBall)
	{
		//if (firstBall)
	}
}
