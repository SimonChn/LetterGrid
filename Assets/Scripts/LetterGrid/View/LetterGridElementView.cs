using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class LetterGridElementView : MonoBehaviour, IGridElementView
{
    private const int minCharIndex = 65;
    private const int maxCharIndex = 90;

    private RectTransform rectTransform;

    public Vector2 Position
    {
        get
        {
            return rectTransform.anchoredPosition;
        }

        set
        {
            rectTransform.anchoredPosition = value;
        }
    }

    public void Setup(Vector2 size, Vector2 anchorMin, Vector2 anchorMax)
    {
        rectTransform.sizeDelta = size;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }

    public void UpdateValue()
    {
        int charIndex = UnityEngine.Random.Range(minCharIndex, maxCharIndex + 1);

        rectTransform = GetComponent<RectTransform>();
        GetComponent<TextMeshProUGUI>().text = Convert.ToChar(charIndex).ToString();
    }
}