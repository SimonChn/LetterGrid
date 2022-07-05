using System;
using UnityEngine;

using URandom = UnityEngine.Random;

public class LetterGridController : MonoBehaviour
{
    private GridGenerator gridGenerator;
    private IGridElementViewMover elementsMover;

    private LetterElementsPoolController elementsPoolController;

    private GridProperties gridProperties;
    private LetterGridElementView[] elements;
    private int[] order;

    private bool isBusy = false;

    [SerializeField] private Transform gridParent;

    [Space(20)]
    [SerializeField] private Canvas spawnCanvas;
    [SerializeField] private Transform poolParent;
    [SerializeField] private LetterGridElementView gridElementPrefab;

    public Action OnBusyStarted;
    public Action OnBusyFinished;

    public void Init(GridGenerator gridGenerator, IGridElementViewMover elementsMover)
    {
        this.gridGenerator = gridGenerator;
        this.elementsMover = elementsMover;

        OnBusyFinished = delegate { isBusy = false; };
    }

    public void SetNewMover(IGridElementViewMover newMover)
    {
        elementsMover = newMover;
    }

    public void CreateNewGrid(int columns, int rows)
    {
        SetNewGrid(gridGenerator.GenerateNewGrid(rows, columns));
    }

    private void SetNewGrid(GridProperties gridProperties)
    {
        this.gridProperties = gridProperties;

        if (elements == null)
        {
            elementsPoolController = new LetterElementsPoolController(poolParent, gridElementPrefab, 
                gridProperties.TotalSize, spawnCanvas.transform.localScale.x);
        }
        else
        {
            elementsMover.ForceStop();
            ClearElements();
        }

        elements = new LetterGridElementView[gridProperties.TotalSize];
        order = new int[gridProperties.TotalSize];
        order.FillArrayWithIndexes();

        for (int i = 0; i < gridProperties.TotalSize; i++)
        {
            var newGridElement = elementsPoolController.GetElement();
            newGridElement.UpdateValue();

            newGridElement.transform.SetParent(gridParent);
            newGridElement.Setup(gridProperties.ElementSize, Vector2.zero, Vector2.zero);

            elements[i] = newGridElement;
        }

        TryToSetBusyState();
        elementsMover.MoveElementsToStartPositions(elements, gridProperties, OnBusyFinished);
    }

    public void ShuffleElements()
    {
        elementsMover.ForceStop();
        int[] shuffledPlaces = new int[order.Length];
        order.CopyTo(shuffledPlaces, 0);

        shuffledPlaces.ShuffleDistinct();

        for(int i = 0; i < shuffledPlaces.Length; i++)
        {
            order[shuffledPlaces[i]] = i;
        }

        TryToSetBusyState();
        elementsMover.MoveElementsToPositions(elements, shuffledPlaces, gridProperties, OnBusyFinished);
    }

    public void ShuffleRandomRow()
    {
        if(gridProperties.RowLength == 1)
        {
            return;
        }

        int rowIndex = URandom.Range(0, gridProperties.ColumnLength);

        int fromIndex = gridProperties.RowLength * rowIndex;
        int toIndex = fromIndex + gridProperties.RowLength - 1;

        LetterGridElementView[] shuffledElements = new LetterGridElementView[gridProperties.RowLength];
        int[] newElementsOrder = new int[shuffledElements.Length];

        order.Shuffle(fromIndex, toIndex, distinct: true);

        for (int i = 0; i < gridProperties.RowLength; i++)
        {
            shuffledElements[i] = elements[order[fromIndex + i]];
            newElementsOrder[i] = fromIndex + i;
        }

        TryToSetBusyState();
        elementsMover.MoveElementsToPositions(shuffledElements, newElementsOrder, gridProperties, OnBusyFinished);
    }

    private void TryToSetBusyState()
    {
        if (!isBusy)
        {
            isBusy = true;
            OnBusyStarted?.Invoke();
        }
    }

    private void ClearElements()
    {
        for(int i = 0; i < elements.Length; i++)
        {
            elementsPoolController.ReleaseElement(elements[i]);
        }
    }

    private void OnDisable()
    {
        if(elementsPoolController != null)
        {
            elementsPoolController.DestroySelf();
            elementsPoolController = null;
        }   

        OnBusyStarted = null;
        OnBusyFinished = null;
    }
}