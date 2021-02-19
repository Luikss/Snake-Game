using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    private static GameHandler instance;
    private static int score;
    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;

    private void Awake() {
        instance = this;
        InitializeStatic();
        GameHandler.ResumeGame();
    }

    void Start() {
        levelGrid = new LevelGrid(21,21);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused()) {
                GameHandler.ResumeGame();
            } else {
                GameHandler.PauseGame();
            }
        }
    }

    private static void InitializeStatic() {
        score = 0;
    }

    public static int GetScore() {
        return score;
    }

    public static void AddScore() {
        score += 1;
    }

    public static void SnakeDied() {
        Score.TrySetNewHighscore(score);
        GameOverWindow.ShowStatic();
    }

    public static void ResumeGame() {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
    }

    public static void PauseGame() {
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
    }

    public static bool isGamePaused() {
        return Time.timeScale == 0f;
    }
}