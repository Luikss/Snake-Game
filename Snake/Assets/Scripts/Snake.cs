﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;

    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
    }

    private void Awake() {
        gridPosition = new Vector2Int(10,10);
        gridMoveTimerMax = 0.25f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDirection.y != -1) {
            gridMoveDirection.x = 0;
            gridMoveDirection.y = 1;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && gridMoveDirection.y != 1) {
            gridMoveDirection.x = 0;
            gridMoveDirection.y = -1;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDirection.x != 1) {
            gridMoveDirection.x = -1;
            gridMoveDirection.y = 0;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDirection.x != -1) {
            gridMoveDirection.x = 1;
            gridMoveDirection.y = 0;
        }

        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
        }

        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

        levelGrid.SnakeMovement(gridPosition);
    }

    private float GetAngleFromVector(Vector2 dir) {
        float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        if(n < 0) {
            n += 360;
        }
        return n;
    }

    public Vector2Int GetGridPosition() {
        return gridPosition;
    }
}
