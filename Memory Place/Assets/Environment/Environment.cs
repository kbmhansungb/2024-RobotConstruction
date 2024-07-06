using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Environment : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Volume volume;
    [SerializeField] private Volume fadeInOutVolume;

    public void Init()
    {
        
    }

    public void FadeInOut(float totalDuration, Action outCallback, Action inCallback)
    {
        GameManager.Instance.StartCoroutine(FadeInOutCoroutine(totalDuration, outCallback, inCallback));
    }

    private IEnumerator FadeInOutCoroutine(float totalDuration, Action outCallback, Action inCallback)
    {
        float halfDuration = totalDuration / 2.0f;

        float elapsedTime = 0.0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / halfDuration;

            fadeInOutVolume.weight = Mathf.Lerp(0.0f, 1.0f, t);

            yield return null;
        }

        // 완전히 불투명해지기 위해 한 프레임을 더 기다립니다.
        fadeInOutVolume.weight = 1.0f;
        yield return null; 

        outCallback?.Invoke();

        elapsedTime = 0.0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / halfDuration;

            fadeInOutVolume.weight = Mathf.Lerp(1.0f, 0.0f, t);

            yield return null;
        }

        // 완전히 투명해지기 위해 한 프레임을 더 기다립니다.
        fadeInOutVolume.weight = 0.0f;
        yield return null;

        inCallback?.Invoke();
    }
}
