using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class LetterGridElementView : MonoBehaviour
{
    private const int minCharIndex = 65;
    private const int maxCharIndex = 90;

    private RectTransform rectTransform;

    public Vector2 Positon => rectTransform.anchoredPosition;

    public void Init()
    {
        UpdateValue();
    }

    public void SetRectTransform(Vector2 size, Vector2 anchorMin, Vector2 anchorMax)
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

    public void SetAnchoredPosition(Vector2 newPosition)
    {
        rectTransform.anchoredPosition = newPosition;
    }
}
