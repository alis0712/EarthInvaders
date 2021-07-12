using UnityEngine;
using System.Collections;
using System;

// Reference COSC 504 Heaps
public class heapminmax<T> where T : IHeapNode<T> // Takes in a type T and implements the interface given in line 152
{

	T[] nodes;									 // array of nodes
	int currentNodeCount;						 // keeps track of current node count

	public heapminmax(int maxHeapSize)			 // constructor
	{
		nodes = new T[maxHeapSize];				 // takes in the integer for max heap size (grid size x and grid size y)
	}

	public void Add(T node)						 // add new nodes to the heap
	{
		node.HeapIndex = currentNodeCount;		// keeps track of nodes visited
		nodes[currentNodeCount] = node;			// once every node is visited add new nodes to the end of the node array
		SortUp(node);							// method that keeps comparing the parent node to the child in terms of priority
		currentNodeCount++;						// increment current node count by one
	}

	public T Remove()							// method that remove items based on the priority 
	{
		T firstNode = nodes[0];					// calls the first item in the child node

		currentNodeCount--;						// one less item 					
		nodes[0] = nodes[currentNodeCount];		// take the bottom child based on the priority and put it on the parent so that the some children will not longer be visited
		nodes[0].HeapIndex = 0;					// set the heap index to 0
		SortDown(nodes[0]);						// now call method sort down
		return firstNode;
	}

	public void UpdateItem(T node)				// changes the priority of the item 
	{
		SortUp(node);
	}

	public int Count							 // returns the number of nodes in the tree
	{
		get
		{
			return currentNodeCount;
		}
	}

	public bool Contains(T node)			    // checks if the heap contains a specific node	
	{
		return Equals(nodes[node.HeapIndex], node);	 // checks if the heap index value is the same as node value
	}

	void Evaluate(T alpha, T beta)				// calls for alpha and beta value
	{
		nodes[alpha.HeapIndex] = beta;			// first swap in the array
		nodes[beta.HeapIndex] = alpha;
		int alphaIndex = alpha.HeapIndex;		// temporary integer															
		alpha.HeapIndex = beta.HeapIndex;	    // swap heap index values 
		beta.HeapIndex = alphaIndex;			// set the heap index to the alpha value
	}

	void SortDown(T node)
	{
		while (true)
		{
			int childIndexLeft = node.HeapIndex * 2 + 1;			// children of the parent
			int childIndexMiddle = node.HeapIndex * 2 + 2;
			int childIndexRight = node.HeapIndex * 2 + 3;

			int evaluateIndex = 0;

			if (childIndexLeft < currentNodeCount)					// if a left child exists then
			{
				evaluateIndex = childIndexLeft;						// then evaluate the left child

				if (childIndexRight < currentNodeCount)				// if a right child exists then
				{
					if (nodes[childIndexLeft].CompareTo(nodes[childIndexRight]) < 0)	// check the priority of the child
					{
						evaluateIndex = childIndexRight;								// set the evaluate index to the right child (change default)
					}
					if (nodes[childIndexLeft].CompareTo(nodes[childIndexRight]) > 0)
					{
						evaluateIndex = childIndexRight;
					}
				}
				if (childIndexMiddle < currentNodeCount)
				{
					if (nodes[childIndexLeft].CompareTo(nodes[childIndexMiddle]) < 0)
					{
						evaluateIndex = childIndexMiddle;
					}
					if (nodes[childIndexLeft].CompareTo(nodes[childIndexMiddle]) > 0)
					{
						evaluateIndex = childIndexMiddle;
					}
				}

				if (node.CompareTo(nodes[evaluateIndex]) < 0)					// if the child has a higher priority than its parent then swap child's value with the parent			
				{
					Evaluate(node, nodes[evaluateIndex]);
				}
				if (node.CompareTo(nodes[evaluateIndex]) > 0)                   // if the child has a lower priority than its parent then swap child's value with the parent			
				{
					Evaluate(node, nodes[evaluateIndex]);
				}
				else
				{
					return;														// if the priorities are the same then don't do anything
				}

			}
			else 
			{
				return;															// if there are no children then return						
			}

		}
	}


	void SortUp(T node)
	{
		int parentIndex = (node.HeapIndex - 1) / 2;								// integer for parent index

		while (true)
		{
			T parentNode = nodes[parentIndex];									// variable T for parent node and creates an array for the parent indexes								
			if (node.CompareTo(parentNode) > 0)									// compare the parent node to the current node in terms prioirity (> 0 = 1 < 0 = -1 in terms of f cost)
			{
				Evaluate(node, parentNode);										// call evaluate method and evaluate current node with parent node with current and replace
			}
			if (node.CompareTo(parentNode) < 0)
			{
				Evaluate(node, parentNode);
			}
			else
			{
				break;															// once the priorities are determined break out of the loop
			}

			parentIndex = (node.HeapIndex - 1) / 2;								// if not priorities are not determined then recalculate
		}
	}





}
public interface IHeapNode<T> : IComparable<T>									// compares the nodes with higher or lower prirority
{
	int HeapIndex
	{
		get;
		set;
	}
}