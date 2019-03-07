using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public GameObject scoreUI;
	public Image scoreBackground;
	public Image scoreBar;
	public GameObject particleAttractor;
	float score;
	public float maxScore;

	[Space()]
	[Range(0, 1)] public float scoreLerpSpeed = 0.2f;
	public AnimationCurve scaleCurve;

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
		StartCoroutine(UpdateScoreUI());
	}

	IEnumerator UpdateScoreUI()
	{
		//float initialCounter = (score / maxScore) - scoreBar.fillAmount;
		while ((score / maxScore) - scoreBar.fillAmount  > 0.01f)
		{
			//float counter = Mathf.Clamp(((score / maxScore) - scoreBar.fillAmount) / initialCounter, 0, 1);
			//float currentScale = scaleCurve.Evaluate(counter);
			//scoreBar.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
			//scoreBackground.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
			scoreBar.fillAmount = Mathf.Lerp(scoreBar.fillAmount, (score / maxScore), scoreLerpSpeed );
			yield return null;
		}
		scoreBar.fillAmount = score / maxScore;
		//scoreBar.transform.localScale = Vector3.one;
		//scoreBackground.transform.localScale = Vector3.one;
	}
}
