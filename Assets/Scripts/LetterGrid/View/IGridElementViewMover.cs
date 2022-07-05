using System;

public interface IGridElementViewMover 
{
    public void ForceStop();

    public void MoveElementsToStartPositions(IGridElementView[] elements, GridProperties gridProperties, Action onEndCallback);

    public void MoveElementsToPositions(IGridElementView[] elements, int[] targetGridIndexes, GridProperties gridProperties, Action onEndCallback);
}
