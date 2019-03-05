using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFollow : MonoBehaviour
{
	Transform self;
	public Transform parent;
	public float lerpSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
		self = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		self.position = Vector3.Lerp(self.position, parent.position, lerpSpeed);
    }
}
