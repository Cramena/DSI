using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallNoiseMove : MonoBehaviour
{
	Transform self;
	//Rigidbody body;
	public float maxDistance = 0.25f;
	public float noiseFrequency = 0.25f;
	public float noiseSpeed = 1;
	public float directionLerpSpeed = 0.1f;
	float noiseTimer;
	Vector3 lastDirection;
	Vector3 targetDirection;
	Vector3 noiseDirection;
	
    void Start()
    {
		self = transform;
    }
	
    void FixedUpdate()
    {
        if (noiseTimer > 0)
		{
			noiseTimer -= Time.fixedDeltaTime;
		}
		else
		{
			noiseTimer = noiseFrequency;
			NoiseMove();
		}

		if (Mathf.Abs(self.localPosition.x) + Mathf.Abs(self.localPosition.y) > maxDistance)
		{
			StopMovement();
		}
		UpdateDirection();
		Move();


	}

	void StopMovement()
	{
		lastDirection = noiseDirection;
		targetDirection = -self.localPosition.normalized;
	}

	void NoiseMove()
	{
		float xMove = Random.Range(-1f, 1f);
		float yMove = Random.Range(-1f, 1f);
		lastDirection = noiseDirection;
		targetDirection = new Vector3(xMove, yMove, 0);
	}

	void UpdateDirection()
	{
		noiseDirection = Vector3.Lerp(lastDirection, targetDirection, directionLerpSpeed * noiseFrequency);
	}

	void Move()
	{
		self.position += noiseDirection * noiseSpeed * Time.deltaTime;
	}
}
