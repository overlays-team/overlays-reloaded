using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelToggle : MonoBehaviour {

    public LevelData level;
    public RectTransform selectedBar;
    public Image background;
    private Toggle toggle;
    private Animator anim;

	// Use this for initialization
	void Start () {
        toggle = GetComponent<Toggle>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Workaround: Have to manually reset the toggle values so it isn't preselected on enable/disabling the level select screen
    public void ResetToggle()
    {
        toggle.isOn = false;
        selectedBar.localScale = Vector3.zero;
        background.color = new Color(1, 1, 1, 0);
    }

    private void OnDisable()
    {
        ResetToggle();
    }

    public void SetAnimationState()
    {
        if (toggle.isOn)
        {
            anim.SetTrigger("Selected");
        }
        else
        {
            anim.SetTrigger("Normal");
        }
    }
}
