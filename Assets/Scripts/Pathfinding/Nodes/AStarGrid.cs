using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public Transform enemyToUse;

    public Vector2 gridWorldSize;
    public Vector2 gridOffset;
    public float nodeRadius;
    
    public LayerMask nonTraversableMask;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    

    Node[,] nodeGrid;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateNodeGrid();
    }

    private void CreateNodeGrid()
    {
        nodeGrid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right
            * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2 + gridOffset;

        // Create Nodes:
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right
                    * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter * nodeDiameter);

                bool walkable = !(Physics2D.OverlapCircle(
                    worldPoint,
                    nodeRadius,
                    nonTraversableMask
                    ));

                nodeGrid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPoint)
    {
        float percentX = (worldPoint.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPoint.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int index_X = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int index_Y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return nodeGrid[index_X, index_Y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (nodeGrid != null)
        {
            foreach (Node node in nodeGrid)
            {
                Gizmos.color = (node.isTraversable ? Color.green : Color.red);
                Gizmos.DrawSphere(node.worldPosition, nodeRadius);
            }
        }
    }
}
