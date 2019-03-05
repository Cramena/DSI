using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONSTANTS : MonoBehaviour
{
	public static CONSTANTS instance;

	//[Space]
	//[Header("Cells parameters:")]
	//public float cellSpacing;

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
