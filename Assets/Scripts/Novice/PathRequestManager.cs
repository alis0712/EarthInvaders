using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Reference: https://www.youtube.com/watch?v=dn1XRIaROM4

public class PathRequestManager : MonoBehaviour
{

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>(); // Create a queue of path requests
	PathRequest currentPathRequest; // store the current path request

	static PathRequestManager instance; // For accessing from RequestPath
	//PathfindingMinMax pathfinding;
	PathfindingAstar pathfindingAstar;

	bool isProcessingPath;

	void Awake()
	{
		instance = this;
		//pathfinding = GetComponent<PathfindingMinMax>();
		pathfindingAstar = GetComponent<PathfindingAstar>();


	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) // processes a path request using Unity's built in Action which stores arrays and call them back where there is a need and tells whether the path request was successful
	{
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newRequest); //Adding the new request
		instance.ProcessNext();
	}

	void ProcessNext()
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0) // if path is not processing and the path request queue is not empty 
		{
			currentPathRequest = pathRequestQueue.Dequeue(); // obtain the first item in the queue
			isProcessingPath = true; // process the path
			pathfindingAstar.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // start the path and end the path
		}
	}

	public void FinishedProcessingPath(Vector3[] path, bool success) // finishes processing the parth
	{
		currentPathRequest.callback(path, success);
		isProcessingPath = false;	// once done move onto the next							
		ProcessNext();
	}

	struct PathRequest // data structure holding all the variables
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) // constructor
		{
			pathStart = _start; 
			pathEnd = _end;
			callback = _callback;
		}

	}
}