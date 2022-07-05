using System;
using UnityEngine;

public class LetterElementsPoolController 
{
    private Transform parent;
    private LetterGridElementView elementPrefab;

    private ObjectsPool<LetterGridElementView> elementsPool;

    private Vector3 scaledSize;

    public LetterElementsPoolController(Transform parent, LetterGridElementView elementPrefab, int startCapacity, float canvasScale)
    {
        this.parent = parent;
        this.elementPrefab = elementPrefab;
        scaledSize = Vector3.one * canvasScale;

        elementsPool = new ObjectsPool<LetterGridElementView>(CreateElement, OnElementReceived, OnElementReleased, startCapacity + 1);
    }

    public LetterGridElementView GetElement()
    {
        if(elementsPool.TryToGetPoolElement(out var element))
        {           
            return element;
        }
        else
        {
            throw new NullReferenceException();
        }
    }

    public void ReleaseElement(LetterGridElementView element)
    {
        elementsPool.Release(element);
    }

    public void DestroySelf()
    {
        parent = null;
        elementPrefab = null;

        elementsPool.DestroySelf();
    }

    private LetterGridElementView CreateElement()
    {
        var newElement = UnityEngine.Object.Instantiate(elementPrefab, parent, false);
        newElement.transform.localScale = scaledSize;
        newElement.gameObject.SetActive(false);

        return newElement;
    }

    private void OnElementReceived(LetterGridElementView element)
    {
        element.gameObject.SetActive(true);
    }

    private void OnElementReleased(LetterGridElementView element)
    {
        element.gameObject.SetActive(false);
        element.transform.SetParent(parent);
    }
}
