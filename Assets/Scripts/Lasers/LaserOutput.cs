using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOutput : MonoBehaviour {

    /*
     * Jedes BlockObject, welches Laser rausschießt hat einen oder mehrere LaserOutputs
     * Abhängig davon wie die Rotation eines Laser  outputs ist, so werden auch die Laser rausgeschossen
     */

    public GameObject laserPrefab;
    public Laser laser;
    public bool active = false;
    [SerializeField]
    private Color activecolor = new Color(1, 0, 0); //if laser output is active
    [SerializeField]
    private Color inactivecolor = new Color(1, 1, 1);//if laser output is inactive

    void Awake () {
        laser = Instantiate(laserPrefab).GetComponent<Laser>();
        laser.active = false;
        laser.startingBlock = transform.parent.GetComponent<BlockObject>();
        laser.laserOutput = transform;  //output point sets direction, can be at 000 transform of parent
	}
	
	void Update () {
        if (active)
        {
            laser.active = true;
            ChangeMaterial(activecolor);
        }
        else
        {
            laser.active = false;
            ChangeMaterial(inactivecolor);
        }
	}

    public void SetLaser(Transform startPoint, BlockObject startingBlock)
    {
        laser.laserOutput = startPoint;
        laser.startingBlock = startingBlock;
    }

    //chnages emission (glow effect) in each children of graphics
    void ChangeMaterial(Color emissioncolor)
    {
        if(this.transform.childCount == 0)
        {
            return;
        }
        Transform graphics = this.gameObject.transform.GetChild(0);
        foreach(Transform child in graphics)
        {
            GameObject gochild = child.gameObject;
            Renderer meshrenderer = gochild.GetComponent<Renderer>();
            if (meshrenderer != null)
            {
                meshrenderer.material.SetColor("_Color", emissioncolor); //_EmissionColor
            }
        }
    }


}
