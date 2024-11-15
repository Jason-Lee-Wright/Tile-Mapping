using UnityEngine;

public class TileMovement : MonoBehaviour
{
    public float tileSize = 0.08f; // Size of each tile
    public float moveSpeed = 1f; // Speed of movement between tiles
    private Vector3 targetPosition;
    private Vector3Int lastMove;
    private bool isMoving = false;

    private TestTilemap map; // Reference to the TestTilemap script

    void Start()
    {
        map = FindObjectOfType<TestTilemap>(); // Find the TestTilemap script
        targetPosition = transform.position;    // Set initial target position
    }

    void Update()
    {
        // If already moving, don't accept new input
        if (isMoving)
        {
            // Smoothly move to the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving if we reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
            return;
        }

        // Check for input and move in the corresponding direction
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMove(Vector3Int.up);
            lastMove = Vector3Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMove(Vector3Int.down);
            lastMove = Vector3Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(Vector3Int.left);
            lastMove = Vector3Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(Vector3Int.right);
            lastMove = Vector3Int.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryMove(lastMove * 2);
        }
    }

    void TryMove(Vector3Int direction)
    {
        // Calculate the new tile position
        Vector3Int gridPosition = map.tilemap.WorldToCell(transform.position);
        Vector3Int targetGridPosition = gridPosition + direction;

        // Check if the target tile is passable
        if (map.IsTilePassable(targetGridPosition))
        {
            targetPosition = map.tilemap.CellToWorld(targetGridPosition) + new Vector3(tileSize / 2, tileSize / 2, 0); // Offset to center on tile
            isMoving = true;

            direction = lastMove;
        }
    }
}