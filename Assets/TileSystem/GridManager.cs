using NUnit;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    // Dimensions of grid
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float gridScale;

    // Grid structure
    private GameObject[][] grid;

    private void Awake() {
        InitalizeGrid(); // Initalizing grid
    }

    // Setting the local position of the transform component on the game object
    private void UpdateGridAtPos(Vector2Int coord) {
        grid[coord.x][coord.y].transform.localPosition = new Vector3(coord.x * gridScale, 0, coord.y * gridScale);
    }

    // Instantiates and moves gameobject to position, returns successful or not
    public bool InstantiateObjectOnGrid(Vector2Int coord, GameObject newObject) {
        // Checking if its a valid position to spawn at
        if (!WithinBounds(coord)) { return false; }
        if (Occupied(coord)) { return false; }

        // Spawning and updating position
        GameObject createdNewObject = Instantiate(newObject, transform);
        createdNewObject.GetComponent<GridTransform>().SetGridManager(this);
        grid[coord.x][coord.y] = createdNewObject;
        UpdateGridAtPos(coord);

        // Returning success
        return true;
    }

    // Fills grid with gameobject reguardless of what was there before
    public void FillGrid(GameObject newObject) {
        ClearGrid();
        for (int i = 0; i < grid.Length; i++) {
            for (int j = 0; j < grid[i].Length; j++) {
                InstantiateObjectOnGrid(new Vector2Int(i, j), newObject);
            }
        }
    }

    // Destorys all game objects in grid
    public void ClearGrid() {
        for (int i = 0; i < grid.Length; i++) {
            for (int j = 0; j < grid[i].Length; j++) {
                DestoryObjectAtGridPos(new Vector2Int(i, j));
            }
        }
    }
    // Destorys the game object at position
    public void DestoryObjectAtGridPos(Vector2Int coord) {
        Destroy(grid[coord.x][coord.y]);
        grid[coord.x][coord.y] = null;
    }

    // Initalizing the length and width of the grid array
    private void InitalizeGrid() {
        grid = new GameObject[width][];
        for(int i = 0; i < grid.Length; i++) {
            grid[i] = new GameObject[height];
        }
    }

    // Checking if the position is occupied by a game object
    public bool Occupied(Vector2Int coord) {

        if (!WithinBounds(coord))
            return true;

        return grid[coord.x][coord.y] != null;
    }

    // Checking if a position is within the bounds of the grid
    public bool WithinBounds(Vector2Int coord) {
        if(coord.x < 0 || coord.y < 0) {
            return false;
        }

        if (coord.x >= width || coord.y >= height) {
            return false;
        }

        return true;
    }

    // Searching for game object at position
    public Vector2Int GetPositionOfObject(GameObject findObject) {
        for (int i = 0; i < grid.Length; i++) {
            for (int j = 0; j < grid[i].Length; j++) {
                if (grid[i][j] == findObject) {
                    // Return coords if found
                    return new Vector2Int(i, j);
                }
            }
        }

        // if not found return (-1,-1)
        return new Vector2Int(-1, -1);
    }

    // Getting the game object at a position in the coord
    public GameObject GetGameObjectAtPosition(Vector2Int coord) {
        return grid[coord.x][coord.y];
    }

    public bool MoveGameObject(GameObject objectToMove, Vector2Int displacement) {
        // Getting the start position and calculating end position
        Vector2Int start = GetPositionOfObject(objectToMove);
        Vector2Int end = start + displacement;

        // Checking if its a valid position to spawn at
        if (!WithinBounds(end)) { return false; }
        if (Occupied(end)) { return false; }

        grid[end.x][end.y] = objectToMove; // Moving the object
        UpdateGridAtPos(end);

        grid[start.x][start.y] = null; // resetting the start position

        return true;
    }

    // Helps visualize the grid
    private void OnDrawGizmos() {
        if(grid == null) {
            InitalizeGrid();
        }
        Vector3 origin = transform.position;
        for (int i = 0; i < grid.Length; i++) {
            for (int j = 0; j < grid[i].Length; j++) {
                Gizmos.DrawWireCube(origin + new Vector3(i * gridScale, 0, j * gridScale), new Vector3(gridScale, 0, gridScale));
            }
        }
    }
}