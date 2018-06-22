using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathGrid : MonoBehaviour {

    public int width;
    public int height;
    public float size = 1f;

    public LayerMask unWalkableMask;

    public Grid grid;

    private Node[,] _nodes;
    public Node[,] Nodes
    {
        get { return _nodes; }
        set { _nodes = value; }
    }

    public int MaxSize
    {
        get
        {
            return (int)(width * height / size);
        }
    }

    public bool ShowConnections = false;
    public bool ShowGrid = false;

    private void Awake()
    {
        InitializeGrid();
    }

    private void CreateGrid()
    {
        Nodes = new Node[(int)(width / size), (int)(height / size)];
        for(int x = 0; x < width/size; x++)
        {
            for(int y = 0; y < height/size; y++)
            {
                Nodes[x, y] = new Node(x, y, this, size);
            }
        }
    }

    private void ClearGrid()
    {
        Nodes = null;
    }

    public void InitializeGrid()
    {
        ClearGrid();
        CreateGrid();
        InitializeConnections();
    }

    public void InitializeConnections()
    {
        if(Nodes != null)
        {
            foreach (var node in Nodes)
            {
                node.CheckNeighbors(ShowConnections);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(Nodes != null && ShowGrid)
        {
            foreach(Node n in Nodes)
            {
                Gizmos.color = (n.walkable) ? Color.green : Color.red;
                Gizmos.DrawCube(new Vector2(n.XCoordinate, n.YCoordinate), Vector3.one * 0.1f);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPoint)
    {
        float percentX = (worldPoint.x - this.transform.position.x) / width;
        float percentY = (worldPoint.y - this.transform.position.y) / height;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(((width / size) - 1) * percentX);
        int y = Mathf.RoundToInt(((height / size) - 1) * percentY);

        if(Nodes[x,y].XCoordinate % 1 != 0)
        {
            x++;
        }
        if(Nodes[x,y].YCoordinate % 1 != 0)
        {
            y++;
        }

        return Nodes[x, y];
    }

    public List<Edge> GetNeighbors(Node node)
    {
        return Nodes[node.x, node.y].nodeEdges;
    }

    public bool IsNodeGround(Node node)
    {
        var bottomNode = node.nodeEdges.Where(x => x.edgeType == Edge.EdgeType.Bottom).SingleOrDefault();
        if(bottomNode != null && !bottomNode.nodeB.walkable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
