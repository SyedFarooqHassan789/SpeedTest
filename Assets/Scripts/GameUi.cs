using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUi: MonoBehaviour {
	public GameObject GameOverText;
	public bool isGameOver;
	public Text ScoreText;
	public Text HighScoreText;
	public static int Score;
	public GameController GameControllerClass;

	//Start method is used to initialize all the variables
	//It is unity default method which will be called at start for one time in game lifecycle
	void Start() {
		isGameOver = false;
		GameControllerClass = GameObject.Find("GameController").GetComponent < GameController > (); //Getting instance of gamecontroller class
		ShowScore(); //Show score at start
		UpdateHighScore(); //Update highscore at start
	}
	//Method called when user misses cirlce or clicked on wron cirlce
	//This method show game over image
	public void GameOver() {
		UpdateHighScore(); //Call upate highscore
		isGameOver = true;
		Time.timeScale = 0; //stop the game
		StartGameClickEvent();
	}
	//Method will be callled whenever new game black circle is clicked
	public void StartGameClickEvent() {
		Time.timeScale = 1; //Start the game 
		StartCoroutine(DelayAtStart()); //Coroutine to add slight delay at start when black start game image is clicked
	}
	IEnumerator DelayAtStart() {
		if (isGameOver) {
			isGameOver = false;
			Application.LoadLevel("MainScene"); //Restart the game again
		} else {
			Score = 0;
			ShowScore(); //Show score 
			GameOverText.SetActive(false); //Make black start game image invisible
			GameControllerClass.isGameStarted = true;
			yield return new WaitForSeconds(0.8f); //adding delay of 0.8 
			GameControllerClass.GenerateFlash(); //call generate flash to create a flash
		}
	}
	//Method to show the score
	public void ShowScore() {
		if (!isGameOver) {
			ScoreText.text = "" + Score; //filling the text of score
			PlayerPrefs.SetInt("Score", Score); //storing the value of score in player pref
			if (Score > PlayerPrefs.GetInt("HighScore")) {
				PlayerPrefs.SetInt("HighScore", Score); //Check vlaue of score and compare with high score
			}
		}
	}
	//Method to increment score
	public void IncrementScore() {
		if (!isGameOver) {
			Score = Score + 1;
		}
	}
	//Method to update score
	public void UpdateHighScore() {
		HighScoreText.text = "" + PlayerPrefs.GetInt("HighScore");
	}
}