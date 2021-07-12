using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapNode<Node>                         // implements the heap min max class
{
    public bool walkable;                                   // two states walkable or not
    public Vector3 worldPosition;                           // what point in the world this node represents
    public int gridX;
    public int gridY;

    public int gCost;                                           // node's g cost
    public int hCost;                                           // node's h cost
    public Node parent;                                         // calling the parent node and comparing the child's fcost to the parent's fcost
    int heapIndex;                                              // setting up a heap index

    public Node(bool _walkable, 
        Vector3 _worldPos, int _gridX, int _gridY)              // constructor
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost                                           // total cost which combines fcost and hcost                                                      
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex                                      // assigning an index to the trees
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)                  // compares the f cost of each node
    {
        int compare = fCost.CompareTo
            (nodeToCompare.fCost);         
        if(compare == 0)                                      // if the f costs are the same
        {
            compare = hCost.CompareTo
                (nodeToCompare.hCost);                        // then compare h costs
        }
        return -compare;                                      // return 1 since we are looking for the max priority          
    }



}
