using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using GridPathfindingSystem;
using Cinemachine;

public class GameHandler_GridCombatSystem : MonoBehaviour {

    public static GameHandler_GridCombatSystem Instance { get; private set; }

    [SerializeField] private Transform cinemachineFollowTransform;
    [SerializeField] private MovementTilemapVisual movementTilemapVisual;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Grid<GridCombatSystem.GridObject> grid;
    private MovementTilemap movementTilemap;
    public GridPathfinding gridPathfinding;

    private void Awake() {
        Instance = this;

        int mapWidth = 40;
        int mapHeight = 25;
        float cellSize = 10f;
        Vector3 origin = new Vector3(0, 0);

        grid = new Grid<GridCombatSystem.GridObject>(mapWidth, mapHeight, cellSize, origin, (Grid<GridCombatSystem.GridObject> g, int x, int y) => new GridCombatSystem.GridObject(g, x, y));

        gridPathfinding = new GridPathfinding(origin + new Vector3(1, 1) * cellSize * .5f, new Vector3(mapWidth, mapHeight) * cellSize, cellSize);
        gridPathfinding.RaycastWalkable();
        //gridPathfinding.PrintMap((Vector3 vec, Vector3 size, Color color) => World_Sprite.Create(vec, size, color));

        movementTilemap = new MovementTilemap(mapWidth, mapHeight, cellSize, origin);
    }

    private void Start() {
        movementTilemap.SetTilemapVisual(movementTilemapVisual);
        /*
        movementTilemap.SetAllTilemapSprite(MovementTilemap.TilemapObject.TilemapSprite.Move);
        grid.GetXY(new Vector3(171.5f, 128.5f), out int testX, out int testY);
        FunctionTimer.Create(() => {
            movementTilemap.SetAllTilemapSprite(MovementTilemap.TilemapObject.TilemapSprite.None);
            movementTilemap.SetTilemapSprite(testX + 0, testY + 0, MovementTilemap.TilemapObject.TilemapSprite.Move);
            movementTilemap.SetTilemapSprite(testX + 1, testY + 0, MovementTilemap.TilemapObject.TilemapSprite.Move);
            movementTilemap.SetTilemapSprite(testX - 1, testY + 0, MovementTilemap.TilemapObject.TilemapSprite.Move);
            movementTilemap.SetTilemapSprite(testX + 0, testY + 1, MovementTilemap.TilemapObject.TilemapSprite.Move);
            movementTilemap.SetTilemapSprite(testX + 0, testY - 1, MovementTilemap.TilemapObject.TilemapSprite.Move);

            movementTilemap.SetTilemapSprite(testX + 2, testY + 0, MovementTilemap.TilemapObject.TilemapSprite.Move);
            movementTilemap.SetTilemapSprite(testX + 1, testY + 1, MovementTilemap.TilemapObject.TilemapSprite.Move);
            movementTilemap.SetTilemapSprite(testX + 1, testY - 1, MovementTilemap.TilemapObject.TilemapSprite.Move);

            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 5; j++) {
                    movementTilemap.SetTilemapSprite(testX + i, testY + j, MovementTilemap.TilemapObject.TilemapSprite.Move);
                }
            }
        }, 1f);
        */
    }

    private void Update() {
        HandleCameraMovement();
    }

    private void HandleCameraMovement() {
        Vector3 moveDir = new Vector3(0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveDir.y = +1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveDir.y = -1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveDir.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveDir.x = +1;
        }
        moveDir.Normalize();

        float moveSpeed = 80f;
        cinemachineFollowTransform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public Grid<GridCombatSystem.GridObject> GetGrid() {
        return grid;
    }

    public MovementTilemap GetMovementTilemap() {
        return movementTilemap;
    }

    public void SetCameraFollowPosition(Vector3 targetPosition) {
        cinemachineFollowTransform.position = targetPosition;
    }

    public void ScreenShake() {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 15f;

        FunctionTimer.Create(() => { cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f; }, .1f);
    }





    public class EmptyGridObject {

        private Grid<EmptyGridObject> grid;
        private int x;
        private int y;

        public EmptyGridObject(Grid<EmptyGridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

            Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }

        public override string ToString() {
            return "";
        }
    }
}
