using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    PathGrid gridPath;

    private void Awake()
    {
        gridPath = GetComponent<PathGrid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = gridPath.NodeFromWorldPoint(request.pathStart);
        Node targetNode = gridPath.NodeFromWorldPoint(request.pathEnd);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(gridPath.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Edge edge in gridPath.GetNeighbors(currentNode))
                {
                    Node neighbor = edge.nodeB;
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int airWeight = 0;
                    if (!gridPath.IsNodeGround(edge.nodeB))
                        airWeight = 4;

                    int newMovementCost = currentNode.gCost + GetDistance(currentNode, neighbor) + (int)edge.weight + airWeight;

                    if (newMovementCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCost;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
            pathSuccess = wayPoints.Length > 0;
        }
        callback(new PathResult(wayPoints, pathSuccess, request.controller, request.callback));
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //Vector3[] waypoints = SimplifyPath(path);

        List<Vector3> waypoints = new List<Vector3>();
        foreach(var node in path)
        {
            waypoints.Add(new Vector3(node.XCoordinate, node.YCoordinate));
        }
        waypoints.Reverse();
        return waypoints.ToArray();
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector3 directionNew = new Vector3(path[i - 1].XCoordinate - path[i].XCoordinate, path[i - 1].YCoordinate - path[i].YCoordinate);
            if(directionNew != directionOld)
            {
                waypoints.Add(new Vector3(path[i].XCoordinate, path[i].YCoordinate));
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    // Calculate distance based on A*
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.x - nodeB.x);
        int distY = Mathf.Abs(nodeA.y - nodeB.y);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
