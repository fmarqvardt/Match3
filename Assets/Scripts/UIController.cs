using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	[SerializeField]
	private Text scoreText;
	private int score = 0;

	public static UIController Instance
	{
		get { return instance; }
		set { instance = value; }
	}
	private static UIController instance;

	void Awake()
	{
		instance = GetComponent<UIController>();
		SetScore(0);
	}

	/// <summary>
	/// Add to current score.
	/// </summary>
	public void AddScore(int amount)
	{
		score += amount;
		scoreText.text = "Score: " + score;
	}

	/// <summary>
	/// Set score.
	/// </summary>
	public void SetScore(int amount)
	{
		score = amount;
		scoreText.text = "Score: " + amount;
	}
}
