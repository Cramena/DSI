using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONSTANTS : MonoBehaviour
{
	public static CONSTANTS instance;

	public float smallBallSize = 0.5f;
	public float mediumBallSize = 0.75f;
	public float bigBallSize = 1f;

	public Color smallColor;
	public Color mediumColor;
	public Color bigColor;

	private void Awake()
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

}
