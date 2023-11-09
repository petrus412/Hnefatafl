using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager myScript = (GridManager)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.EditorCreateGrid();
        }
        else if (GUILayout.Button("ClearList"))
        {
            myScript.ClearList();
        }
    }
}

