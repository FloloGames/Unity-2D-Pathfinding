using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public LayerMask WallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float DistToWall = 0f;
    [Header("Distance Between Nodes")]
    public float Distance;

    Node[,] grid;
    public List<Node> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private bool showGrid = false;

    private void Awake()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        Debug.Log("GRID CREATED");
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 bottomLeft = new Vector2(transform.position.x, transform.position.y) - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = bottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool Wall = true;

                if (Physics2D.OverlapCircle(worldPoint, nodeRadius + DistToWall, WallMask))
                {
                    Wall = false;
                }
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
        showGrid = true;
    }
    public void DestroyGrid()
    {
        grid = null;
        Debug.Log("GRID DESTROYED");
    }
    public void ShowHideGrid()
    {
        showGrid = !showGrid;
    }
    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (node.IsWall)
                {
                    Gizmos.color = new Color(255, 255, 255, 127);
                }
                else
                {
                    Gizmos.color = new Color(0, 0, 0, 127);
                }

                if (FinalPath != null)
                {
                    if (FinalPath.Contains(node))
                    {
                        Gizmos.color = new Color(255, 0, 0, 127);
                    }
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - Distance));
            }
        }
    }
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.gridX + 1;
        icheckY = a_NeighborNode.gridY;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.gridX - 1;
        icheckY = a_NeighborNode.gridY;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.gridX;
        icheckY = a_NeighborNode.gridY + 1;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.gridX;
        icheckY = a_NeighborNode.gridY - 1;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    public Node NodeFromWorldPosition(Vector2 worldPosition)
    {
        Vector3 localPosition = new Vector3(worldPosition.x - transform.position.x - nodeRadius, worldPosition.y - transform.position.y - nodeRadius, 1);
        float percentX = (localPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (localPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        float xFloat = (gridSizeX) * percentX;
        int x = Mathf.RoundToInt(xFloat);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        float yFloat = (gridSizeY) * percentY;
        int y = Mathf.RoundToInt(yFloat);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);
        return grid[x, y];
    }
}
