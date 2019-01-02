﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridPositioner))]
public class GridPositionerEditor : Editor
{

   
    public override void OnInspectorGUI()
    {
       
        DrawDefaultInspector();
        
        GridPositioner myScript = (GridPositioner)target;
        EditorUtility.SetDirty(myScript);
        if (GUILayout.Button("UpdatePlanes"))
        {
            myScript.UpdatePlanes();
        }
    }
}
