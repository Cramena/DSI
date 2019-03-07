using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public List<BallCollide> balls = new List<BallCollide>();

	int levelIndex = 1;

	public GameObject nextLevel;
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

	public delegate void NewLevelEvent();
	public NewLevelEvent NewLevel;
	bool transition;

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
		particleAttractor.transform.position = new Vector3(particleAttractor.transform.position.x, particleAttractor.transform.position.y, 0);

	}

	public void AddScore(float _scoreAmount)
	{
		score += _scoreAmount;
		if (score >= maxScore)
		{
			NextLevel();
		}
		StartCoroutine(UpdateScoreUI());
	}

	IEnumerator UpdateScoreUI()
	{
		particleAttractor.transform.position = ConvertUIToWorld(Vector3.Lerp(barStart.position, barEnd.position, score / maxScore)); //ConvertUIToWorld(scoreBar.transform.position);
		particleAttractor.transform.position = new Vector3(particleAttractor.transform.position.x, particleAttractor.transform.position.y, 0);
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

	void NextLevel()
	{
		if (transition) return;
		transition = true;
		PlayerController.instance.state = PlayerState.CantPlay;

		Instantiate(nextLevel, new Vector3(0, 5f, 0), Quaternion.identity);
		levelIndex++;
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
				balls[i].Die();
		}
		StartCoroutine(InitializeLevel());
	}

	IEnumerator InitializeLevel()
	{
		yield return new WaitForSecondsRealtime(1.5f);
		
		PlayerController.instance.state = PlayerState.Default;

		score = 0;
		maxScore *= 1.2f;
		scoreBar.fillAmount = 0;
		particleAttractor.transform.position = ConvertUIToWorld(barStart.position);
		particleAttractor.transform.position = new Vector3(particleAttractor.transform.position.x, particleAttractor.transform.position.y, 0);
		transition = false;
		//if (NewLevel != null) NewLevel();
	}
}
