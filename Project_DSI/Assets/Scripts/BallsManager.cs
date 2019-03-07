using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
	public GameObject ballPrefab;
	public int nbOfSpawnedBalls = 1;
	public bool randomNumberOfSpawnedBalls;
	public int minNbOfSpawnedBalls = 1;
	public int maxNbOfSpawnedBalls = 1;

	public float randomSpawnPosLateral = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
		PlayerController.instance.SwipeEnd += SpawnBall;
    }

	void SpawnBall(Direction dir)
	{
		if (randomNumberOfSpawnedBalls)
		{
			for (int i = 0; i < (int)Random.Range(minNbOfSpawnedBalls, maxNbOfSpawnedBalls+1); i++)
			{
				SpawnSingleBall(dir);
			}
		}
		else
		{
			for (int i = 0; i < nbOfSpawnedBalls; i++)
			{
				SpawnSingleBall(dir);
			}
		}

	}

	void SpawnSingleBall(Direction dir)
	{

		float Xpos = 0;
		float Ypos = 0;

		switch (dir)
		{
			case Direction.Left:
				Ypos = Random.Range(4.5f, -2);
				Xpos = Random.Range(1.4f - randomSpawnPosLateral, 1.4f);
				break;
			case Direction.Right:
				Ypos = Random.Range(4.5f, -2);
				Xpos = Random.Range(-1.4f, -1.4f + randomSpawnPosLateral);
				break;
			case Direction.Up:
				Xpos = Random.Range(-2, 2);
				Ypos = Random.Range(-2, -2 + randomSpawnPosLateral);
				break;
			case Direction.Down:
				Xpos = Random.Range(-2, 2);
				Ypos = Random.Range(4.5f - randomSpawnPosLateral, 4.5f);
				break;
			default:
				break;
		}
		int randomColor = Random.Range(0, 3);

		//print("Random spawn x: " + randomSpawnX);


		Vector3 spawnPosition = new Vector3(Xpos, Ypos, 0);
		BallCollide ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity).GetComponent<BallCollide>();
		//ball.size = BallSize.Small;
		ball.size = (BallSize)randomColor;
	}
}
