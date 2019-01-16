using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{

    public float animationSpeed = 1; //the lager the value, the slower the fading
    public Image img;
    public AnimationCurve curve;

    private void Start()
    {
        //StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    public void FadeToNextScene(int sceneIndex)
    {
        StartCoroutine(FadeOutWithIndex(sceneIndex));
    }

    public void FadeToClear()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime * animationSpeed; //t decrease by every single frame
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0; //break. wait unti the next frame and continue
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed; //t increase by every single frame
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0; //break. wait unti the next frame and continue
        }

        SceneManager.LoadScene(scene);
    }

    IEnumerator FadeOutWithIndex(int sceneIndex)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed; //t increase by every single frame
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0; //break. wait unti the next frame and continue
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
