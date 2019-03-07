using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDie : MonoBehaviour
{
	float timer;

	private void Start()
	{
		timer = 1.5f;
	}

	// Update is called once per frame
	void Update()
    {
		if (timer <= 0) Destroy(gameObject);//gameObject.SetActive(false);
		timer -= Time.deltaTime;
    }
}
