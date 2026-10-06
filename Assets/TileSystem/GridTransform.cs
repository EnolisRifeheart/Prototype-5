using UnityEngine;

public class GridTransform : MonoBehaviour
{
    private GridManager gridManager;
    public void Move(Vector2Int input) {
        gridManager.MoveGameObject(gameObject, input);
    }

    public void SetGridManager(GridManager newManager) {
        gridManager = newManager;
    }
}