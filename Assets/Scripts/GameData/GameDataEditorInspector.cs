using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(GameDataEditor))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
		GameDataEditor myScript = (GameDataEditor)target;

		//EditorGUILayout.Space();

		//if(GUILayout.Button("Show Data from File"))
  //      {
  //          //myScript.LoadData();
  //          myScript.LoadData();
  //          Repaint();
  //      }

		//EditorGUILayout.Space();

  //      if(GUILayout.Button("Save Data to File"))
  //      {
  //          myScript.SaveData();
  //      }

		EditorGUILayout.Space();

		if(GUILayout.Button("Clear Data"))
        {
            myScript.ResetData();
			Repaint();
        }

		EditorGUILayout.Space();
    }
}
#endif

