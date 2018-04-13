using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectPlacer))]
public class ObjectPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPlacer myScript = (ObjectPlacer)target;
        if (GUILayout.Button("Place Objects"))
        {
            myScript.PlaceObjects();
        }
        if (GUILayout.Button("Clear Objects"))
        {
            myScript.ClearObjects();
        }
    }
}


