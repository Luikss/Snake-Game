using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;

    void Start() {
        levelGrid = new LevelGrid(21,21);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }

    void Update() {
    }
}