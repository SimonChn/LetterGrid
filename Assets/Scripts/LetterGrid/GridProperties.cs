using UnityEngine;

public struct GridProperties
{
    private Vector2Int gridSize;
    private Vector2 startPosition;
    
    public Vector2 GridRectSize { get; private set; }
    public Vector2 ElementSize { get; private set; }
    public float SpaceBetweenElements { get; private set; }

    public int TotalSize => (gridSize.x * gridSize.y);
    public int RowLength => gridSize.x;
    public int ColumnLength => gridSize.y;

    public GridProperties(Vector2 gridRectSize, Vector2Int gridSize, Vector2 startPosition, Vector2 elementSize, float spaceBetweenElements)
    {
        this.gridSize = gridSize;
        this.startPosition = startPosition;

        GridRectSize = gridRectSize;
        ElementSize = elementSize;
        SpaceBetweenElements = spaceBetweenElements;
    }

    public Vector2 GetGridElementPosition(int index)
    {
        float x = startPosition.x + SpaceBetweenElements * (index % gridSize.x);
        float y = startPosition.y + SpaceBetweenElements * (index / gridSize.x);
        return new Vector2(x, y);
    }
}