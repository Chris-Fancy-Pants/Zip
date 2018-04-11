using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TreeStrip))]
public class TreeStripEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TreeStrip myScript = (TreeStrip)target;
        if (GUILayout.Button("Place Trees"))
        {
            myScript.PlaceTrees();
        }
        if (GUILayout.Button("Clear Trees"))
        {
            myScript.ClearTrees();
        }
    }
}


