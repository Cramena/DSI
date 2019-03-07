using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public Image scoreBar;
	public GameObject particleAttractor;
	float score;
	public float maxScore;

    // Start is called before the first frame update
    void Start()
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

	public void AddScore(float _scoreAmount)
	{
		score += _scoreAmount;
		if (score >= maxScore)
		{
			//End level
		}
		UpdateScoreUI();
	}

	void UpdateScoreUI()
	{
		scoreBar.fillAmount = score / maxScore;
	}
}
