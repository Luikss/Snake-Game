using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovementPositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
    }

    private void Awake() {
        gridPosition = new Vector2Int(10,10);
        gridMoveTimerMax = 0.25f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);

        snakeMovementPositionList = new List<Vector2Int>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
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
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovementPositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            bool snakeAteFood = levelGrid.SnakeMovement(gridPosition);

            if(snakeAteFood) {
            snakeBodySize++;
            CreateSnakeBody();
            }

            if(snakeMovementPositionList.Count >= snakeBodySize + 1) {
                snakeMovementPositionList.RemoveAt(snakeMovementPositionList.Count - 1);
            }
        }

        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

        UpdateSnakeBodyParts();
    }

    private void CreateSnakeBody() {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts() {
        for (int i=0; i<snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].SetGridPosition(snakeMovementPositionList[i]);
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
        gridPositionList.AddRange(snakeMovementPositionList);
        return gridPositionList;
    }

    private class SnakeBodyPart {

        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex) {
            GameObject snakeBodyGameObject = new GameObject("Snakebody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition) {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
}