using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour {
  
    private static GameOverWindow instance;

    private void Awake() {
        instance = this;
        Hide();
    }

    private void Show(bool isNewHighscore) {
        gameObject.SetActive(true);

        transform.Find("NewHighScoreText").gameObject.SetActive(isNewHighscore);
        transform.Find("NewHighScoreNumber").GetComponent<TMPro.TextMeshProUGUI>().text = "SCORE: " + GameHandler.GetScore().ToString();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public static void ShowStatic(bool isNewHighscore) {
        instance.Show(isNewHighscore);
    }
}