using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : IHeapItem<Node> {

    // Coordinates
    public int x;
    public float XCoordinate
    {
        get { return _path.gameObject.transform.position.x + (x * _path.size); }
    }
    private int _tileX
    {
        get { return (int)Math.Floor(_path.gameObject.transform.position.x + (x * _path.size)); }
    }
    public int y;
    public float YCoordinate
    {
        get { return _path.gameObject.transform.position.y + (y * _path.size); }
    }
    private int _tileY
    {
        get { return (int)Mathf.Floor(_path.gameObject.transform.position.y + (y * _path.size)); }
    }

    // Heap
    private int _heapIndex;
    public int HeapIndex
    {
        get
        {
            return _heapIndex;
        }
        set
        {
            _heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    // Pathfinding Parameters
    public int gCost;
    public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public Node parent;

    public int movementPenalty;
    public bool walkable;

    private GameObject _nodeGUI;
    private PathGrid _path;

    public List<Edge> nodeEdges;

    public Node(int X, int Y, PathGrid grid, float size)
    {
        x = X;
        y = Y;

        _path = grid;

        nodeEdges = new List<Edge>();
        Initialize();
    }

    private void Initialize()
    {
        CheckWalkable();
    }

    private void CheckWalkable()
    {
        walkable = true;
        foreach(var tileMap in _path.grid.GetComponentsInChildren<Tilemap>())
        {
            if (_path.unWalkableMask == (_path.unWalkableMask.value | (1 << tileMap.gameObject.layer)))
            {
                if(XCoordinate % 1 == 0)
                {
                    if (tileMap.HasTile(new Vector3Int(_tileX, _tileY, 0)) || tileMap.HasTile(new Vector3Int(_tileX - 1, _tileY,0)))
                    {
                        walkable = false;
                    }
                }
                if(tileMap.HasTile(new Vector3Int(_tileX, _tileY, 0)))
                {
                    walkable = false;
                }
                //if(XCoordinate % 1 == 0)
                //{
                //    if(tileMap.HasTile(new Vector3Int((_tileX - 1), _tileY, 0)))
                //    {
                //        walkable = false;
                //    }
                //}
                //if (YCoordinate % 1 == 0)
                //{
                //    if (tileMap.HasTile(new Vector3Int((_tileX), (_tileY - 1), 0)))
                //    {
                //        walkable = false;
                //    }
                //}
            }
        }
    }

    public void CheckNeighbors(bool ShowConnections)
    {
            if (x > 0)
            {
                // Left
                nodeEdges.Add(CheckNeighbor(x - 1, y, 1, Edge.EdgeType.Left));
                if (y > 0)
                {
                    // Bottom Left
                    nodeEdges.Add(CheckNeighbor(x - 1, y - 1, 3f, Edge.EdgeType.BL));
                }
                if (y < (_path.Nodes.GetLength(1) - 1))
                {
                    // Top Left
                    nodeEdges.Add(CheckNeighbor(x - 1, y + 1, 3f, Edge.EdgeType.TL));
                }
            }
            if (x < (_path.Nodes.GetLength(0) - 1))
            {
                // Right
                nodeEdges.Add(CheckNeighbor(x + 1, y, 1, Edge.EdgeType.Right));
                if (y > 0)
                {
                    // Bottom Right
                    nodeEdges.Add(CheckNeighbor(x + 1, y - 1, 3f, Edge.EdgeType.BR));
                }
                if (y < (_path.Nodes.GetLength(1) - 1))
                {
                    // Top Right
                    nodeEdges.Add(CheckNeighbor(x + 1, y + 1, 3f, Edge.EdgeType.TR));
                }
            }
            if (y > 0)
            {
                // Bottom
                nodeEdges.Add(CheckNeighbor(x, y - 1, 4, Edge.EdgeType.Bottom));
            }
            if (y < (_path.Nodes.GetLength(1) - 1))
            {
                // Top
                nodeEdges.Add(CheckNeighbor(x, y + 1, 4, Edge.EdgeType.Top));
            }
    }

    private Edge CheckNeighbor(int x, int y, float weight, Edge.EdgeType type)
    {
        if (_path.Nodes[x, y] != null)
        {
            return new Edge(this, _path.Nodes[x, y], weight, type);
        }
        return null;
    }
}
