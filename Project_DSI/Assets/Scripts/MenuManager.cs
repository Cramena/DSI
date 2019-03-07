using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

	public GameObject playButton;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Play()
	{
		playButton.SetActive(false);
		//playButton.enabled = false;
		PlayerController.instance.state = PlayerState.Default;
	}
}
