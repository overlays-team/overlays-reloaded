using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageInputSandbox : ImageInput
{

    [Header("Image Input Sandbox")]
    [SerializeField]
    [Tooltip("this panel shows the detailed node view and can contain further options for settings in the nodes - used for sandbox mode")]
    GameObject detailedNodeView;

    protected override void DoubleClickAction()
    {
        detailedNodeView.SetActive(true);
    }
}
