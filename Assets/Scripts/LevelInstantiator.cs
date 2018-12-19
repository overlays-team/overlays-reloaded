using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInstantiator : MonoBehaviour {

    public static LevelInstantiator CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LevelInstantiator>(jsonString);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
