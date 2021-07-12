using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Reference: https://www.youtube.com/watch?v=dn1XRIaROM4


public class PathfindingMinMax: MonoBehaviour
{
	PathRequestManager requestManager;

	public Transform seeker, target;
	Grid grid;

	void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();		// awakes the path request manager
		grid = GetComponent<Grid>();								// awakes the grid
	}

	void Update()
	{
		FindPath(seeker.position, target.position);						// finds the path
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos)		// method to start the path
    {
		StartCoroutine(FindPath(startPos, targetPos));
    }

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];							// creates an array of waypoints
		bool pathSuccess = false;										// initialize path success
		
		
		Node startNode = grid.NodeFromWorldPoint(startPos);				// initialize a start node
		Node targetNode = grid.NodeFromWorldPoint(targetPos);			// initialize a target node
		
		
		if (startNode.walkable && targetNode.walkable)
		{	
			heapminmax<Node> openSet = new heapminmax<Node>(grid.MaxSize); //  call heapminmax to evaluate the set of nodes to be evaluated	
			HashSet<Node> closedSet = new HashSet<Node>();				// call a hashset to the set of nodes that already have been evaluated
			openSet.Add(startNode);										// adds starting node to array

			while (openSet.Count > 0)									// while openset is not empty
			{
				Node node = openSet.Remove();							// remove the list of utility nodes that don't need to be visited
				closedSet.Add(node);									// add the nodes to a closed set of arrays

				if (node == targetNode)									// once the target node which identifies the location of the player 
					
				{
					pathSuccess = true;                                 // then the path success is true
					break;
				
				}
                

				foreach (Node neighbor in grid.GetNeighbors(node))		// for each of the surrounding neighbor nodes
				{
					if (!neighbor.walkable || closedSet.Contains(neighbor))	// if the neighbor nodes are not walkable or if the neighbors are already visited then
					{
						continue;											// continue
					}

					int newCostToNeighbor = node.gCost + GetDistance(node, neighbor);			// calculate the new gcost
					if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))		// if the the new path to the neighbor is shorter or neighbor is closed
					{
						neighbor.gCost = newCostToNeighbor;										// set the g and h cost of the neighbor
						neighbor.hCost = GetDistance(neighbor, targetNode);
						neighbor.parent = node;													// set the parent node to current

						if (!openSet.Contains(neighbor))										// any nodes which are still open
							openSet.Add(neighbor);												// add those to the set of open nodes
						else
							openSet.UpdateItem(neighbor);										// otherwise if they are closed then update
					}
				}
			}
		}
		yield return null;
		if(pathSuccess)
        {
			waypoints = RetracePath(startNode, targetNode);

		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;

	}

	Vector3[] SimplifyPath(List<Node> path)
    {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for(int i = 1; i < path.Count; i++)
        {
			Vector2 directionNew = new Vector2(path[i - 1].gridX 
				- path[i].gridX, 
				path[i - 1].gridY 
				- path[i].gridY);
			if(directionNew != directionOld)
            {
				waypoints.Add(path[i].worldPosition);
            }
			directionOld = directionNew;
		}
		return waypoints.ToArray();
    }

	int GetDistance(Node nodeA, Node nodeB)															// heuristic evaluation - determines the x and y values from enemy to the player
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);											
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
		{
			return 12 * dstY + 7 * (dstX - dstY);
		}
		else
        {
			return 12 * dstX + 7 * (dstY - dstX);

		}



	}
}
