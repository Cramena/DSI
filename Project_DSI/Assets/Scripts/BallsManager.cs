using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
	public GameObject ballPrefab;

    // Start is called before the first frame update
    void Start()
    {
		PlayerController.instance.SwipeEnd += SpawnBall;
    }
	

	void SpawnBall(Direction dir)
	{
		float Xpos = 0;
		float Ypos = 0;

		switch (dir)
		{
			case Direction.Left:
				Ypos = Random.Range(5.25f, -3);
				Xpos = 2;
				break;
			case Direction.Right:
				Ypos = Random.Range(5.25f, -3);
				Xpos = -2;
				break;
			case Direction.Up:
				Xpos = Random.Range(-2, 2);
				Ypos = -3;
				break;
			case Direction.Down:
				Xpos = Random.Range(-2, 2);
				Ypos = 5.25f;
				break;
			default:
				break;
		}
		int randomColor = Random.Range(0, 3);

		//print("Random spawn x: " + randomSpawnX);


		Vector3 spawnPosition = new Vector3(Xpos, Ypos, 0);
		BallCollide ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity).GetComponent<BallCollide>();
		ball.size = BallSize.Small;
		//ball.size = (BallSize)randomColor;
	}
}
