using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageInputSandbox : ImageInput
{
    protected override void Start()
    {
        base.Start();
        detailedNodeViewImage.sprite = debugImage.sprite;
    }

    public void OnImportButtonClicked()
   {
        Debug.Log("import Button clicked");
   }
  
}
