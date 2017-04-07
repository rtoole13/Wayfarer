using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    public static void RequestPath(Node startNode, Node endNode, int xDir, int yDir, Action<Node[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(startNode, endNode, xDir, yDir, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.startNode, currentPathRequest.endNode, currentPathRequest.xDir, currentPathRequest.yDir);
        }
    }

    public void FinishedProcessingPath(Node[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Node startNode;
        public Node endNode;
        public Action<Node[], bool> callback;
        public int xDir;
        public int yDir;
        public PathRequest(Node _startNode, Node _endNode, int _xDir, int _yDir, Action<Node[], bool> _callback)
        {
            startNode = _startNode;
            endNode = _endNode;
            xDir = _xDir;
            yDir = _yDir;
            callback = _callback;
        }
    }
}
