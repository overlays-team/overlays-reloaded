/*Author: Konstantin Regenhardt*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour {
    [Header("Assets->Prefabs->Gameplay->gridsystem->BackgroundGridPlane")]
    [Tooltip("Insert tile to be instanciated here.")]
    public GameObject tile;
    [Header("(Margins in World Coordinates)")]
    [Tooltip("This is the margin from the borders of the screenspace.")]
    public float borderMargin;
    [Tooltip("Margin between the tiles.")]
    public float cellMargin;
    [Tooltip("How far the grid is going to be away from the camera.")]
    public float zPos;

    private float width;
    private float height;
    private Camera cam;
    //CamToWorldCoordinates
    private Vector3 minWorldV3;
    private Vector3 maxWorldV3;

    // Use this for initialization
    void Start () {
        width = Screen.width;
        height = Screen.height;
        cam = Camera.main;
        minWorldV3 = cam.ScreenToWorldPoint(new Vector3(0, 0, zPos));
        maxWorldV3 = cam.ScreenToWorldPoint(new Vector3(width, height, zPos));
        print("MinV3: " + minWorldV3 + "\nMaxV3: " + maxWorldV3);

        for (float x = minWorldV3.x + borderMargin; x <= maxWorldV3.x - borderMargin; x += cellMargin)
        {
            print("First Loop");
            for(float y = minWorldV3.y + borderMargin; y <= maxWorldV3.y - borderMargin; y += cellMargin)
            {
                print("Second Loop");
                tile.name = "BGPlane" + x.ToString() + " " + y.ToString();
                Instantiate(tile, new Vector3(x, y, 0.6f), Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
