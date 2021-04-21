using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfindingGrid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathfindingGrid myScript = (PathfindingGrid)target;
        if (GUILayout.Button("CREATE GRID"))
        {
            myScript.CreateGrid();
        }
        if (GUILayout.Button("DESTROY GRID"))
        {
            myScript.DestroyGrid();
        }
        if (GUILayout.Button("SHOW/HIDE GRID"))
        {
            myScript.ShowHideGrid();
        }
    }
}
