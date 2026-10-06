using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class PlayerGridController : MonoBehaviour
{

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask encounterLayer;
    [SerializeField] private float gridSize = 2f;
    private GridTransform gridTransform;

    // Tracks which direction the player is facing.
    private Vector2Int facingDirection = Vector2Int.up;

    private void Start()
    {
        gridTransform = GetComponent<GridTransform>();
    }

    private bool CanMove(Vector2Int dir)
    {
        Vector3 moveDir = new Vector3(dir.x, 0f, dir.y);

        Vector3 targetPosition = transform.position + moveDir * gridSize;

        Vector3 checkSize = new Vector3(gridSize * 0.4f, 1f, gridSize * 0.4f);

        // Check if an enemy is occupying the next grid-space.
        Collider[] encounters = Physics.OverlapBox(targetPosition,checkSize,Quaternion.identity, encounterLayer);

        if (encounters.Length > 0)
        {
            FixedEncounter encounter = encounters[0].GetComponent<FixedEncounter>();

            if (encounter != null)

            {
                encounter.StartEncounter();

                return false;
            }
        }

        // We need to check if the grid-space is being occupied by a wall.
        return !Physics.CheckBox(targetPosition, new Vector3(gridSize * 0.4f, 1f, gridSize * 0.4f), Quaternion.identity, wallLayer);

        {
            return false;
        }

        return true;

    }


    public void MoveForward()
    {
        if (CanMove(facingDirection))
        {
            gridTransform.Move(facingDirection);
        }
    }

    public void MoveBackward()
    {
        Vector2Int dir = -facingDirection;

        if (CanMove(dir))
        {
            gridTransform.Move(dir);
        }
    }

    public void MoveLeft()
    {
        Vector2Int dir = new Vector2Int(-facingDirection.y, facingDirection.x);

        if (CanMove(dir))
        {
            gridTransform.Move(dir);
        }
    }
    public void MoveRight()
    {
        Vector2Int dir = new Vector2Int(facingDirection.y, -facingDirection.x);

        if (CanMove(dir))
        {
            gridTransform.Move(dir);
        }
    }
    public void RotateLeft()
    {
        facingDirection = new Vector2Int(-facingDirection.y, facingDirection.x);

        transform.Rotate(0f, -90f, 0f);
    }

    public void RotateRight()
    {
        facingDirection = new Vector2Int(facingDirection.y, -facingDirection.x);

        transform.Rotate(0f, 90f, 0f);
    }
}