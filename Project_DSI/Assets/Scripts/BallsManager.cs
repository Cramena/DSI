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
		int randomSpawnX = Random.Range(-2, 3);
		int randomColor = Random.Range(0, 3);

		//print("Random spawn x: " + randomSpawnX);


		Vector3 spawnPosition = new Vector3(randomSpawnX, 6, 0);
		BallCollide ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity).GetComponent<BallCollide>();
		ball.size = BallSize.Small;
		//ball.size = (BallSize)randomColor;
	}
}
