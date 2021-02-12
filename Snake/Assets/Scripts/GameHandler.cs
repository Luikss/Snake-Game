﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    private static GameHandler instance;
    private static int score;
    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;

    private void Awake() {
        instance = this;
    }

    void Start() {
        levelGrid = new LevelGrid(21,21);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }

    public static int GetScore() {
        return score;
    }

    public static void AddScore() {
        score += 1;
    }
}