using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathGrid))]
public class PathGridEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathGrid script = (PathGrid)target;
        if(GUILayout.Button("Initialize Grid"))
        {
            script.InitializeGrid();
        }
    }
}
