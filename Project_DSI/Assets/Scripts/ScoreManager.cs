using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public List<BallCollide> balls = new List<BallCollide>();

	public int levelIndex = 1;

	public Text loseText;
	public Text currentLVLUI;
	public Text nextLVLUI;
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

	public float maxBallsSize = 18;
	public float ballsSize;
	bool aboutToCheck;

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
	}


	private void Start()
	{
		PlayerController.instance.SwipeEnd += CheckLose;
		particleAttractor.transform.position = ConvertUIToWorld(barStart.position);
		particleAttractor.transform.position = new Vector3(particleAttractor.transform.position.x, particleAttractor.transform.position.y, 0);
		//levelIndex = SaveManager.instance.currentSave.level;
		currentLVLUI.text = levelIndex.ToString();
		nextLVLUI.text = (levelIndex + 1).ToString();
		loseText.text = "";
	}

	//private void Update()
	//{
	//	CheckLose(Direction.Down);
	//}

	public void CheckLose (Direction dir)
	{
		if (aboutToCheck)
		{
			aboutToCheck = false;
			//StartCoroutine(WaitCheckLose());
			CheckLoseNow();
		}
		else
		{
			aboutToCheck = true;
			StartCoroutine(WaitCheckLose());
	}
}

	void CheckLoseNow()
	{
		StopCoroutine(WaitCheckLose());
		ballsSize = 0;
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null) ballsSize += balls[i].self.localScale.x;
		}
		if (ballsSize > maxBallsSize)
		{
			Lose();
		}
	}

	IEnumerator WaitCheckLose()
	{
		yield return new WaitForSecondsRealtime(0.75f);
		aboutToCheck = false;
		ballsSize = 0;
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null) ballsSize += balls[i].self.localScale.x;
		}
		if (ballsSize > maxBallsSize)
		{
			Lose();
		}
		else if (ballsSize > maxBallsSize - 5)
		{
			loseText.text = "WARNING: TOO MANY BUBBLES";
			loseText.fontSize = 50;
		}
		else if (!transition)
		{
			loseText.text = "";
		}
	}

	void Lose()
	{
		if (transition) return;
		print("Lose!");
		loseText.text = "YOU LOSE!";
		loseText.fontSize = 70;
		transition = true;
		PlayerController.instance.state = PlayerState.CantPlay;
		
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
				balls[i].Die();
		}
		StartCoroutine(InitializeLevel());
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
		maxScore *= 1.2f;

		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
				balls[i].Die();
		}
		SaveManager.instance.currentSave.level = levelIndex;
		SaveManager.instance.SaveGame();
		StartCoroutine(InitializeLevel());
	}

	IEnumerator InitializeLevel()
	{
		yield return new WaitForSecondsRealtime(1.5f);
		loseText.text = "";
		PlayerController.instance.state = PlayerState.Default;
		currentLVLUI.text = levelIndex.ToString();
		nextLVLUI.text = (levelIndex + 1).ToString();
		score = 0;
		scoreBar.fillAmount = 0;
		particleAttractor.transform.position = ConvertUIToWorld(barStart.position);
		particleAttractor.transform.position = new Vector3(particleAttractor.transform.position.x, particleAttractor.transform.position.y, 0);
		transition = false;
		//if (NewLevel != null) NewLevel();
	}
}
