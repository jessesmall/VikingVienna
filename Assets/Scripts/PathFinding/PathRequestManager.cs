using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PathRequestManager : MonoBehaviour {

    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    PathFinding pathFinding;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    private void Update()
    {
        if(results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success, result.controller);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathFinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public StateController controller;
    public Action<Vector3[], bool, StateController> callback;

    public PathResult(Vector3[] path, bool success, StateController controller, Action<Vector3[], bool, StateController> callback)
    {
        this.path = path;
        this.success = success;
        this.controller = controller;
        this.callback = callback;
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public StateController controller;
    public Action<Vector3[], bool, StateController> callback;

    public PathRequest(Vector3 _start, Vector3 _end, StateController _controller, Action<Vector3[], bool, StateController> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        controller = _controller;
        callback = _callback;
    }
}
