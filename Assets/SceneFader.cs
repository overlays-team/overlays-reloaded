using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneFader : MonoBehaviour {


    public Image img;
    public AnimationCurve curve;

    private void Start()
    {
        StartCoroutine(FadeIn());

    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    private readonly float speedFactor = 2f; //the lager the value, the slower the fading
    IEnumerator FadeIn()
    {
        float t = 1f * speedFactor;

        while (t > 0)
        {
            t -= Time.deltaTime; //t decrease by every single frame

            float a = curve.Evaluate(t);

            img.color = new Color(0f, 0f, 0f, a);
            yield return 0; //break. wait unti the next frame
        }
    }


    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime; //t increase by every single frame

            float a = curve.Evaluate(t);

            img.color = new Color(0f, 0f, 0f, a);
            yield return 0; //break. wait unti the next frame
        }

        SceneManager.LoadScene(scene);
    }


}
