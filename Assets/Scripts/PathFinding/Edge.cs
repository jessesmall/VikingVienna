using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge{

    public Node nodeA;
    public Node nodeB;
    public float weight;
    public enum EdgeType { TL, Top, TR, Left, Right, BL, Bottom, BR};
    public EdgeType edgeType;

    public Edge(Node a, Node b, float weight, EdgeType edgeType)
    {
        nodeA = a;
        nodeB = b;
        this.weight = weight;
        this.edgeType = edgeType;
    }
}
