using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpGroup : MonoBehaviour 
{	
	public float cutOffDelta = 0.1f;
	Transform[] objects;
	Vector3[] targetSizes;
	Vector2 cutOff;

	void Start () 
	{
        Transform[] allTransforms = GetComponentsInChildren<Transform>();
        List<Transform> firstDepthTransforms = new List<Transform>();

        foreach (Transform trans in allTransforms)
        {
            if (trans.parent.gameObject == gameObject)
            {
                firstDepthTransforms.Add(trans);
            }
        }

        objects = firstDepthTransforms.ToArray();

        List<Transform> temp = new List<Transform>(objects);
		temp.Remove(objects[0]);
		objects = temp.ToArray();

		targetSizes = new Vector3[objects.Length];

		for(int i = 0; i < objects.Length; i++)
		{
			targetSizes[i] = objects[i].localScale;
			objects[i].localScale = Vector3.zero;
		}

		cutOff = findLowestObject();
	}
	
	void FixedUpdate () 
	{
		cutOff.x += cutOffDelta;
		cutOff.y += cutOffDelta;

		for(int i = 0; i < objects.Length; i++)
		{
			if(objects[i].localPosition.x < cutOff.x && objects[i].localPosition.z < cutOff.y)
			{
				objects[i].localScale = Vector3.Lerp(objects[i].localScale, targetSizes[i], 0.1f);
			}
		}
	}

	Vector2 findLowestObject()
	{
		List<Transform> sorted = new List<Transform>(objects);
		
		sorted.Sort((a, b) => a.localPosition.x.CompareTo(b.localPosition.x));
		float lowestX = sorted[0].localPosition.x;

		sorted.Sort((a, b) => a.localPosition.z.CompareTo(b.localPosition.z));
		float lowestY = sorted[0].localPosition.z;

		return new Vector2(lowestX, lowestY);
	}
}
