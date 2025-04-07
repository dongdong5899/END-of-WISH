using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSY_FadeCircleUI : MonoBehaviour
{
    [SerializeField] private float FadeTime = 1f;
    private Image circleImage;

    Vector2 fullSizeVector = new Vector2(3840, 2160);

    private void Awake()
    {
        circleImage = GetComponent<Image>();
    }

    private void Start()
    {
        FadeCircle(true);
    }
    public void FadeCircle(bool isFadeIn)
    {
        StartCoroutine(ChangeCircleScale());
        
    }

    private IEnumerator ChangeCircleScale(bool isFadeIn = true)
    {
        Vector2 startSize = Vector2.zero, endSize = Vector2.zero;
        if(isFadeIn) endSize = fullSizeVector;
        else startSize = fullSizeVector;

        float currentTime = 0f;
        while(currentTime < FadeTime)
        {
            currentTime += Time.deltaTime;
            circleImage.rectTransform.sizeDelta =
                Vector2.Lerp(startSize, endSize, currentTime/FadeTime);
            yield return null;
        }
        yield return null;
    }
}
