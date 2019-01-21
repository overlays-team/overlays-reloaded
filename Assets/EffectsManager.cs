using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public GameObject[] effects;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomEffect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRandomEffect()
    {
        int randomIndex = Random.Range(0, effects.Length);
        foreach(GameObject effect in effects)
        {
            effect.SetActive(false);
        }
        effects[randomIndex].SetActive(true);
    }
}
