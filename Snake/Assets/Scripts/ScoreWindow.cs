using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour {

    private Text scoreText;

    private void Awake() {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();

        int highscore = Score.GetHighScore();
        transform.Find("HighScoreText").GetComponent<TMPro.TextMeshProUGUI>().text = "HIGHSCORE:\n" + highscore.ToString();
    }

    private void Update() {
        scoreText.text = GameHandler.GetScore().ToString();
    }
}