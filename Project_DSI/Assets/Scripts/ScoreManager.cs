using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	
	public Image scoreBackground;
	public Image scoreBar;
	public GameObject particleAttractor;
	public Transform barStart;
	public Transform barEnd;
	float score;
	public float maxScore;

	[Space()]
	[Range(0, 1)] public float scoreLerpSpeed = 0.2f;
	public AnimationCurve scaleCurve;

    // Start is called before the first frame update
    void Awake()
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
		particleAttractor.transform.position = ConvertUIToWorld(barStart.position);
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
		StartCoroutine(UpdateScoreUI());
	}

	IEnumerator UpdateScoreUI()
	{
		particleAttractor.transform.position = ConvertUIToWorld(Vector3.Lerp(barStart.position, barEnd.position, score / maxScore)); //ConvertUIToWorld(scoreBar.transform.position);
		while ((score / maxScore) - scoreBar.fillAmount  > 0.01f)
		{
			scoreBar.fillAmount = Mathf.Lerp(scoreBar.fillAmount, (score / maxScore), scoreLerpSpeed );
			yield return null;
		}
		scoreBar.fillAmount = score / maxScore;

	}

	Vector3 ConvertUIToWorld(Vector3 _posUI)
	{
		return Camera.main.ScreenToWorldPoint(_posUI);
	}
}
