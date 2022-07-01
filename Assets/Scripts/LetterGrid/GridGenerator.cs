using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private IGridController gridView;

    private Vector2 gridRectSize;
    private Vector2 gridCenter;
    private Vector2 clampedGridArea;

    [SerializeField] private Vector2 borderOffset;
    [SerializeField] private RectTransform gridParent;

    public void Init()
    {
        gridRectSize = gridParent.rect.size;
        gridCenter = gridRectSize / 2f;
        clampedGridArea = gridRectSize - borderOffset;
    }

    public void SetGridController(IGridController gridController)
    {
        this.gridView = gridController;
    }

    public void GenerateNewGrid(int horizontalElementsNumber, int verticalElementsNumber)
    {
        if (horizontalElementsNumber < 0) horizontalElementsNumber = 1;
        if (verticalElementsNumber < 0) verticalElementsNumber = 1;

        Vector2Int gridSize = new Vector2Int(horizontalElementsNumber, verticalElementsNumber);

        float minSideLength = Mathf.Min(clampedGridArea.x, clampedGridArea.y);
        int maxElementsInOrder = (Mathf.Max(horizontalElementsNumber, verticalElementsNumber));

        float elementSideLength = minSideLength / maxElementsInOrder / 2f;
        Vector2 elementSize = new Vector2(elementSideLength, elementSideLength);

        float spaceBetweenElements = (minSideLength - elementSideLength) / (maxElementsInOrder);

        float startHorizontalCoordinate = gridCenter.x - (spaceBetweenElements * (horizontalElementsNumber - 1) / 2f);
        float startVerticalCoordinate = gridCenter.y - (spaceBetweenElements * (verticalElementsNumber - 1) / 2f);
        Vector2 startSpawnPosition = new Vector2(startHorizontalCoordinate, startVerticalCoordinate);

        GridProperties gridProperties = new GridProperties(gridRectSize, gridSize, startSpawnPosition, elementSize, spaceBetweenElements);

        gridView.SetNewGrid(gridProperties);
    }

    private void OnValidate()
    {
        if(borderOffset.x < 0)
        {
            borderOffset.x = 0;
        }

        if(borderOffset.y < 0)
        {
            borderOffset.y = 0;
        }
        
        if(gridParent == null)
        {
            return;
        }

        Vector2 gridSize = gridParent.rect.size;

        float maxBorderPart = 0.25f;

        float maxX = gridSize.x * maxBorderPart;
        float maxY = gridSize.y * maxBorderPart;

        if (borderOffset.x > maxX)
        {
            borderOffset.x = maxX;
        }

        if(borderOffset.y > maxY)
        {
            borderOffset.y = maxY;
        }
    }
}