using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;                                                                            // player's position                            
    public LayerMask unwalkableMask;                                                                    // areas which are unwalkable for the player
    public Vector2 gridWorldSize;                                                                       // area in the world coordinates that the grid is going to cover 
    public float nodeRadius;
    Node[,] grid;                                                                                       // 2 dimensional array of nodes

    float nodeDiameter;                                                                                 // diameter of each node 
    int gridSizeX, gridSizeY;                                                                           // assigning grid size in x and y axis

    void Awake()                                                                                        // awake the grid
    {
        nodeDiameter = nodeRadius * 2;                                                                  // diameter of the node                                                    
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);                                   // how many nodes can be fit into x - axis
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);                                   // how many nodes can fit into y - axis
        CreateGrid();
    }

    public int MaxSize                                                                                  // the max size of the grid
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()                                                                                   // creates the grid
    {
        grid = new Node[gridSizeX, gridSizeY];                                                          // new 2d array of nodes                                                       
        Vector3 worldBottomLeft =                                                                       
            transform.position                                                                          // identifies where origin
            - Vector3.right * gridWorldSize.x / 2 
            - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)                                                             // loops through all the positions on where the nodes will be at on the x and y axis                                
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = 
                   worldBottomLeft 
                 + Vector3.right * (x * nodeDiameter + nodeRadius) 
                 + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));         // checks which points are walkable and are not walkable 
                grid[x, y] = new Node(walkable, worldPoint, x, y);                                      // populates the grid with nodes with walkable and not walkable areas
            }
        }
    }

    public List<Node> GetNeighbors(Node node)                                                           // creates a list of nodes which are around the player                                            
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)                                                                   
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)                                           // highlights the position of the player
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;                 // converts x grid values to a percentage 
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;                 // converts y grid values to a percentage
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);                                       // rounds off the values x and y values
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];


    }

    public List<Node> path;

    void OnDrawGizmos()                                                                             // draws path around the grid for visualization purposes                                                                                                        
    {
        Gizmos.DrawLine(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1 ));
        if(grid !=null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                if(path != null)
                {
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                }
                Gizmos.DrawLine(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }

    }
}
