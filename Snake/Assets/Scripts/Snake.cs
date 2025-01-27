﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    private enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    private enum State {
        Alive,
        Dead
    }

    private State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    public float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovementPositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private bool hasMoved;

    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
    }

    private void Awake() {
        gridPosition = new Vector2Int(10,10);
        gridMoveTimerMax = 0.25f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        snakeMovementPositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }

    private void Update() {
        switch (state) {
        case State.Alive:
            HandleInput();
            HandleGridMovement();
            SpeedUp();
            break;
        case State.Dead:
            break;
        }
    }

    private void HandleInput() {
        if(Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDirection != Direction.Down && hasMoved == true) {
            gridMoveDirection = Direction.Up;
            hasMoved = false;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && gridMoveDirection != Direction.Up && hasMoved == true) {
            gridMoveDirection = Direction.Down;
            hasMoved = false;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDirection != Direction.Right && hasMoved == true) {
            gridMoveDirection = Direction.Left;
            hasMoved = false;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDirection != Direction.Left && hasMoved == true) {
            gridMoveDirection = Direction.Right;
            hasMoved = false;
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            SoundManager.PlaySound(SoundManager.Sound.SnakeMove);

            SnakeMovePosition previousSnakeMovePosition = null;
            if(snakeMovementPositionList.Count > 0) {
                previousSnakeMovePosition = snakeMovementPositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovementPositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector = new Vector2Int (+1, 0);
            switch(gridMoveDirection) {
                case Direction.Right:   gridMoveDirectionVector = new Vector2Int (+1, 0); hasMoved = true; break;
                case Direction.Left:    gridMoveDirectionVector = new Vector2Int (-1, 0); hasMoved = true; break;
                case Direction.Up:      gridMoveDirectionVector = new Vector2Int (0, +1); hasMoved = true; break;
                case Direction.Down:    gridMoveDirectionVector = new Vector2Int (0, -1); hasMoved = true; break;
            }
            gridPosition += gridMoveDirectionVector;
            
            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            bool snakeAteFood = levelGrid.SnakeMovement(gridPosition);

            if(snakeAteFood) {
            SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
            snakeBodySize++;
            CreateSnakeBody();
            }

            if(snakeMovementPositionList.Count >= snakeBodySize + 1) {
                snakeMovementPositionList.RemoveAt(snakeMovementPositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition) {
                    state = State.Dead;
                    GameHandler.SnakeDied();
                    SoundManager.PlaySound(SoundManager.Sound.SnakeDie);
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }

    private void SpeedUp() {
        if (snakeBodySize > 4 && snakeBodySize < 10) {
            gridMoveTimerMax = 0.2f;
        } else if (snakeBodySize > 9 && snakeBodySize < 20) {
            gridMoveTimerMax = 0.15f;
        } else if (snakeBodySize > 19 && snakeBodySize < 30) {
            gridMoveTimerMax = 0.1f;
        } else if (snakeBodySize > 29) {
            gridMoveTimerMax = 0.085f;
        }
    }

    private void CreateSnakeBody() {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts() {
        for (int i=0; i<snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovementPositionList[i]);
        }
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

    public List<Vector2Int>GetFullSnakeGridPosition() {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovementPositionList) {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    private class SnakeBodyPart {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex) {
            GameObject snakeBodyGameObject = new GameObject("Snakebody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition) {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection()) {
            default:
            case Direction.Up:
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 0; 
                    break;
                case Direction.Left:
                    angle = 0 + 45; 
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Right:
                    angle = 0 - 45; 
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                }
                break;
            case Direction.Down:
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 180; 
                    break;
                case Direction.Left:
                    angle = 180 - 45;
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                case Direction.Right:
                    angle = 180 + 45; 
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Left:
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = +90; 
                    break;
                case Direction.Down:
                    angle = 180 - 45; 
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                case Direction.Up:
                    angle = 45; 
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Right:
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = -90; 
                    break;
                case Direction.Down:
                    angle = 180 + 45; 
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Up:
                    angle = -45; 
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                }
                break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition() {
            return snakeMovePosition.GetGridPosition();
        }
    }

    private class SnakeMovePosition {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction) {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition() {
            return gridPosition;
        }

        public Direction GetDirection() {
            return direction;
        }

        public Direction GetPreviousDirection() {
            if(previousSnakeMovePosition == null) {
                return Direction.Right;
            } else {
                return previousSnakeMovePosition.direction;
            }
        }
    }
}